using NeoTask.Domain.Tasks.Attributes;

namespace NeoTask.Domain.Tasks;

/// <summary>
/// Represents a core task in the NeoTask system.
/// </summary>
public class Task
{
    /// <summary>
    /// Gets or sets the unique identifier for the task.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public required string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    public required NeoTaskStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the priority of the task.
    /// </summary>
    public required NeoTaskPriority Priority { get; set; }
}
