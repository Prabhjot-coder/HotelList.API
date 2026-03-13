using Microsoft.Data.SqlClient;

namespace WebApplication4.Data
{
    public class HotelRepository
    {
        private readonly string _connectionString;

        public HotelRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Hotel> GetAllHotels()
        {
            var hotels = new List<Hotel>();

            // Create Connection to the database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Hotels";
                // create a SQL command to select all hotels
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    conn.Open();
                    // Execute (select) 
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            hotels.Add(new Hotel
                            {
                                Id = (int)reader["Id"],
                                HotelName = reader["HotelName"].ToString(),
                                Rating = (int)reader["Rating"],
                                Address = reader["Address"].ToString()
                            });
                        }
                    }

                }
            }
            return hotels;
        }

        public Hotel GetHotelById(int Id)
        {
            Hotel hotel = null;
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Hotels WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hotel = new Hotel
                            {
                                Id = (int)reader["Id"],
                                HotelName = reader["HotelName"].ToString(),
                                Rating = (int)reader["Rating"],
                                Address = reader["Address"].ToString()
                            };
                        }
                    }
                }
            }
            return hotel;
        }

        //Create a new hotel
        public int createHotel(Hotel newHotel)
        {
            int newHotelId = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Hotels (HotelName, Rating, Address)" +
                            " VALUES (@HotelName, @Rating, @Address); " +
                            "SELECT SCOPE_IDENTITY();";// Gets the last inserted ID
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HotelName", newHotel.HotelName);
                    cmd.Parameters.AddWithValue("@Rating", newHotel.Rating);
                    cmd.Parameters.AddWithValue("@Address", newHotel.Address);
                    conn.Open();
                    newHotelId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return newHotelId;
        }
        // Update an existing hotel
        public bool updateHotel(int id, Hotel updatedHotel)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Hotels SET HotelName = @HotelName, Rating = @Rating, Address = @Address " +
                            "WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HotelName", updatedHotel.HotelName);
                    cmd.Parameters.AddWithValue("@Rating", updatedHotel.Rating);
                    cmd.Parameters.AddWithValue("@Address", updatedHotel.Address);
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0; // Return true if the update was successful
        }
        //Delete a hotel
        public bool deleteHotel(int id)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Hotels WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0; // Return true if the delete was successful
        }

    }
}
