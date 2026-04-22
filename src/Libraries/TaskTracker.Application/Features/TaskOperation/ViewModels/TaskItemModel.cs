using AutoMapper;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Core.Entities;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Application.Features.TaskOperation.ViewModels
{

    public class TaskItemModel : IMapFrom<TaskItem>
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int PriorityId { get; set; }

        public bool IsCompleted { get; set; } = false;
        public string? PriorityName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskItem, TaskItemModel>().ReverseMap();
        }
    }
}
