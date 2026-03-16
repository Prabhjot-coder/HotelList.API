namespace WebApplication4.Data
{
    //Repository Pattern implementation for Hotel entity
    public class HotelRepository : IHotelRepository
    {
        public Task<Hotel> AddNewHotel(Hotel newHotel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteHotelById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Hotel>> GetAllHotels()
        {
            throw new NotImplementedException();
        }

        public Task<Hotel> GetHotelById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateHotelInfo(Hotel updateHotel)
        {
            throw new NotImplementedException();
        }
    }
}
