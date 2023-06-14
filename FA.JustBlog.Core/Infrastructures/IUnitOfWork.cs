using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;

namespace FA.JustBlog.Core.Infrastructures
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IPostRepository PostRepository { get; }
        ITagRepository TagRepository { get; }
        ICommentRepository CommentRepository { get; }

        JustBlogContext JustBlogContext { get; }

        int SaveChange();

        Task<int> SaveChangeAsync();
    }
}