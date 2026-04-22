using AutoMapper;
using TaskTracker.Core.Entities;
using TaskTracker.Shared.Contracts;

namespace Scholify.Application.Features.Teachers.TeacherOperation.ViewModels
{

    public class TaskItemModel : IMapFrom<TaskItem>
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int Priority { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskItem, TaskItemModel>().ReverseMap();
        }
    }
}
