using Library.Core.DB;
using Library.Core.Models;
using Library.Core.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTests.Core.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private AppDbContext _context;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            var books = new List<Book>
            {
                new() { Id = 1, Title = "Test Book 1", Description = "Desc 1" },
                new() { Id = 2, Title = "Test Book 2", Description = "Desc 2" }
            };

            _context.Books.AddRange(books);
            _context.SaveChanges();

            _bookService = new BookService(_context);

        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetBooks_ShouldReturnAllBooks()
        {
            var books = await _bookService.GetBooksAsync();
            Assert.That(books.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetBook_ShouldReturnCorrectBook()
        {
            var book = await _bookService.GetBookAsync(1);
            Assert.That(book.Title, Is.EqualTo("Test Book 1"));
        }

        [Test]
        public async Task CreateBook_ShouldAddNewBook()
        {
            var newBook = new Book { Title = "Test Book 3", Description = "Desc 3" };
            await _bookService.CreateBookAsync(newBook);

            var books = await _bookService.GetBooksAsync();
            Assert.That(books.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task UpdateBook_ShouldModifyExistingBook()
        {
            var book = _context.Books.First();
            book.Title = "Updated Title";
            var result = await _bookService.UpdateBookAsync(book.Id, book);
            Assert.That(result, Is.True);
            Assert.That(_context.Books.First().Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public async Task DeleteBook_ShouldRemoveBook()
        {
            var result = await _bookService.DeleteBookAsync(1);
            Assert.That(result, Is.True);
            var books = await _bookService.GetBooksAsync();
            Assert.That(books.Count(), Is.EqualTo(1));
        }
    }
}
