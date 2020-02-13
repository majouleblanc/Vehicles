using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicules.Models;

namespace Vehicules.Entities
{
    public class VehiculeDbContext : DbContext
    {
        public VehiculeDbContext(DbContextOptions<VehiculeDbContext> options) : base(options)
        {

        }

        public DbSet<Vehicule> Vehicules { get; set; }
    }
}
