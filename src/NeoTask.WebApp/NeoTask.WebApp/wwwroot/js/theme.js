window.setTheme = (theme) => {
    // Preserve font-accessible class if present
    const isAccessible = document.body.classList.contains('font-accessible');
    document.body.className = theme + (isAccessible ? ' font-accessible' : '');
    document.cookie = `theme=${theme}; path=/; max-age=31536000`;
}

window.toggleAccessibleFont = (isAccessible) => {
    if (isAccessible) {
        document.body.classList.add('font-accessible');
    } else {
        document.body.classList.remove('font-accessible');
    }
    document.cookie = `fontAccessible=${isAccessible}; path=/; max-age=31536000`;
}
