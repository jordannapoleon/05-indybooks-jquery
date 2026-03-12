#nullable enable
using IndyBooks.Models;
namespace IndyBooks.Services;
public interface IWriterService
{
    //TODO: Add the interface for GetAllBooksByWriter method which
    //     takes one parameter of type long and returns a List of Book objects
    public Task<List<Book>> GetAllBooksByWriter(long id);
    
    public List<Writer> GetWriterList();
    public Writer? GetWriterById(long id);

    public Writer? DeleteWriterById(long id);
    
    public long PostWriter(Writer writer);
    public Writer? PutWriter(Writer writer, long id);
}