using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpacePark.Classes;

namespace SpacePark.App.Classes
{
    class DBModel : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=SpacePark.db");
    }
}