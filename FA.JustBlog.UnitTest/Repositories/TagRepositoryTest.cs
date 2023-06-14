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
    public class TagRepositoryTest
    {
        private ITagRepository _repository;
        private Mock<JustBlogContext> _context;
        private Mock<DbSet<Tag>> _dbSet;

        [SetUp]
        public void SetUp()
        {
            _dbSet = new Mock<DbSet<Tag>>();
            var data = new List<Tag>()
            {
                new Tag() { Id = 1, Name = "Apple",UrlSlug="Apple"},
                new Tag() { Id = 2, Name = "Samsung", UrlSlug="Samsung"},
                new Tag() { Id = 3, Name = "Apple1", UrlSlug="Apple1"},
            }.AsQueryable();
            _dbSet.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _context = new Mock<JustBlogContext>();
            _context.Setup(x => x.Set<Tag>()).Returns(_dbSet.Object);
            _context.Setup(x => x.Tags).Returns(_dbSet.Object);
            _repository = new TagRepository(_context.Object);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnListTag()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void GetTagByUrlSlug_WhenCalled_ReturnTagByUrlSlug()
        {
            var result = _repository.GetTagByUrlSlug("Apple");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}