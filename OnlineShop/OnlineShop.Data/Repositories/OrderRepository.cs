using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;

namespace OnlineShop.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        //IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        //public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        //{
        //    var parameters = new SqlParameter[]{
        //        new SqlParameter("@fromDate",fromDate),
        //        new SqlParameter("@toDate",toDate)
        //    };
        //    return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("GetRevenueStatistic @fromDate,@toDate", parameters);
        //}
    }
}
