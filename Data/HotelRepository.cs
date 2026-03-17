using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Data
{
    //Repository Pattern implementation for Hotel entity
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelContext _context;
        public HotelRepository(HotelContext context)
        {
            _context = context;
        }
        public async Task<Hotel> AddNewHotel(Hotel newHotel)
        {
            _context.Hotels.Add(newHotel);
            await _context.SaveChangesAsync();
            return newHotel;
        }

        public async Task<bool> DeleteHotelById(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotels()
        {
            return await _context.Hotels.OrderBy(s=>s.HotelName).ToListAsync(); 
        }

        public async Task<Hotel> GetHotelById(int id)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            return hotel;
        }

        public async Task<Hotel> UpdateHotelInfo(int id, Hotel updateHotel)
        {
           var existingHotel = await _context.Hotels.FindAsync(updateHotel.Id);
            if (existingHotel != null)
            {
                existingHotel.HotelName = updateHotel.HotelName;
                existingHotel.Rating = updateHotel.Rating;
                existingHotel.Address = updateHotel.Address;
                existingHotel.EmailAddress = updateHotel.EmailAddress;
                existingHotel.CreatedDate = updateHotel.CreatedDate;
            }

            await _context.SaveChangesAsync();

            return existingHotel;
        }
    }
}
