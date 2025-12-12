window.setTheme = (theme) => {
    document.body.className = theme;
    document.cookie = `theme=${theme}; path=/; max-age=31536000`;
}
