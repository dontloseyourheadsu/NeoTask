using System;
using Microsoft.JSInterop;

namespace NeoTask.WebApp.Services
{
    public class ScrollInfoService: IScrollInfoService
    {
        public event EventHandler<int> OnScroll; 

        public ScrollInfoService(IJSRuntime jsRuntime)
        {
            RegisterServiceViaJsRuntime(jsRuntime);
        }

        private void RegisterServiceViaJsRuntime(IJSRuntime jsRuntime)
        {
            // We don't await here because we can't in a constructor. 
            // In a real app, this might be better in an async initialization method.
            // But following the user's snippet:
            jsRuntime.InvokeVoidAsync("RegisterScrollInfoService", DotNetObjectReference.Create(this));
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
