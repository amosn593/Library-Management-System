using AspNetCoreHero.ToastNotification;

namespace USERVIEW.StartApp
{
    public static class NotificationDependencyInjection
    {
        public static IServiceCollection NotificationService(this IServiceCollection Services) 
        {
            Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 4;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

            return Services;
        }
    }
}
