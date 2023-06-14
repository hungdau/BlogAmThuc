using FA.JustBlog.Common;
using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;

namespace FA.JustBlog.Core.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(JustBlogContext context) : base(context)
        {
        }

        public Tag GetTagByUrlSlug(string urlSlug)
        {
            return base.dbSet.FirstOrDefault(p => p.UrlSlug == urlSlug);
        }
        public IList<Tag> GetTagsByPost(int postId)
        {
            var list = context.Tags.Join(context.PostTagMap, x => x.Id, y => y.TagId, (x, y) => new { TagId = x.Id, PostId =y.PostId, Count=x.Count }).Where(x => x.PostId == postId).OrderBy(x=>x.Count).Take(10).ToList();
            List<Tag> tags = new List<Tag>();
            foreach (var tag in list)
            {
                tags.Add(context.Tags.FirstOrDefault(x => x.Id == tag.TagId));
            }
            return tags;
        }
        public IEnumerable<int> AddTagByString(string tags)
        {
            var tagNames = tags.Split(';');

            foreach (var tagName in tagNames)
            {
                var tagExisting = this.dbSet.Where(t => t.Name.Trim().ToLower() == tagName.Trim().ToLower()).Count();
                if (tagExisting == 0)
                {
                    var tag = new Tag()
                    {
                        Name = tagName,
                        UrlSlug = SeoUrlHepler.FrientlyUrl(tagName)
                    };
                    this.dbSet.Add(tag);
                    
                }
            }
       
            this.context.SaveChanges(); 

            foreach (var tagName in tagNames)
            {
                var tagExisting = this.dbSet.FirstOrDefault(t => t.Name.Trim().ToLower() == tagName.Trim().ToLower());
                if (tagExisting != null)
                {
                    yield return tagExisting.Id;
                }
            }
        }

    }
}