using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicules.Models;

namespace Vehicules.Services
{
    public class InMemoryVehiculeRepository : IVehiculeRepository
    {
        private static List<Vehicule> _VehicleList;

        static InMemoryVehiculeRepository()
        {
            _VehicleList = new List<Vehicule>()
            {
                new Vehicule()
                {
                    Id = 1,
                    Color = ColorEnum.Blue,
                    Type = VehicleTypeEnum.Car,
                    Make = "make 1",
                    Model = "model 1",
                    VIN = "VIN 1 "
                }
            };
        }
        public void add(Vehicule vehicle)
        {
            if (!_VehicleList.Contains(vehicle))
            {
                vehicle.Id = _VehicleList.Max(v => v.Id) + 1;
                _VehicleList.Add(vehicle);
            }
        }

        public void Delete(Vehicule vehicle)
        {
            if (_VehicleList.Contains(vehicle))
            {
                _VehicleList.Remove(vehicle);
            }
        }

        public Vehicule Get(int id)
        {
            Vehicule vehicle = _VehicleList.FirstOrDefault(v => v.Id == id);

            if (vehicle != null)
            {
                return vehicle;
            }
            throw new NullReferenceException("vehicule does not exist in the DB");
        }

        public IEnumerable<Vehicule> GetAll()
        {
            return _VehicleList;
        }

        public void Update(Vehicule vehicle)
        {
            var oldVehicule = Get(vehicle.Id);
            if (oldVehicule == null)
            {
                throw new NullReferenceException("the Vehicule you want to update does not exist in the DB");
            }

            oldVehicule.Make = vehicle.Make;
            oldVehicule.Model = vehicle.Model;
            oldVehicule.Type = vehicle.Type;
            oldVehicule.VIN = vehicle.VIN;
            oldVehicule.Color = vehicle.Color;
        }
    }
}
