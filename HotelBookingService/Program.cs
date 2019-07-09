using HotelBookingService.API;
using HotelBookingService.BLL;
using HotelBookingService.BLL.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HotelBookingService
{
    class Program
    {
        static async Task Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var reservationServiceGetter = serviceProvider.GetService<IReservationStepsGetter>();

            ReservationApi api = new ReservationApi(reservationServiceGetter);

            var steps = await api.GetStepsForHotel(Guid.NewGuid());

            api.SetDataForReservation(steps);

            if(await steps.SendDataAsync())
            {
                if(!string.IsNullOrWhiteSpace(steps.FailedStepName))
                {
                    Console.WriteLine($"Step that has failed is {steps.FailedStepName}.");
                }
                Console.WriteLine("Reservation succesfull!");
            }
            else
            {
                Console.WriteLine($"Reservation failed on the step {steps.FailedStepName}.");
                Console.WriteLine("Reservation unsuccesfull!");
            }
            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = Configuration.GetConfiguration();

            services.AddTransient<FakeApiResponder>();

            services.AddHttpClient("HotelBookingApi", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("HotelBookingApiUrl"));
                client.DefaultRequestHeaders
                    .Add(configuration.GetValue<string>("HotelBookingApiKeyHeader"),
                        configuration.GetValue<string>("HotelBookingApiKey"));
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10)
            }))
            .AddHttpMessageHandler<FakeApiResponder>();

            services.AddTransient<IApiConsumer, ApiConsumer>();
            services.AddTransient<IMailSender, MailSender>();

            IServiceProvider provider = services.BuildServiceProvider();

            var apiConsumer = provider.GetRequiredService<IApiConsumer>();
            var mailSender = provider.GetRequiredService<IMailSender>();

            IReservationStepsGetter reservationStepsGetter = new ReservationStepsGetter(apiConsumer, mailSender);

            services.AddSingleton<IReservationStepsGetter>(reservationStepsGetter);
            services.AddTransient<IReservationStep, MakePayment>();
            services.AddTransient<IReservationStep, MakeReservation>();
            services.AddTransient<IReservationStep, SendConfirmationMail>();
            services.AddTransient<IReservationStep, VerifyRoomAvailability>();
            services.AddTransient<IReservationStep, VerifyRoomPrice>();
        }
    }
}
