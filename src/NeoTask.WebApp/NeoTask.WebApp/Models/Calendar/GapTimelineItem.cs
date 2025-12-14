namespace NeoTask.WebApp.Models.Calendar;

public class GapTimelineItem : TimelineItem
{
    public TimeSpan Duration { get; set; }
    public double HeightPx { get; set; }
    public bool IsSafeTime { get; set; }
    public string Label { get; set; } = "";
}
