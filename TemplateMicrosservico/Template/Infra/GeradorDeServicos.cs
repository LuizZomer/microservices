using Microsoft.Extensions.DependencyInjection;

namespace Template.Infra
{
    public static class GeradorDeServicos
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        // Método para configurar o contêiner de DI
        public static void Configure(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
        }

        // Método para acessar um serviço a partir do contêiner
        public static T GetService<T>() => ServiceProvider.GetRequiredService<T>();
    }
}
