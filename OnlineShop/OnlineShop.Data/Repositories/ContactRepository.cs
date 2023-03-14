using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;

namespace OnlineShop.Data.Repositories
{
    public interface IContactRepository : IRepository<Contact>
    {
    }

    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
