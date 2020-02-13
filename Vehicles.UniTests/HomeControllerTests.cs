using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Vehicules.Controllers;
using Vehicules.Services;
using Vehicules.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;
using Vehicules.ViewModels;

namespace Vehicles.UniTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private readonly Mock<IVehiculeRepository> _mockRepo;
        private readonly HomeController _controller;

        List<Vehicule> vehiculesLijst = new List<Vehicule>()
            {
                new Vehicule
                {
                    Id=1, Color=ColorEnum.Blue, Make = "make1", Model ="model1", Type = VehicleTypeEnum.Car, VIN = "Vin1"
                },
                new Vehicule
                {
                    Id=2, Color=ColorEnum.Green, Make = "make2", Model ="model2", Type = VehicleTypeEnum.Bus, VIN = "Vin2"
                },
                new Vehicule
                {
                    Id=3, Color=ColorEnum.Red, Make = "make3", Model ="model3", Type = VehicleTypeEnum.Truck, VIN = "Vin3"
                }
            };


        public HomeControllerTests()
        {





            _mockRepo = new Mock<IVehiculeRepository>();
            _mockRepo.Setup(x => x.GetAll()).Returns(vehiculesLijst);
            _mockRepo.Setup(x => x.Get(2)).Returns(vehiculesLijst[2]);

            var UpdatedVehicule = new Vehicule() { Id = 1, Color = ColorEnum.Blue, Make = "UpdatedMake", Model = "UpdatedModel", Type = VehicleTypeEnum.Car, VIN = "UpdatedVIN" };
            _mockRepo.Setup(x => x.Update(UpdatedVehicule));

            _mockRepo.Setup(x => x.Delete(vehiculesLijst[2])).Callback<Vehicule>(v=>vehiculesLijst.Remove(v)) ;
            

            _controller = new HomeController(_mockRepo.Object);
        }


        //--------------------------------------INDEX----------------------------------------------------
        //check if index returns an ok response
        [TestMethod]
        public void Index_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult), "index has not returned a okResutl");
        }

        //checks of index returns the same list of vehicules
        [TestMethod]
        public void Index_ShouldReturnAllVehicules()
        {
            //arrange

            //act
            var result = _controller.Index() as OkObjectResult;
            var lijstFromResult = (List<Vehicule>)result.Value;
            //assert
            Assert.AreEqual(vehiculesLijst, lijstFromResult, "index has not returned the same vehiculeList");
        }

        //-----------------------------------------DETAIL-----------------------------------------
        //check for notFoundResult  
        [TestMethod]
        public void Detail_InvalidIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.Detail(100);

            // Assert
            Assert.IsInstanceOfType(notFoundResult, typeof(Microsoft.AspNetCore.Mvc.NotFoundResult), "detail have not returned notfound error when given an invalid id");
        }

        //check for okResult
        [TestMethod]
        public void Detail_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            int testId = 2;

            // Act
            var okResult = _controller.Detail(testId);

            // Assert
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        //check for returning the right vehicule
        [TestMethod]
        public void GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Arrange
            int testId = 2;

            // Act
            var okResult = _controller.Detail(testId) as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(Vehicule), "object returned is not a vehicule type");
            var vehicule = (Vehicule)okResult.Value;
            // Assert

            Assert.AreEqual(vehiculesLijst[testId], vehicule, "the vehicule returned and the vehicule expected are not the same");
        }

        //----------------------------------------CREAT-----------------------------------------------------------------
        //check the BadRequestResponse when invalid vehicule has been passed
        [TestMethod]
        public void Create_InvalidVehiculePassed_ReturnsBadRequest()
        {
            // Arrange
            var VINMissingVehicule = new VehiculeCreateViewModel()
            {
                Model = "bad model",
                Make = "missing properties",
                Color = ColorEnum.Blue,
                Type = VehicleTypeEnum.Bus
            };

            _controller.ModelState.AddModelError("VIN", "Required");

            // Act
            var badResponse = _controller.Create(VINMissingVehicule);

            // Assert
            Assert.IsInstanceOfType(badResponse, typeof(BadRequestObjectResult));
        }

        //check the returntype when a valid vehicule has been passed
        [TestMethod]
        public void Create_ValidVehiculePassed_ReturnsCreatedResponse()
        {
            // Arrange
            var newVehicule = new VehiculeCreateViewModel()
            {
                Color = ColorEnum.Blue,
                Make = "makeNewVehicule",
                Model = "modelNewVehicule",
                Type = VehicleTypeEnum.Car,
                VIN = "VINNewVehicule"
            };

            // Act
            var createdResponse = _controller.Create(newVehicule);

            // Assert
            Assert.IsInstanceOfType(createdResponse, typeof(CreatedAtActionResult));
        }


        //check if the vehicule was created successfully
        [TestMethod]
        public void Create_ValidVehiculePassed_ReturnedResponseHasCreatedVehicule()
        {
            // Arrange
            var testVehiculeCreateViewModel = new VehiculeCreateViewModel()
            {
                Color = ColorEnum.Blue,
                Make = "makeNewVehicule",
                Model = "modelNewVehicule",
                Type = VehicleTypeEnum.Car,
                VIN = "VINNewVehicule"
            };
            var testVehicule = new Vehicule()
            {
                Color = ColorEnum.Blue,
                Make = "makeNewVehicule",
                Model = "modelNewVehicule",
                Type = VehicleTypeEnum.Car,
                VIN = "VINNewVehicule"
            };
            _mockRepo.Setup(r => r.add(It.IsAny<Vehicule>()))
                .Callback<Vehicule>(x => testVehicule = x);

            // Act
            var createdResponse = _controller.Create(testVehiculeCreateViewModel) as CreatedAtActionResult;
            Assert.IsInstanceOfType(createdResponse.Value, typeof(Vehicule));
            var vehicule = createdResponse.Value as Vehicule;
            // Assert

            Assert.AreEqual(testVehicule, vehicule);
        }


        //-------------------------------------------UPDATE----------------------------------------
        //checks not foundResponse in case of invalid id 
        [TestMethod]
        public void Update_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var testVehiculeUpdateViewModel = new VehiculeUpdateViewModel()
            {
                Color = ColorEnum.Blue,
                Make = "makeNewVehicule",
                Model = "modelNewVehicule",
                Type = VehicleTypeEnum.Car,
                VIN = "VINNewVehicule"
            };
            // Act
            var notFoundResponse = _controller.Update(500,testVehiculeUpdateViewModel);

            // Assert

            Assert.IsInstanceOfType(notFoundResponse, typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        //checks for okResult when an valid id has been passed
        [TestMethod]
        public void Update_ExistingIdPassed_ReturnsNoContentResult()
        {
            // Arrange
            int existingId = 2;
            var testVehiculeUpdateViewModel = new VehiculeUpdateViewModel()
            {
                Color = ColorEnum.Blue,
                Make = "makeNewVehicule",
                Model = "modelNewVehicule",
                Type = VehicleTypeEnum.Car,
                VIN = "VINNewVehicule"
            };
            // Act
            var noContentResponse = _controller.Update(existingId, testVehiculeUpdateViewModel);

            // Assert
            Assert.IsInstanceOfType(noContentResponse, typeof(NoContentResult));
        }

        
        //-------------------------------------------DELETE----------------------------------------
        //checks not foundResponse in case of invalid id 
        [TestMethod]
        public void Delete_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            int notExistingId = 800;
            // Act
            var notFoundResponse = _controller.Delete(notExistingId);

            // Assert

            Assert.IsInstanceOfType(notFoundResponse, typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        //checks when okResult when an valid id has been passed
        [TestMethod]
        public void Delete_ExistingIdPassed_ReturnsNoContentResult()
        {
            // Arrange
            int existingId = 2;
            // Act
            var noContentResponse = _controller.Delete(existingId);

            // Assert
            Assert.IsInstanceOfType(noContentResponse, typeof(NoContentResult));
        }

        //checks if vehicule has been removed when a valid id has been passed
        [TestMethod]
        public void Delete_ExistingIdPassed_RemovesOneVehicule()
        {
            // Arrange
            int existingId = 2;
            // Act
            var okResponse = _controller.Delete(existingId);

            // Assert
            Assert.AreEqual(2, vehiculesLijst.Count);
        }
    }
}
