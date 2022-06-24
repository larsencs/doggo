using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {

        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
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

        public void AddWalk(Walks walk)
        {
            using (SqlConnection conn = Connection)
            { 
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"INSERT INTO Walks (Duration, DogId, WalkerId, Date )
                                        OUTPUT INSERTED.Id
                                         VALUES (@duration, @dogId, @walkerId, @date)";

                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@date", walk.Date);

                    int id = (int)cmd.ExecuteScalar();

                    walk.Id = id;

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void DeleteWalk(Walks walk)
        {
            throw new System.NotImplementedException();
        }

        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT * FROM Walks";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walks> walks = new List<Walks>();
                        while (reader.Read())
                        {
                            Walks walk = new Walks 
                            {
                               Id = reader.GetInt32(reader.GetOrdinal("Id")),
                               Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                               Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                               WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                               DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                            };

                            walks.Add(walk);

                        }

                        reader.Close();
                        return walks;
                    
                    }
                }
            }
        }

        public Walks GetWalk(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Walks WHERE Id=@id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Walks walk = new Walks 
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId"))
                        };

                        return walk;
                    }
                    return null;
                }
                conn.Close();
            }
        }

        public List<Walks> GetWalkByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select * FROM Walks WHERE WalkerId=@walkerId";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walks> walks = new List<Walks>();
                        while (reader.Read())
                        {
                            Walks walk = new Walks 
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId"))
                            };

                            walks.Add(walk);
                        }
                        reader.Close();
                        return walks;
                    }
                }
            
            }
        }

        public void UpdateWalk(Walks walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Walks SET Duration=@duration, Date=@date, DogId=@dogId, WalkerId=@walkerId WHERE Id=@id";

                    cmd.Parameters.AddWithValue("@id", walk.Id);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);

                    cmd.ExecuteNonQuery();

                }
            }
        }
    }
}
