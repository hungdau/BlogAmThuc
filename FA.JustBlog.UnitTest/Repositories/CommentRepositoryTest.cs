using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FA.JustBlog.UnitTest.Repositories
{
    [TestFixture]
    public class CommentRepositoryTest
    {
        private ICommentRepository _repository;
        private Mock<JustBlogContext> _context;
        private Mock<DbSet<Comment>> _dbSet;

        [SetUp]
        public void SetUp()
        {
            _dbSet = new Mock<DbSet<Comment>>();
            var data = new List<Comment>()
            {
                new Comment() { Id = 1, Name = "comment 1",PostId=1},
                new Comment() { Id = 2, Name = "Comment 2", PostId=2},
                new Comment() { Id = 3, Name = "Comment 3",PostId=1},
            }.AsQueryable();
            _dbSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _context = new Mock<JustBlogContext>();
            _context.Setup(x => x.Set<Comment>()).Returns(_dbSet.Object);
            _repository = new CommentRepository(_context.Object);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnListComment()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void GetCommentsForPost_WhenCalled_ReturnListCommentById()
        {
            var result = _repository.GetCommentsForPost(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

    }
}