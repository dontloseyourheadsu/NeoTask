namespace NeoTask.WebApp.Components.Customization;

public class ThemeState
{
    public string CurrentTheme { get; private set; } = "theme-calm";
    public event Action? OnChange;

    public void SetTheme(string theme)
    {
        CurrentTheme = theme;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
