using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using My_Dashboard.Hubs;
using My_Dashboard.Models;
using My_Dashboard.Models.DB;
using System.Diagnostics;

namespace My_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<SignalHub> _chartHub; // SignalR hub context

        public HomeController(ILogger<HomeController> logger, IHubContext<SignalHub> chartHub)
        {
            _logger = logger;
            _chartHub = chartHub; // Initialize SignalR hub context
        }

        public IActionResult Index()
        {
            MachineUtilizationContext dbContext = new MachineUtilizationContext();
            var machines = dbContext.Machines.Select(x => x).Where(x => x.IsActive).ToList();
            dbContext.Dispose();

            return View(machines);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task sendData()
        {
            while (true)
            {
               // Generate some sample data to send to the SignalR hub
                    string newLabel = "Label_" + DateTime.Now.ToString("HH:mm:ss");
                    int newDataPoint = new Random().Next(0, 100);

                    // Send the data to all clients connected to the SignalR hub
                    await _chartHub.Clients.All.SendAsync("onReceiveCPU", newLabel, newDataPoint);
                    await Task.Delay(1000); // This doesn't block the thread
            }
     
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
