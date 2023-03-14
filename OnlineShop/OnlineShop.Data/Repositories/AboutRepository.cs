using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using OnlineShop.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Data.Repositories
{
    public interface IAboutRepository : IRepository<About>
    {

    }

    public class AboutRepository : RepositoryBase<About>, IAboutRepository
    {
        private readonly ILogger<AboutRepository> _logger;
        private readonly IConfiguration _configuration;

        public AboutRepository(IDbFactory dbFactory, IConfiguration configuration, ILogger<AboutRepository> logger) : base(dbFactory)
        {
            _configuration = configuration;
            _logger = logger;
        }

    }
}
