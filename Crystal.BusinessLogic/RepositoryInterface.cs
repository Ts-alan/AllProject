using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Crystal.BusinessLogic
{
    public enum Plants { plant43 = 3, plant47 = 4, plant12 = 12, plant20 = 20, plant_none = 0 };
   
    public interface IRepository
    {
        //IDbConnection GetConnection(Plants plant);
    }

    public abstract class CrystalRepository : IRepository
    {
        
        public Plants RepoPlant;
        public const string CONNECTION_STRING = "_ConnectionString";
       // public abstract IDbConnection GetConnection(Plants plant);
    }
    public interface ITrackingDataManager
    {

        void AddItemTo(TrackedItem item);

        void RemoveItemFrom(TrackedItem item);

        IEnumerable<TrackedItem> GetTrackedItemsByUser(int userId);
    }

}
