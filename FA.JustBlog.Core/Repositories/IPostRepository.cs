using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;

namespace FA.JustBlog.Core.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Post FindPost(int year, int month, string urlSlug);

        //Post FindPost(int postId);

        //void AddPost(Post post);

        //void UpdatePost(Post post);

        //void DeletePost(Post post);

        //void DeletePost(int postId);

        //IList<Post> GetAllPosts();

        IList<Post> GetPublisedPosts();

        IList<Post> GetUnpublisedPosts();

        IList<Post> GetLatestPost(int size);

        IList<Post> GetPostsByMonth(DateTime monthYear);

        int CountPostsForCategory(string category);

        IList<Post> GetPostsByCategory(string category);

        int CountPostsForTag(string tag);

        IList<Post> GetPostsByTag(string tag);

        IList<Post> GetMostViewedPost(int size);

        IList<Post> GetHighestPosts(int size);
    }
}