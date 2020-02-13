using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vehicules.Models;

namespace Vehicules.ViewModels
{
    public class VehiculeUpdateViewModel
    {
        [Required, MaxLength(80)]
        public string Make { get; set; }

        [Required, MaxLength(80)]
        public string Model { get; set; }

        [Required, MaxLength(80)]
        public string VIN { get; set; }

        public VehicleTypeEnum Type { get; set; }
        public ColorEnum Color { get; set; }
    }
}
