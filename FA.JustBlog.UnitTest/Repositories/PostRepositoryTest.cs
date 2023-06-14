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
    public class PostRepositoryTest
    {
        private IPostRepository _repository;
        private Mock<JustBlogContext> _context;
        private Mock<DbSet<Post>> _dbSet;

        [SetUp]
        public void SetUp()
        {
            _dbSet = new Mock<DbSet<Post>>();
            var data = new List<Post>()
            {
                new Post() { Id = 1, Title = "Apple", CategoryId=1,Published=true,ViewCount=19},
                new Post() { Id = 2, Title = "Samsung", CategoryId=2,Published=true,ViewCount=10},
                new Post() { Id = 3, Title = "Apple1", CategoryId=1, Published=false,ViewCount=15},
                new Post() { Id = 4, Title = "Samsung1", CategoryId=2,Published=true,ViewCount=10},
                new Post() { Id = 5, Title = "Samsung2", CategoryId=2, Published = false,ViewCount=10},
                new Post() { Id = 6, Title = "Apple2", CategoryId=1, Published = true,ViewCount=20},
            }.AsQueryable();
            _dbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _context = new Mock<JustBlogContext>();
            _context.Setup(x => x.Set<Post>()).Returns(_dbSet.Object);
            //_context.Setup(x => x.Posts).Returns(_dbSet.Object);
            _repository = new PostRepository(_context.Object);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnListPost()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count());
        }

        [Test]
        public void GetPublisedPosts_WhenCalled_ReturnListPostPublic()
        {
            var result = _repository.GetPublisedPosts();
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [Test]
        public void GetUnPublisedPosts_WhenCalled_ReturnListPostUnPublic()
        {
            var result = _repository.GetUnpublisedPosts();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetMostViewedPost_WhenCalled_ReturnListMostViewedPost()
        {
            var result = _repository.GetMostViewedPost(3);
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.FirstOrDefault().Id);
        }
    }
}