using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;
using Timer = System.Timers.Timer;

namespace OutboxPatternDemo.Publisher.CustomOutbox
{
    public class CustomOutboxProcessor : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private Timer _timer;

        public CustomOutboxProcessor(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(10000);
            _timer.Elapsed += PublishEventsInOutbox;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;

            return Task.CompletedTask;
        }

        private void PublishEventsInOutbox(object sender, System.Timers.ElapsedEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            using var outboxContext = scope.ServiceProvider.GetService<CustomOutboxContext>();
            var messageSession = scope.ServiceProvider.GetService<IMessageSession>();

            var unpublishedMessages = outboxContext.Messages.Where(m => !m.ProcessedTimeUtc.HasValue);

            foreach (var unpublishedMessage in unpublishedMessages)
            {
                var message = unpublishedMessage.Type switch
                {
                    "StateUpdated" => JsonConvert.DeserializeObject<StateUpdated>(unpublishedMessage.Data),
                    _ => throw new System.NotImplementedException(),
                };
                messageSession.Publish(message).GetAwaiter().GetResult();

                unpublishedMessage.ProcessedTimeUtc = DateTime.UtcNow;
            }
            outboxContext.SaveChanges();
        }
    }
}
