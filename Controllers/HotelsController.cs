using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")] // Attribute routing to define the route for this controller
    [ApiController] 
    public class HotelsController : ControllerBase
    {
        //Dependency Injection of HotelRepository
        // Constructor to inject the HotelRepository dependency into the controller

        //Method Injection is a design pattern that allows you to inject dependencies into a class rather
        //than creating them within the class itself.

        //Property Injection is a design pattern that allows you to inject dependencies into a class through
        //properties rather than through the constructor.
        private readonly HotelRepository _hotelRepository;
        public HotelsController(HotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        // GET: api/<HotelsController>
        [HttpGet]
        public ActionResult<IEnumerable<Hotel>> Get()
        {
            var hotels = _hotelRepository.GetAllHotels();
            return Ok(hotels); // Return 200 OK with the list of hotels
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public ActionResult<Hotel> Get(int id)
        {
            var hotel = _hotelRepository.GetHotelById(id);
            if(hotel == null)
            {
                return NotFound(); // Return 404 if the hotel is not found
            }
            return Ok(hotel); // Return 200 OK with the hotel
        }
        //Post is to insert     data into the database,
        //Put is to update data in the database,
        //Delete is to delete data from the database (Hard / Soft - Isdelete = 0)
        // POST api/<HotelsController>
        [HttpPost]
        public ActionResult Post([FromBody] Hotel newHotel)
        {
            var newHotelId = _hotelRepository.createHotel(newHotel);
            return CreatedAtAction(nameof(Get), new { id = newHotelId }, newHotel);
        }

        // PUT api/<HotelsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Hotel updatedHotel)
        {
            var existingHotel = _hotelRepository.GetHotelById(id);
            if (existingHotel == null)
            {
                return NotFound(); // Return 404 if the hotel is not found
            }
            var updated = _hotelRepository.updateHotel(id, updatedHotel);
            if(updated)
            {
                return Ok(updatedHotel);
            }
            return BadRequest("Update failed");
             
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _hotelRepository.deleteHotel(id);
            return NoContent();
        }
    }
}
