// BackgroundTaskService.cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using ParkingManagementSystem.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class BackgroundTaskService : BackgroundService
{
    private readonly ILogger<BackgroundTaskService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundTaskService(ILogger<BackgroundTaskService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new Timer(async state => await DoWorkAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        // Wait until the cancellation token is signaled
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task DoWorkAsync()
    {
        _logger.LogInformation("Background task is running.");

        using (var scope = _serviceProvider.CreateScope())
        {
            var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
            var parkingLotService = scope.ServiceProvider.GetRequiredService<IParkingLotService>();
            var reservations = await reservationService.GetAllReservations();

            foreach (var reservation in reservations)
            {
                if (reservation.StartTime > DateTime.Now && reservation.Status == "Cancelled")
                {
                    await reservationService.DeleteReservation(reservation.ReservationID);
                    continue;
                }
                if (reservation.EndTime < DateTime.Now || reservation.Status == "Cancelled")
                {
                    await reservationService.DeleteReservation(reservation.ReservationID);
                    await parkingLotService.UpdateLotAvailability(reservation.LotID, true);
                }

                if (reservation.StartTime <= DateTime.Now && reservation.EndTime >= DateTime.Now && reservation.Status == "Pending")
                {
                    reservation.Status = "Active";
                    await reservationService.EditReservation(reservation);
                    await parkingLotService.UpdateLotAvailability(reservation.LotID, false);
                }
            }
        }

    }
}
