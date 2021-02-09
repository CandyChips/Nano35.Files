using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nano35.Contracts;
using Nano35.Files.Api.Services;

namespace Nano35.Files.Api.Configurators
{
    public class EntityFrameworkConfiguration : 
        IConfigurationOfService
    {
        private readonly string _dbServer;
        private readonly string _catalog;
        private readonly string _login;
        private readonly string _password;
        
        public EntityFrameworkConfiguration(
            string dbServer, 
            string catalog, 
            string login,
            string password)
        {
            _dbServer = dbServer;
            _catalog = catalog;
            _login = login;
            _password = password;
        }
        public void AddToServices(
            IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => 
                options.UseSqlServer($"server={_dbServer}; Initial Catalog={_catalog}; User id={_login}; Password={_password};"));
        }
    }
}