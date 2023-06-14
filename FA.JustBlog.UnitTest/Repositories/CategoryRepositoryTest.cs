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
    public class CategoryRepositoryTest
    {
        private ICategoryRepository _repository;
        private Mock<JustBlogContext> _context;
        private Mock<DbSet<Category>> _dbSet;

        [SetUp]
        public void SetUp()
        {
            _dbSet = new Mock<DbSet<Category>>();
            var data = new List<Category>()
            {
                new Category() { Id = 1, Name = "Apple",UrlSlug="Apple"},
                new Category() { Id = 2, Name = "Samsung", UrlSlug="Samsung"},
                new Category() { Id = 3, Name = "Oppo", UrlSlug="Oppo"},
            }.AsQueryable();
            _dbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _context = new Mock<JustBlogContext>();
            _context.Setup(x => x.Set<Category>()).Returns(_dbSet.Object);
            _repository = new CategoryRepository(_context.Object);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnListCategory()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void Find_WhenCalled_ReturnCategoryById()
        {
            var result = _repository.Find(x => x.Name == "Apple");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
        //[Test]
        //public void CreateBlog_saves_a_blog_via_context()
        //{
        //    var mockSet = new Mock<DbSet<Category>>();

        //    var mockContext = new Mock<JustBlogContext>();
        //    mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

        //    var service = new CategoryRepository(mockContext.Object);
        //    service.Add(new Category() { Id = 1, Name = "Apple", UrlSlug = "Apple" });

        //    mockSet.Verify(m => m.Add(It.IsAny<Category>()), Times.Once());
        //    mockContext.Verify(m => m.SaveChanges(), Times.Once());
        //}
    }
}