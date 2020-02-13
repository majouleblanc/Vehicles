using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicules.Entities;
using Vehicules.Models;

namespace Vehicules.Services
{
    public class EfVehiculeRepository : IVehiculeRepository
    {
        private readonly VehiculeDbContext context;

        public EfVehiculeRepository(VehiculeDbContext context)
        {
            this.context = context;
        }
        public void add(Vehicule vehicle)
        {
            context.Add(vehicle);
            context.SaveChanges();
        }

        public void Delete(Vehicule vehicle)
        {
            context.Remove(vehicle);
            context.SaveChanges();
        }

        public Vehicule Get(int id)
        {
            return context.Vehicules.Find(id);
        }

        public IEnumerable<Vehicule> GetAll()
        {
            return context.Vehicules.ToList();
        }

        public void Update(Vehicule vehicle)
        {
            var vehiculeUpdate = context.Vehicules.FirstOrDefault(v=>v.Id==vehicle.Id);
            //var vehiculeUpdate = context.Attach(vehicle);
            //vehiculeUpdate.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            vehiculeUpdate.Make = vehicle.Make;
            vehiculeUpdate.Model = vehicle.Model;
            vehiculeUpdate.Type = vehicle.Type;
            vehiculeUpdate.VIN = vehicle.VIN;
            vehiculeUpdate.Color = vehicle.Color;
            context.SaveChanges();
        }
    }
}