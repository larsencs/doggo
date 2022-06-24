using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id AS personId, w.[Name] AS personName, ImageUrl, NeighborhoodId, n.Id AS neighborhoodId, n.[Name] AS neighborhoodName
                        FROM Walker w
                        LEFT JOIN Neighborhood n on w.NeighborhoodId = n.Id

                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    List<Neighborhood> neighborhoods = new List<Neighborhood>();
                    while (reader.Read())
                    {

                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("personId")),
                            Name = reader.GetString(reader.GetOrdinal("personName")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        Neighborhood neighborhood = new Neighborhood 
                        {
                            Id=reader.GetInt32(reader.GetOrdinal("neighborhoodId")), 
                            Name=reader.GetString(reader.GetOrdinal("neighborhoodName"))
                        };


                        walkers.Add(walker);
                        neighborhoods.Add(neighborhood);
                    }

                    foreach (Walker walker in walkers)
                    {
                        foreach (Neighborhood neighborhood in neighborhoods)
                        {
                            if (walker.NeighborhoodId == neighborhood.Id)
                            { 
                                walker.Neighborhood = neighborhood;
                            }
                        }
                    }
                    reader.Close();

                    return walkers;
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        return walker;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT Id, [Name], ImageUrl, NeighborhoodId
                                         FROM Walker
                                         WHERE NeighborhoodId = @neighborhoodId";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    { 
                        List<Walker> walkers = new List<Walker>();

                        while (reader.Read())
                        {
                            Walker walker = new Walker 
                            {
                                Id= reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            
                            };

                            walkers.Add(walker);
                        }
                        return walkers;
                    }
                }
            }
        }
    }
}