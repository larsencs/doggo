using DogGo.Models;
using System;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        public List<Dog> GetAllDogs();

        public Dog GetById(int id);

        public void Update(Dog dog);

        public void Delete(int id);

        public void AddDog(Dog dog);

        List<Dog> GetDogsByOwnerId(int ownerId);

    }
}
