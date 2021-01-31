using AIHolding_task.Data;
using AIHolding_task.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AIHolding_task.Services
{
    public class SubscriptionRenewal : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
/*        private DBContext _context;*/

        private string Schedule => "*/10 * * * * *"; //Runs every 10 seconds

        public IConfiguration Configuration { get; }

        public SubscriptionRenewal(IConfiguration configuration)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
/*            _context = new DBContext(configuration.GetConnectionString("AIHoldingDB"));*/
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private void Process()
        {
/*            _context.Subscriptions.Add(new Subscription { StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), RenewalType = 1, UserID = 1 });
            _context.SaveChanges();*/
            Console.WriteLine("hello world" + DateTime.Now.ToString("F"));
        }
    }
}
