using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Crystal.BusinessLogic;
using Crystal.DomainModel;
namespace EFContext
{

    public static class ContextHelper
    {
        private static EfDbContext context;
        private static object objLock = new object();

        public static void Open()
        {
            lock (objLock)
            {
                if (context != null)
                    throw new InvalidOperationException("Already opened");
                context = new EfDbContext();
                context.Database.Connection.Open();
            }
        }

    }

    public class EfDbContext : DbContext 
    {
        public DbSet<TrackedDevice> TrackedDevices { get; set; }
        public DbSet<TrackedBatch> TrackedBatches { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackedDevice>().HasKey(a => a.RecordId);
            modelBuilder.Entity<TrackedBatch>().HasKey(a => a.RecordId);
        }
    }

    public class DeviceDataManagerEF : ITrackingDataManager
    {

        public void AddItemTo(TrackedItem item)
        {
            using (var ctx = new EfDbContext())
            {
                ctx.TrackedDevices.Add(item as TrackedDevice);
                //ctx.Entry(item).State = System.Data.EntityState.Added;
                ctx.SaveChanges();
            }
        }

        public void RemoveItemFrom(TrackedItem item)
        {
            using (var ctx = new EfDbContext())
            {
                var delItem = item as TrackedDevice;
                ctx.TrackedDevices.Remove(
                    ctx.TrackedDevices.First(a => a.UserId == delItem.UserId && a.ProdNumber == delItem.ProdNumber && a.DeviceNumber == delItem.DeviceNumber)
                       );
                //ctx.Entry(delItem).State = System.Data.EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        public IEnumerable<TrackedItem> GetTrackedItemsByUser(int userId)
        {
            using (var ctx = new EfDbContext())
            {
                return ctx.TrackedDevices.Where(a => a.UserId == userId).ToArray();
            }
        }
    }
    public class BatchDataManagerEF : ITrackingDataManager
    {
       
        public void AddItemTo(TrackedItem item)
        {
            using (var ctx = new EfDbContext())
            {
                ctx.TrackedBatches.Add(item as TrackedBatch);
                ctx.SaveChanges();
            }
        }

        public void RemoveItemFrom(TrackedItem item)
        {
            using (var ctx = new EfDbContext())
            {
                var delItem = item as TrackedBatch;
                ctx.TrackedBatches.Remove(
                    ctx.TrackedBatches.First(a => a.UserId == delItem.UserId && a.ProdNumber == delItem.ProdNumber && a.BatchNumber == delItem.BatchNumber));
                ctx.SaveChanges();
            }
        }

        public IEnumerable<TrackedItem> GetTrackedItemsByUser(int userId)
        {
            using (var ctx = new EfDbContext())
            {
                return ctx.TrackedBatches.Where(a => a.UserId == userId).ToArray();
            }
        }
    }

}
