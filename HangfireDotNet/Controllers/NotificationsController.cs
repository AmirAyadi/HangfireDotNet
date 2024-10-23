using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        [Route("login")]
        public String Login()
        {
            // Fire and Forget Job - Job that will be executed only once
            var JobId = BackgroundJob.Enqueue(() => Console.Write("Welcome admin"));

            return $"Job ID : {JobId}. Welcome Email has been sent to admin";
        }

        [HttpGet]
        [Route("getNotification")]
        public String GetNotifications()
        {
            // Delayed Job - this job will be executed one but not immediately after some time
            var JobId = BackgroundJob.Schedule(() => Console.WriteLine("you received a new notification !"), TimeSpan.FromSeconds(20));

            return $"Job ID: {JobId}. You received a new notification admin ";
        }

        [HttpGet]
        [Route("GetPayment")]
        public String Payment()
        {
            // Fire and Forget Job - This Job is executed only once
            var parentJobId = BackgroundJob.Enqueue(() => Console.WriteLine("Your payment is received successfully"));
            // Continuation Job - This job is executed when its parent job is executed.
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("Your receipt is being printed"));
            return "Payment done and receipt printed";
        }

        [HttpGet]
        [Route("dailyOffers")]
        public String Offers()
        {
            // Recurring Job - this Job is executed many times on the specified cron schedule
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Sent similar products offers and suggestions"),Cron.Daily);
            return "Offers sent";
        }


    }
}

