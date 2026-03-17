using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")] // Attribute routing to define the route for this controller
    [ApiController] 
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _repository;

        public HotelsController(IHotelRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<HotelsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            var hotels = await _repository.GetAllHotels();
            return Ok(hotels);
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
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
        // POST api/<HotelsController>
        [HttpPost]
        public async Task<ActionResult<Hotel>> Post([FromBody] Hotel newHotel)
        {
           if(ModelState.IsValid)
            {
                var createdHotel = await _repository.AddNewHotel(newHotel);
                return CreatedAtAction(nameof(Get), new { id = createdHotel.Id }, createdHotel);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<HotelsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> Put(int id, [FromBody] Hotel updatedHotel)
        {
         return  await _repository.UpdateHotelInfo(id, updatedHotel);
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
           return await _repository.DeleteHotelById(id);
           
        }
    }
}
