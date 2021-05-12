using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace MyResumeSiteBackEnd.Services
{
    public class NotificationService
    {
        private readonly IJSRuntime _jSRuntime;

        public NotificationService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task Notify(string message, bool isSuccess = true)
        {
            if (_jSRuntime != null)
            {
                await _jSRuntime.InvokeVoidAsync("console.log", message);
            }
        }
    }
}
