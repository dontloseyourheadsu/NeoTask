using NeoTask.Domain.Tasks;
using NeoTask.Domain.Tasks.Attributes;

namespace NeoTask.WebApp.Services.Tasks.Queries;

public class TaskQueryService : ITaskQueryService
{
    private readonly List<NeoTaskCore> _tasks;

    public TaskQueryService()
    {
        _tasks = new List<NeoTaskCore>();
        var today = DateTime.Today;

        // Generate mock tasks
        for (int i = 1; i <= 20; i++)
        {
            // Distribute tasks across today and tomorrow
            var date = today.AddDays(i % 2);
            // Random time between 8 AM and 5 PM (17:00)
            // i=1 -> 9, i=2 -> 10, ...
            var hour = 8 + (i % 10);

            _tasks.Add(new NeoTaskCore
            {
                Id = Guid.NewGuid(),
                Title = $"Task {i}",
                Description = $"This is the detailed description for Task {i}. It contains important information about what needs to be done.",
                Status = NeoTaskStatus.ToDo,
                Priority = NeoTaskPriority.Medium,
                GoalDate = date.AddHours(hour),
                EstimatedDuration = TimeSpan.FromMinutes(30 + (i % 3) * 30) // 30, 60, or 90 mins
            });
        }

        // Sort by GoalDate for the calendar view
        _tasks.Sort((a, b) => Nullable.Compare(a.GoalDate, b.GoalDate));
    }

    public IEnumerable<NeoTaskCore> GetTasks()
    {
        return _tasks;
    }

    public NeoTaskCore? GetTask(Guid id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }
}
