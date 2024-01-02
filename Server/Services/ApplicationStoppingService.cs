using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ApplicationStoppingService
    {
        public ApplicationStoppingService(IHostApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(async () => {

                Console.WriteLine("Registering...");
                await Task.Delay(2000);
            });
        }
    }
}
