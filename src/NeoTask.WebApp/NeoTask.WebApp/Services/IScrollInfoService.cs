using System;

namespace NeoTask.WebApp.Services
{
    public interface IScrollInfoService
    {
        event EventHandler<int>? OnScroll;
        int YValue { get; }
        Task InitializeAsync();
    }
}
