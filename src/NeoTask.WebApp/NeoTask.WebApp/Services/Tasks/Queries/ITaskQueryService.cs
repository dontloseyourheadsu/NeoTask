using NeoTask.Domain.Tasks;
using NeoTask.WebApp.Models.Query;

namespace NeoTask.WebApp.Services.Tasks.Queries;

public interface ITaskQueryService
{
    IEnumerable<NeoTaskCore> GetTasks();
    NeoTaskCore? GetTask(Guid id);
    IEnumerable<NeoTaskCore> QueryTasks(QueryRequest request);
}
