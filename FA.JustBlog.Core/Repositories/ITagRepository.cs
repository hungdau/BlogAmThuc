using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;

namespace FA.JustBlog.Core.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        //Tag Find(int TagId);

        //void AddTag(Tag Tag);

        //void UpdateTag(Tag Tag);

        //void DeleteTag(Tag Tag);

        //void DeleteTag(int TagId);

        //IList<Tag> GetAllTags();

        Tag GetTagByUrlSlug(string urlSlug);
        IList<Tag> GetTagsByPost(int postId);
        public IEnumerable<int> AddTagByString(string tags);
    }
}