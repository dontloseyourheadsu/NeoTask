using NeoTask.Domain.Tasks;

namespace NeoTask.WebApp.Models.Calendar;

public class TaskTimelineItem : TimelineItem
{
    public required NeoTaskCore Task { get; set; }
    public double HeightPx { get; set; }
}
