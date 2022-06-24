using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        public List<Walks> GetAllWalks();
        public Walks GetWalk(int id);

        public void AddWalk(Walks walk);

        public void DeleteWalk(Walks walk);

        public void UpdateWalk(Walks walk);

        public List<Walks> GetWalkByWalkerId(int walkerId);


    }
}
