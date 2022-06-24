using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
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

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            { 
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"SELECT [Name],
                                                Email,
                                                Address,
                                                Phone,
                                                NeighborhoodId,
                                                Id
                                           FROM Owner";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> result = new List<Owner>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        string address = reader.GetString(reader.GetOrdinal("Address"));
                        string phone = reader.GetString(reader.GetOrdinal("Phone"));
                        int neighborhood = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"));
                        string email = reader.GetString(reader.GetOrdinal("Email"));

                        Owner owner = new Owner { 
                            Id = id,
                            Name = name,
                            Address = address,
                            Phone = phone,
                            NeighborhoodId = neighborhood,
                            Email = email
                        };

                        result.Add(owner);
                    
                    }
                    return result;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"SELECT [Name],
                                                Address,
                                                Email,
                                                Phone,
                                                NeighborhoodId
                                           FROM Owner
                                          WHERE Id=@id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        string address = reader.GetString(reader.GetOrdinal("Address"));
                        string phone = reader.GetString(reader.GetOrdinal("Phone"));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        int neighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"));

                        Owner owner = new Owner
                        {
                            Id = id,
                            Name = name,
                            Address = address,
                            Phone = phone,
                            Email = email,
                            NeighborhoodId = neighborhoodId
                        };

                        return owner;
                    }
                    else {

                        return null;
                    }
                
                }
            }
        }

        public Owner GetOwnerByEmail(string email)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT Id, [Name], Email, Address, Phone, NeighborhoodId
                                         FROM Owner
                                         WHERE Email = @email";

                    cmd.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Owner owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };

                            return owner;


                        }
                        return null;
                    }
                }
            }
        }

        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }

        public void UpdateOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"UPDATE Owner
                                         SET [Name] = @name, Address = @address, Phone = @phone, Email = @email, NeighborhoodId = @nId
                                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", owner.Id);
                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@nId", owner.NeighborhoodId);

                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id=@id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
