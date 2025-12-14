using System;
using Microsoft.JSInterop;

namespace NeoTask.WebApp.Services
{
    public class ScrollInfoService : IScrollInfoService
    {
        public event EventHandler<int>? OnScroll;
        private readonly IJSRuntime _jsRuntime;

        public ScrollInfoService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            await _jsRuntime.InvokeVoidAsync("RegisterScrollInfoService", DotNetObjectReference.Create(this));
        }

        public int YValue { get; private set; }

        [JSInvokable("OnScroll")]
        public void JsOnScroll(int yValue)
        {
            YValue = yValue;
            // Console.WriteLine("ScrollInfoService.OnScroll " + yValue);
            OnScroll?.Invoke(this, yValue);
        }
    }
}
