window.onscroll = function() {
    if (window.scrollInfoService != null)
        window.scrollInfoService.invokeMethodAsync('OnScroll', window.pageYOffset);
}

// Also listen to wheel events to simulate scroll for fixed containers
window.addEventListener('wheel', function(e) {
    if (window.scrollInfoService != null) {
        // We pass the deltaY as the "yValue" for wheel events, 
        // or we could maintain a virtual scroll position.
        // The user's C# code expects an int yValue.
        // Let's pass deltaY so the C# side can decide direction.
        window.scrollInfoService.invokeMethodAsync('OnScroll', Math.sign(e.deltaY));
    }
});

window.RegisterScrollInfoService = (scrollInfoService) => {
    window.scrollInfoService = scrollInfoService;
}
