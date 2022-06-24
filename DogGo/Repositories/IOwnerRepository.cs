using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
        void AddOwner(Owner owner);

        void UpdateOwner(Owner owner);

        void DeleteOwner(int id);
        Owner GetOwnerByEmail(string email);
    }
}
