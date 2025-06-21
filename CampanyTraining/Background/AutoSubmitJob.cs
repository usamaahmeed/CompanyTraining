using CompanyTraining.Services;

namespace CompanyTraining.Background
{
    public class AutoSubmitJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AutoSubmitJob(IServiceProvider serviceProvider) 
        {
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<AutoSubmitExpiredAttemptsService>();
                await service.RunTask();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // كل 5 دقايق
            }
        }
    }
}
