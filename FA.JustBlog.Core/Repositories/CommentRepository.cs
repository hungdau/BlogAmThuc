using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;

namespace FA.JustBlog.Core.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(JustBlogContext context) : base(context)
        {
        }

        public void AddComment(int postId, string commentName, string commentEmail, string commentTitle, string commentBody)
        {
            this.dbSet.Add(new Comment { PostId = postId, Name = commentName, Email = commentEmail, CommentHeader = commentTitle, CommentText = commentBody });
        }

        public IList<Comment> GetCommentsForPost(int postId)
        {
            return this.dbSet.Where(x => x.PostId == postId).ToList();
        }

        public IList<Comment> GetCommentsForPost(Post post)
        {
            return this.dbSet.Where(x => x.PostId == post.Id).ToList();
        }
    }
}