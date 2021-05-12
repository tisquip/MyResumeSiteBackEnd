
using Microsoft.JSInterop;

namespace MyResumeSiteBackEnd.Services
{
    public class UtilityService
    {
        private readonly IJSRuntime _jSRuntime;
        public NotificationService NotificationService { get; }
        public SignalRService SignalRService { get;  }
        public UtilityService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            NotificationService = new NotificationService(jSRuntime);
            SignalRService = new SignalRService(NotificationService);
        }
    }
}
