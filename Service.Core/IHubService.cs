using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IHubService
    {
        event EventHandler OnAlertReceived;
        event EventHandler OnCelebrateReceived;
        Task Connect();
        Task SendAlert(string user);
        Task SendCelebration(string user);

    }
}
