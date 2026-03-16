namespace WebApplication4.Data
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllHotels();
        Task<Hotel> GetHotelById(int id);
        Task<Hotel> AddNewHotel(Hotel newHotel);
        Task DeleteHotelById(int id);
        Task UpdateHotelInfo(Hotel updateHotel);
                
    }
}
