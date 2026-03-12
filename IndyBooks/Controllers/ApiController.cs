using Microsoft.AspNetCore.Mvc;
using IndyBooks.Models;
using IndyBooks.Services;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop.Implementation;

namespace IndyBooks.Controllers
{
    [Route("api/")] //The base route for all calls to this controller
    public class ApiController : Controller
    {
        private IWriterService _writerService;
        public ApiController(IWriterService writerService) { _writerService = writerService; }
        /**
         * BOOK COUNT: returns a new object holding the count of all the books by a single Author as
                    Be sure it passes the ApiWriterTests  BOOK COUNT TEST
         */


        //TODO: Write the [HttpGet] annotation with the API route for this call
        [Route("writer/{id}/bookcount")]
        [HttpGet]
        public IActionResult GetAuthorBookCount(long id)
        {
            Writer writer = _writerService.GetWriterById(id);

            //DONE: return NotFound if their are no writers in the db with the id
            if(writer is null) { return NotFound(); };

            //TODO: return OK with the AJAX data as a new object, e.g.,{ Count = 3, Id = 5 } for the given writer         
            return Ok( new { Count = _writerService.GetAllBooksByWriter(id).Count, Id = id } );
        }
        /**
         * READ ALL: Retrieves a collection of writers
         * uses a GET verb with the URL pattern "api/writers"
         */
        [Route("writers/")]
        [HttpGet]
        public IActionResult GetWriters()
        {
            return Ok(_writerService.GetWriterList());
        }
         /**
         * READ ONE: Retrieves a particular writer with the given {id}
         * uses a GET verb with the URL pattern "api/writers/41"
         */
        [Route("writers/{id}")]//This annotation passes along the Route Id for the api
        [HttpGet] 
        public IActionResult GetWriter(long id)
        {
            if(_writerService.GetWriterById(id) == null) //replace the if statement with the code here
            {
                return NotFound(); // if not return NotFound() instead of Ok()
            }

        //Otherwise return Ok(writer);
            return Ok( _writerService.GetWriterById(id) );
        }

        /**
         * DELETE: Removes a particular writer with the given {id}
         * uses a DELETE verb with the URL pattern "api/writers/37"
         */
        [Route("writers/{id}")]
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            //Check if writer exists using the _writerService.GetWritersList and the Any() collections method
            if (!_writerService.GetWriterList().Any(w=>w.Id == id)) 
            { 
                return NotFound(); 
            }

            //Pass the _writerService DeleteWriterById method to Accepted() below
            return Accepted(_writerService.DeleteWriterById(id));
        }
        /**
         * CREATE: Add a new writer to the collection
         * uses a POST verb with the URL pattern "api/writers"
         */
        [Route("writers/")]
        [HttpPost]
        public IActionResult PostWriter([FromBody]Writer writer)
        {
            // Test for an invalid ModelState -> return BadRequest();
            if(!ModelState.IsValid) {return BadRequest();}

            //Pass the result from the _writerService PostWriter method to Accepted() below
            return Accepted( _writerService.PostWriter(writer) );

        }
       
        /** 
         * UPDATE: Modify a given writer with id and [FromBody]Writer writer parameters
         * uses a PUT verb with the URL pattern "api/writers/16"
         */
        [Route("writers/{id}")]
        [HttpPut] 
        public IActionResult PutWriter([FromBody]Writer writer, long id)
        {
        // Test for an invalid ModelState -> return BadRequest();
        if(!ModelState.IsValid) {return BadRequest();}

        //Test for missing record using Any() -> return NotFound();
        if (!_writerService.GetWriterList().Any(w=>w.Id == id)) { return NotFound(); }

        //Otherwise, pass the results of the _writerService PutWriter method to Accepted() below

            return Accepted( _writerService.PutWriter(writer, id) );
        }
    }
}

