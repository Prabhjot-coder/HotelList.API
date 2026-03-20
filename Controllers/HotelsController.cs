using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    // api/v1/Hotels
    [Route("api/v{version:apiVersion}/[controller]")] // Attribute routing to define the route for this controller
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _repository;

        public HotelsController(IHotelRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            var hotels = await _repository.GetAllHotels();
            return Ok(hotels);
        }

        // GET api/Hotels/5

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Hotel>> Get(int id)
        {
            var hotel = await _repository.GetHotelById(id);
            if (hotel == null)
            {
                return NotFound(); // Return 404 if the hotel with the specified ID is not found
            }
            return Ok(hotel);

        }
        //Post is to insert     data into the database,
        //Put is to update data in the database,
        //Delete is to delete data from the database (Hard / Soft - Isdelete = 0)
        // POST api/Hotels
        [HttpPost]
        public async Task<ActionResult<Hotel>> Post([FromBody] Hotel newHotel)
        {
            if (ModelState.IsValid)
            {
                var createdHotel = await _repository.AddNewHotel(newHotel);
                return CreatedAtAction(nameof(Get), new { id = createdHotel.Id }, createdHotel);
            }
            return BadRequest(ModelState);
        }
        //Route Constraints - To enforce specific rules on route parameters,
        //such as data types or value ranges
        // PUT api/Hotels/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Hotel>> Put(int id, [FromBody] Hotel updatedHotel)
        {
            return await _repository.UpdateHotelInfo(id, updatedHotel);
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            return await _repository.DeleteHotelById(id);

        }

        //Api Versioning - To manage different versions of the API and ensure backward compatibility

        [HttpGet]
        [Route("~/api/v{version:apiVersion}/legacy/hotels")] // Overriding the route for this specific
                                                             // action to maintain backward compatibility
        public ActionResult GetLegacyProducts()
        {
            return Ok();
        }
        //api/v1/Hotels/search/HotelName/5
        //[HttpGet("search/{name:regex(^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$)}")]
        
        //api/v1/Hotels/Search?name=Taj&id=5&department=HR&isRoom=true
        [HttpGet("Search")]
        public ActionResult SearchProduct(  [FromQuery] string name,
                                            [FromQuery]  int id,
                                            [FromQuery] string department,
                                            [FromQuery] bool isRoom)
        {
            return Ok();
        }
        //api/v1/Hotels/5/rooms
        [HttpGet("{id}/rooms")]
        public ActionResult GetHotelRooms(int id)
        {
            return Ok();
        }
        // multiple Constraints 
        [HttpGet("FetchHotelsWithRange/{id:int:min(1):max(1000)}")]
        public ActionResult GetHotelWithRange(int id)
        {  return Ok(); }

        [HttpGet("hotels/{hotelId}")]
        public ActionResult GetHotetRen([FromRoute] int hotelId)
        {
            return Ok();
        }

    }
}
