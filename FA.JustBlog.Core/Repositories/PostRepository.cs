using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.JustBlog.Core.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(JustBlogContext context) : base(context)
        {
        }

        public int CountPostsForCategory(string category)
        {
            return base.dbSet.Where(p => p.Category.Name == category).Count();
        }

        public int CountPostsForTag(string tag)
        {
            int tagId;
            tagId = context.Tags.Where(x => x.Name == tag).Select(y => y.Id).FirstOrDefault();
            return context.Posts.Join(context.PostTagMap, x => x.Id, y => y.PostId, (x, y) => new { TagId = y.TagId }).Where(x => x.TagId == tagId).Count();
        }

        public Post FindPost(int year, int month, string urlSlug)
        {
            return base.dbSet.Where(p => p.PostedOn.Value.Year == year && p.PostedOn.Value.Month == month && p.UrlSlug == urlSlug).FirstOrDefault();
        }

        public IList<Post> GetHighestPosts(int size)
        {
            return base.dbSet.Include(x => x.Category)
                .OrderByDescending(p => (p.ViewCount.Value==null||p.RateCount.Value==null|| p.ViewCount.Value == 0 || p.RateCount.Value == 0)?0: Convert.ToDecimal(1.0 * p.TotalRate.Value / p.RateCount.Value))
                .Take(size).ToList();
        }

        public IList<Post> GetLatestPost(int size)
        {
            return base.dbSet.Include(x => x.Category).OrderByDescending(p => p.PostedOn).Take(size).ToList();
        }

        public IList<Post> GetMostViewedPost(int size)
        {
            return base.dbSet.Include(x => x.Category).OrderByDescending(p => p.ViewCount).Take(size).ToList();
        }

        public IList<Post> GetPostsByCategory(string category)
        {
            return base.dbSet.Include(x=>x.Category).Where(p => p.Category.Name == category).ToList();
        }

        public IList<Post> GetPostsByMonth(DateTime monthYear)
        {
            return base.dbSet.Where(p => p.PostedOn.Value.Year == monthYear.Year && p.PostedOn.Value.Month == monthYear.Month).ToList();
        }

        public IList<Post> GetPostsByTag(string tag)
        {
            int tagId;
            tagId = context.Tags.Where(x => x.Name == tag).Select(y => y.Id).FirstOrDefault();
            var list = context.Posts.Join(context.PostTagMap, x => x.Id, y => y.PostId, (x, y) => new { TagId = y.TagId, PostId = x.Id }).Where(x => x.TagId == tagId).ToList();
            List<Post> posts = new List<Post>();
            foreach (var post in list)
            {
                posts.Add(context.Posts.FirstOrDefault(x => x.Id == post.PostId));
            }
            return posts;
        }

        public IList<Post> GetPublisedPosts()
        {
            return base.dbSet.Include(x => x.Category).Where(p => p.Published == true).ToList();
        }

        public IList<Post> GetUnpublisedPosts()
        {
            return base.dbSet.Include(x => x.Category).Where(p => p.Published == false).ToList();
        }
        // public IList<Post> GetPostsByTitle(string titlePost)
        //{
        //    return base.dbSet.Include(x=>x.Category).Contains(titlePost,Pos).ToList();
        //}
    }
}