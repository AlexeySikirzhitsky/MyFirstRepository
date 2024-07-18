using Library.Core.Services;
using Library.Controllers;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace LibraryTests.Controllers
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookService> _bookService;
        private Mock<ILogger<BooksController>> _logger;
        private BooksController _controller;

        [SetUp]
        public void Setup()
        {
            _bookService = new Mock<IBookService>();
            _logger = new Mock<ILogger<BooksController>>();
            _controller = new BooksController(_bookService.Object, _logger.Object);
        }

        [Test]
        public async Task GetBooks_ShouldReturnAllBooks()
        {
            _bookService.Setup(x => x.GetBooksAsync())
                .ReturnsAsync(new List<Book> { new Book { Id = 1, Title = "Test", Description = "Desc" } });

            var result = await _controller.GetBooks();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var books = okResult.Value as IEnumerable<Book>;
            Assert.That(books.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetBook_ShouldReturnBook()
        {
            _bookService.Setup(x => x.GetBookAsync(1))
                .ReturnsAsync(new Book { Id = 1, Title = "Test", Description = "Desc" });

            var result = await _controller.GetBook(1);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var book = okResult.Value as Book;
            Assert.That(book.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task PostBook_ShouldCreateBook()
        {
            var newBook = new Book { Title = "New Book", Description = "Desc" };
            _bookService.Setup(x => x.CreateBookAsync(newBook))
                .ReturnsAsync(newBook);

            var result = await _controller.PostBook(newBook);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            var book = createdAtActionResult.Value as Book;
            Assert.That(book.Title, Is.EqualTo("New Book"));
        }

        [Test]
        public async Task PutBook_ShouldUpdateBook()
        {
            var updatedBook = new Book { Id = 1, Title = "Updated Title", Description = "Desc" };
            _bookService.Setup(x => x.UpdateBookAsync(1, updatedBook))
                .ReturnsAsync(true);

            var result = await _controller.PutBook(1, updatedBook);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteBook_ShouldRemoveBook()
        {
            _bookService.Setup(x => x.DeleteBookAsync(1))
                .ReturnsAsync(true);

            var result = await _controller.DeleteBook(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
