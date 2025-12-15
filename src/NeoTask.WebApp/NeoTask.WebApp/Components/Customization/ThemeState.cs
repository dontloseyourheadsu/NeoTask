namespace NeoTask.WebApp.Components.Customization;

public class ThemeState
{
    public string CurrentTheme { get; private set; } = "theme-calm";
    public bool IsAccessibleFont { get; private set; } = false;
    public event Action? OnChange;

    public void SetTheme(string theme)
    {
        CurrentTheme = theme;
        NotifyStateChanged();
    }

    public void SetAccessibleFont(bool isAccessible)
    {
        IsAccessibleFont = isAccessible;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
