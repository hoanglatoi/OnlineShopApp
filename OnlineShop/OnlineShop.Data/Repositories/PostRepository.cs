using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;

namespace OnlineShop.Data.Repositories
{
    public interface IPostRepository : IRepository<PostContent>
    {
        IEnumerable<PostContent> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<PostContent>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<PostContent> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.PostContents
                        join pt in DbContext.PostTags
                        on p.ID equals pt.ContentID
                        where pt.TagID == tag && p.Status
                        orderby p.CreatedDate descending
                        select p;

            totalRow = query.Count();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return query;
        }
    }
}
