using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")] // Attribute routing to define the route for this controller
    [ApiController] 
    public class HotelsController : ControllerBase
    {
        //In-memory data store for hotels (for demonstration purposes)
        private static List<Hotel> hotels = new List<Hotel>
        {
            new Hotel { Id = 1, HotelName = "Hotel A", Rating = 4, Address = "123 Main St" },
            new Hotel { Id = 2, HotelName = "Hotel B", Rating = 5, Address = "456 Elm St" },
            new Hotel { Id = 3, HotelName = "Hotel C", Rating = 3, Address = "789 Oak St" },
            new Hotel { Id = 4, HotelName = "Hotel D", Rating = 2, Address = "321 Pine St" },
        };
        // GET: api/<HotelsController>
        [HttpGet]
        public ActionResult<IEnumerable<Hotel>> Get()
        {
            return hotels;
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public ActionResult<Hotel> Get(int id)
        {
            var hotel = hotels.FirstOrDefault(h => h.Id == id);
            if(hotel == null)
            {
                return NotFound(); // Return 404 if the hotel is not found
            }
            return hotel;
        }
        //Post is to insert     data into the database,
        //Put is to update data in the database,
        //Delete is to delete data from the database (Hard / Soft - Isdelete = 0)
        // POST api/<HotelsController>
        [HttpPost]
        public ActionResult Post([FromBody] Hotel newHotel)
        {
            if (newHotel == null)
            {
                return BadRequest(); // Return 400 if the request body is null
            }
            var hotel = hotels.FirstOrDefault(h => h.Id == newHotel.Id);
            if (hotel != null)
            {
                return Conflict(); // Return 409 if a hotel with the same ID already exists
            }
            hotels.Add(newHotel);
            return CreatedAtAction(nameof(Get), new { id = newHotel.Id }, newHotel);
        }

        // PUT api/<HotelsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Hotel updatedHotel)
        {
            if(updatedHotel == null)
            {
                return BadRequest();
            }
            var hotel = hotels.FirstOrDefault(h=>h.Id == id);
             if(hotel == null)
            {
                return NotFound();
            }
            else
            { 
                hotel.HotelName = updatedHotel.HotelName;
                hotel.Rating = updatedHotel.Rating;
                hotel.Address = updatedHotel.Address;
            }
             return Ok();
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var hotel = hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotels.Remove(hotel);
            return NoContent();
        }
    }
}
