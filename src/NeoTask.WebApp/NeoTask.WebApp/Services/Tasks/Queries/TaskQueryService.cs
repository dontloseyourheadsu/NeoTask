using NeoTask.Domain.Tasks;
using NeoTask.Domain.Tasks.Attributes;
using NeoTask.WebApp.Models.Query;

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

        // Add a few dateless tasks for toggling
        for (int i = 21; i <= 25; i++)
        {
            _tasks.Add(new NeoTaskCore
            {
                Id = Guid.NewGuid(),
                Title = $"Task {i}",
                Description = $"This is a dateless task {i}.",
                Status = NeoTaskStatus.ToDo,
                Priority = NeoTaskPriority.Low,
                GoalDate = null,
                EstimatedDuration = TimeSpan.FromMinutes(45)
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

    public IEnumerable<NeoTaskCore> QueryTasks(QueryRequest request)
    {
        IEnumerable<NeoTaskCore> query = _tasks;

        // Status filter
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<NeoTaskStatus>(request.Status, true, out var status))
        {
            query = query.Where(t => t.Status == status);
        }

        // Priority filter
        if (!string.IsNullOrWhiteSpace(request.Priority) && Enum.TryParse<NeoTaskPriority>(request.Priority, true, out var priority))
        {
            query = query.Where(t => t.Priority == priority);
        }

        // Presence of date filter inferred from date range; when range provided, only dated tasks are considered
        if (request.GoalDateFrom.HasValue && request.GoalDateTo.HasValue)
        {
            query = query.Where(t => t.GoalDate.HasValue);
        }

        // Date range filter
        if (request.GoalDateFrom.HasValue && request.GoalDateTo.HasValue)
        {
            var from = request.GoalDateFrom.Value;
            var to = request.GoalDateTo.Value;
            query = query.Where(t => t.GoalDate.HasValue && t.GoalDate.Value >= from && t.GoalDate.Value <= to);
        }

        // Sort by GoalDate then Title
        query = query.OrderBy(t => t.GoalDate.HasValue ? 0 : 1)
                     .ThenBy(t => t.GoalDate)
                     .ThenBy(t => t.Title);

        // Pagination
        var skip = Math.Max(0, (request.PageNumber - 1) * request.PageSize);
        query = query.Skip(skip).Take(request.PageSize);

        return query.ToList();
    }
}
