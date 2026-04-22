using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.UnitOfWorkContracts;

namespace TaskTracker.Infrastructure.Middlewares
{
    public class MenuAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MenuAuthorizationMiddleware> _logger;
        private readonly IMemoryCache _cache;
        private readonly ICommonRepository _commonRepository;

        public MenuAuthorizationMiddleware(RequestDelegate next, ILogger<MenuAuthorizationMiddleware> logger, IMemoryCache cache, ICommonRepository commonRepository)
        {
            _next = next;
            _logger = logger;
            _cache = cache;
            _commonRepository = commonRepository;
        }

        private static readonly Dictionary<string, (string Method, Func<UserMenuVM, bool> PermissionCheck)> ActionPermissionMap
            = new(StringComparer.OrdinalIgnoreCase)
        {
            { "GetGridData", ("POST", um => um.List) },
            { "GetList", ("POST", um => um.List) },
            { "Upsert", ("POST", um => um.Insert) },
            { "Delete", ("POST", um => um.Delete) },
            // Add more specific actions here if needed
        };

        public async Task InvokeAsync(HttpContext context, ITaskTrackerUnitOfWork unitOfWork)
        {
            try
            {
                //var memoryCache = (MemoryCache)_cache;
                //memoryCache.Compact(1.0); // Remove all entries

                var path = context.Request.Path.Value;

                if (!string.IsNullOrEmpty(path) && (path.StartsWith("/swagger") || path.StartsWith("/api-docs")))
                {
                    await _next(context);
                    return;
                }

                var endpoint = context.GetEndpoint();

                if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
                {
                    await _next(context);
                    return;
                }
                if (endpoint == null || endpoint.Metadata.GetMetadata<ControllerActionDescriptor>() == null)
                {
                    // Skip middleware for requests that are not controller actions
                    await _next(context);
                    return;
                }


                if (!(context.User.Identity?.IsAuthenticated ?? false))
                {
                    await WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized: Please login.");
                    return;
                }

                var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (actionDescriptor == null)
                {
                    await _next(context);
                    return;
                }

                var controllerName = actionDescriptor.ControllerName;
                var actionName = actionDescriptor.ActionName;

                var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    await WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized: Invalid or missing user ID.");
                    return;
                }

                //var cacheKey = $"UserMenu_{userId}";
                //if (!_cache.TryGetValue(cacheKey, out Dictionary<string, UserMenu>? userMenus))
                //{
                //    var menus = await unitOfWork.UserMenuRepository.GetMenusByUserIdAsync(userId);
                //    userMenus = menus
                //        .Where(m => m.Menu != null && !string.IsNullOrEmpty(m.Menu.Controller))
                //        .GroupBy(m => m.Menu.Controller.ToLower())
                //        .ToDictionary(g => g.Key, g => g.First());

                //    _cache.Set(cacheKey, userMenus, TimeSpan.FromMinutes(30));
                //    // TODO: Implement cache invalidation or update logic when user permissions change.
                //    //       For example, after updating UserMenu in the repository, remove or refresh this cache key:
                //    //           _cache.Remove(cacheKey);

                //}


                var cacheKey = $"UserMenu_{userId}";
                if (!_cache.TryGetValue(cacheKey, out List<UserMenuVM>? menuList))
                {
                    menuList = await _commonRepository.GetAssignedMenuListAsync(userId);
                    _cache.Set(cacheKey, menuList, TimeSpan.FromMinutes(30)); // store list
                }
                _cache.Remove(cacheKey);
                var userMenus = menuList
                       .Where(m => !string.IsNullOrEmpty(m.Controller))
                       .GroupBy(m => m.Controller.ToLower())
                       .ToDictionary(g => g.Key, g => g.First());

                var controllerKey = controllerName.ToLower();

                if (!userMenus.TryGetValue(controllerKey, out var userMenu))
                {
                    await WriteErrorResponseAsync(context, StatusCodes.Status403Forbidden,
                        "You do not have permission to perform this action.",
                        $"No permission found for controller '{controllerName}'.", userId, controllerName, actionName);
                    return;
                }

                if (!HasAccess(userMenu, context.Request.Method, actionName))
                {
                    await WriteErrorResponseAsync(context, StatusCodes.Status403Forbidden,
                        "You do not have permission to perform this action.",
                        $"User lacks permission for '{context.Request.Method}' on '{controllerName}/{actionName}'.",
                        userId, controllerName, actionName);
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during menu authorization middleware processing.");
                await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please contact the administrator.", ex.Message);
            }
        }

        private static bool HasAccess(UserMenuVM userMenu, string method, string actionName)
        {
            if (ActionPermissionMap.TryGetValue(actionName, out var config))
            {
                return method.Equals(config.Method, StringComparison.OrdinalIgnoreCase)
                    && config.PermissionCheck(userMenu);
            }

            return method.ToUpper() switch
            {
                "GET" => userMenu.List,
                "POST" => userMenu.Insert || userMenu.Post,
                "PUT" => userMenu.Post,
                "DELETE" => userMenu.Delete,
                _ => false
            };
        }

        private async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message,
            string? exMessage = null, Guid? userId = null, string? controller = null, string? action = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            _logger.LogWarning(
                "Blocked request | Status: {StatusCode}, User: {UserId}, Controller: {Controller}, Action: {Action}, ExMessage: {ExMessage}",
                statusCode, userId, controller, action, exMessage ?? message);

            var response = ResultVM.Fail(message, exMessage ?? string.Empty);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}