using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class DocumentTransferHostedService : IHostedService, IDisposable
    {
        private readonly IOptions<DocumentTransferConfig> _documentTransferConfig;
        private Timer _timer = null!;
        private readonly string _directoryPath;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DocumentTransferHostedService> _logger;

        public DocumentTransferHostedService(IOptions<DocumentTransferConfig> documentTransferConfig, IServiceProvider serviceProvider, ILogger<DocumentTransferHostedService> logger)
        {
            _documentTransferConfig = documentTransferConfig;
            _directoryPath = _documentTransferConfig.Value.DocumentTransferDataPath;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var date = DateTimeOffset.Now;
            using (_logger.BeginScope("{DocTransfer}", date.ToString("dd.MM.yyyy")))
            {
                _logger.LogInformation("Timed Hosted Service running.");
                var interval = TimeSpan.FromMinutes(_documentTransferConfig.Value.Interval);
                _timer = new Timer(ExecuteAsync, null, TimeSpan.Zero, interval);
                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void ExecuteAsync(object? state)
        {
            try
            {
                using var serviceScope = _serviceProvider.CreateScope();
                var services = serviceScope.ServiceProvider;
                var service = services.GetRequiredService<IDocumentTransferService>();

                await service.ExecuteDocTransfer();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Can not make document transfer");
            }
        }
    }
}
