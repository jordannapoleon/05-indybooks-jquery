#nullable enable
using IndyBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace IndyBooks.Services;
public class ApiWriterService : IWriterService
{
    private IndyBooksDataContext _db;
    public ApiWriterService(IndyBooksDataContext db) { _db = db; }

    
    public List<Book> GetAllBooksByWriter(long id) 
    {
        //TODO: Implement the method GetAllBooksByWriter for IWriterService
        List<Book> result = _db.Books.Where(b => b.AuthorId == id).ToList();

        return result;
    }
   
    public List<Writer> GetWriterList()
    {
        return _db.Writers.ToList();
    }
    public Writer? GetWriterById(long id)
    {
        return _db.Writers.SingleOrDefault(w=>w.Id == id); //Uses lamda function and extension methods here
    }
    public Writer DeleteWriterById(long id)
    {
        // Get the Writer at the given id from the db context
        //     Remove the Writer at that id, be sure to SaveChanges()
       //      and return the writer information
        
            var writer = _db.Writers.SingleOrDefault(w=>w.Id == id);
            _db.Writers.Remove(writer!);
             _db.SaveChanges();

        return(new Writer{Id = writer!.Id, Name = writer.Name});

    }
    public long PostWriter(Writer writer)
    {
        //  Add a new Writer to the db context, return the writer id
        _db.Writers.Add(writer);
        _db.SaveChanges();
        return writer.Id;
    }
    public Writer PutWriter(Writer writer, long id)
    {
        // Update the Writer at the given id, return the Writer
        var dbWriter = _db.Writers.SingleOrDefault(w=>w.Id == id);
        dbWriter!.Name = writer.Name;
        _db.Writers.Update(dbWriter);
        _db.SaveChanges();
        return (dbWriter);
    }
}
