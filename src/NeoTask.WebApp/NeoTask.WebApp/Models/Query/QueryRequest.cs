namespace NeoTask.WebApp.Models.Query;

/// <summary>
/// Represents a request to query tasks with specific filters and pagination.
/// </summary>
public readonly record struct QueryRequest
{
    public string? Status { get; }
    public string? Priority { get; }

    /// <summary>
    /// Start of date range (inclusive). Must be accompanied by <see cref="GoalDateTo"/> if set.
    /// </summary>
    public DateTime? GoalDateFrom { get; }

    /// <summary>
    /// End of date range (inclusive). Must be accompanied by <see cref="GoalDateFrom"/> if set.
    /// </summary>
    public DateTime? GoalDateTo { get; }

    public int PageNumber { get; }
    public int PageSize { get; }

    /// <summary>
    /// Calculated convenience flag. True when a date range is provided, otherwise false.
    /// </summary>
    public bool HasDate => GoalDateFrom.HasValue && GoalDateTo.HasValue;

    /// <summary>
    /// Construct a request with validation.
    /// - If either date bound is provided, the other must also be provided.
    /// - PageNumber and PageSize must be positive.
    /// </summary>
    public QueryRequest(
        string? status = null,
        string? priority = null,
        DateTime? goalDateFrom = null,
        DateTime? goalDateTo = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (goalDateFrom.HasValue ^ goalDateTo.HasValue)
        {
            throw new ArgumentException("Both GoalDateFrom and GoalDateTo must be set when filtering by date range.");
        }
        if (pageNumber <= 0) throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

        Status = status;
        Priority = priority;
        GoalDateFrom = goalDateFrom;
        GoalDateTo = goalDateTo;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}