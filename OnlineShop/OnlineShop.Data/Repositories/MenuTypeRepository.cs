using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;

namespace OnlineShop.Data.Repositories
{
    public interface IMenuTypeRepository : IRepository<MenuType>
    {
    }

    public class MenuTypeRepository : RepositoryBase<MenuType>, IMenuTypeRepository
    {
        public MenuTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
