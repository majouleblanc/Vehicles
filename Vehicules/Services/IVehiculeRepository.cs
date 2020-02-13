using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicules.Models;

namespace Vehicules.Services
{
    public interface IVehiculeRepository
    {
        IEnumerable<Vehicule> GetAll();
        Vehicule Get(int id);
        void add(Vehicule vehicule);
        void Delete(Vehicule vehicule);
        void Update(Vehicule vehicule);
    }
}
