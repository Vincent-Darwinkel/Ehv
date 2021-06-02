using Account_Removal_Service.Logic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Account_Removal_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer { Interval = 30000 };
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            CreateHostBuilder(args).Build().Run();
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            QueuedTasks.ExecuteAction();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
