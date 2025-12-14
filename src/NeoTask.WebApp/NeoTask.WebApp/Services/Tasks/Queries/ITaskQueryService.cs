using NeoTask.Domain.Tasks;

namespace NeoTask.WebApp.Services.Tasks.Queries;

public interface ITaskQueryService
{
    IEnumerable<NeoTaskCore> GetTasks();
    NeoTaskCore? GetTask(Guid id);
}
