using Exemplo;

namespace Template.Infra
{
    public static class GeradorDeServicos
    {
        // Serviço que será injetado
        public static IServiceScopeFactory ServiceScopeFactory;

        public static DataContext CarregarContexto()
        {
            // Verificar se o ServiceScopeFactory não é nulo
            if (ServiceScopeFactory == null)
            {
                throw new InvalidOperationException("ServiceScopeFactory não foi inicializado.");
            }

            // Usar ServiceScopeFactory para criar um escopo e obter o contexto
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DataContext>();
                if (context == null)
                {
                    throw new InvalidOperationException("Não foi possível resolver o DataContext.");
                }

                return context;
            }
        }
    }
}
