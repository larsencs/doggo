using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class NeighborhoodRepository : INeighborhoodRepository
    {
        private readonly IConfiguration _iconfig;

        public NeighborhoodRepository(IConfiguration iconfig)
        {
            _iconfig = iconfig;
        }

        public SqlConnection Connection 
        {
            get 
            {
                return new SqlConnection(_iconfig.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Neighborhood> GetAllNeighborhoods()
        {
            using (SqlConnection conn = Connection)
            { 
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"SELECT Id,
                                                [Name]
                                           FROM Neighborhood";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Neighborhood> neighborhoods = new List<Neighborhood>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));

                        Neighborhood neighborhood = new Neighborhood 
                        {
                            Id = id,
                            Name = name,
                        };

                        neighborhoods.Add(neighborhood);
                    
                    }

                    reader.Close();
                    return neighborhoods;


                }

            }
        }

        public Neighborhood GetNeighborhood(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"SELECT Id,
                                                [Name]
                                           FROM Neighborhood
                                           WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int neighborhoodId = reader.GetInt32(reader.GetOrdinal("Id"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));

                        Neighborhood neighborhood = new Neighborhood
                        {
                            Id = neighborhoodId,
                            Name = name,
                        };

                        return neighborhood;


                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
