using IndyBooks.Models;
using IndyBooks.Services;
using IndyBooks.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IndyBooks.Tests;
public class ApiWriterTests
{
    private readonly Mock<IWriterService> _mockWriterService;
   private readonly ApiController _controller;

    public ApiWriterTests()
    {
        // Arrange (setup the mock and the controller with the mock object)
        _mockWriterService = new Mock<IWriterService>();
        _controller = new ApiController(_mockWriterService.Object);
    }

    /**
    * BOOK COUNT TEST: 
        should use the API data to returns a new object holding the count of books by a single Author
                as a new object with two properties Id (Writer Id) and Count (Book count), 
                e.g., if Writer with Id 5 has 3 books, return { Id = 5, Count = 3 }
        
        It also should return NotFound if their are no writers in the db with the id
        otherwise it should return Ok
    */
    [Fact]
    public void GetAuthorBookCount_ReturnsAnObjectWithIdAndCountOfWritersBooks()
    {
        // Arrange
        var mockBooks = new List<Book>
        {
            new Book { AuthorId = 5 },
            new Book { AuthorId = 5 }, 
            new Book { AuthorId = 5 }
        };


        // Tell the mock service what to return when ApiController calls methods
        //TODO: Once you are ready to test your work, uncomment the following line
        _mockWriterService.Setup(service => service.GetAllBooksByWriter(5)).Returns(mockBooks);
        
        _mockWriterService.Setup(service => service.GetWriterById(5)).Returns(new Writer{});
        _mockWriterService.Setup(service => service.GetWriterById(6)).Returns((Writer?)null);

        // Act
        var result =  _controller.GetAuthorBookCount(5);
        var noresults = _controller.GetAuthorBookCount(6);

        // Assert
        // Verify that the result is an OK (HTTP 200) status
        var okResult = Assert.IsType<OkObjectResult>(result);
        //Verify that the noresults is NotFound status
        var noResult = Assert.IsType<NotFoundResult>(noresults);
        // Verify that the returned model has the correct property values        
        Assert.Equal("{ Id = 5, Count = 3 }", okResult.Value!.ToString()!.Trim() );
    }

    [Fact]
    public void GetWriters_ReturnsOkResultWithListOfWriters()
    {
        // Arrange
        var mockWriters = new List<Writer>
        {
            new Writer { Id = 1, Name = "Maya Angelou" },
            new Writer { Id = 2, Name = "Rupert Sheldrake" }
        };

        // Tell the mock service what to return when ApiController is called
        _mockWriterService.Setup(service => service.GetWriterList())
                           .Returns(mockWriters);

        // Act
        var result =  _controller.GetWriters();

        // Assert
        // Verify that the result is an OK (HTTP 200) status
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Verify that the returned model is the list of products we mocked
        var returnedProducts = Assert.IsType<List<Writer>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count);
    }
 [Fact]
 public void GetWriterById_ReturnsOKWithWriter()
    {
        // Arrange
        var mockWriters = new List<Writer>
        {
            new Writer { Id = 1, Name = "Maya Angelou" },
            new Writer { Id = 2, Name = "Rupert Sheldrake" }
        };
        long id = 2;

        // Tell the mock service what to return when ApiController is called
        _mockWriterService.Setup(service => service.GetWriterById(id))
                           .Returns(mockWriters.Single(w=>w.Id == id));

        // Act
        var result =  _controller.GetWriter(id);

        // Assert
        // Verify that the result is an OK (HTTP 200) status
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        // Verify that the returned model is mocked writer we selected
        var returnedWriter = Assert.IsType<Writer>(okResult.Value);
        Assert.Equal(mockWriters[1].Name, returnedWriter.Name); 
    }
    [Fact]
    public void GetWriterById_ReturnsNotFoundwithoutWriter()
    {
        
        // Arrange
        long nonExistentId = 0;

        // Tell the mock service what to return when ApiController is called
        _mockWriterService.Setup(service => service.GetWriterById(nonExistentId))
                           .Returns((Writer?) null);

        // Act

        var result =  _controller.GetWriter(nonExistentId);

        // Assert
        // Verify that the result is an NotFound (HTTP 404) status
        Assert.IsType<NotFoundResult>(result);
        
    }
}