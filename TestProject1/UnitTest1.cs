using KaniniTourism.Controllers;
using KaniniTourism.Models;
using KaniniTourism.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1
{
    public class Tests
    {

        private TouristPlacesController _controller;
        private Mock<ITouristPlacesRepository> _mockRepository;
        private Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        [SetUp]
        public void Setup()
        {

            _mockRepository = new Mock<ITouristPlacesRepository>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new TouristPlacesController(_mockRepository.Object, _mockWebHostEnvironment.Object);
        }

        [Test]
        public void Get_ShouldReturnListOfTouristPlaces()
        {
            
            var places = new List<TouristPlaces>
    {
        new TouristPlaces { TouristId = 1, TouristName = "Place 1", Location = "Location 1" },
        new TouristPlaces { TouristId = 2, TouristName = "Place 2", Location = "Location 2" }
    };

            _mockRepository.Setup(repo => repo.GetAll()).Returns(places);
 
        
            var result = _controller.Get() as OkObjectResult;

           
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var returnedPlaces = result.Value as List<TouristPlaces>;
            Assert.IsNotNull(returnedPlaces);
            Assert.AreEqual(2, returnedPlaces.Count);
        }

        [Test]
        public void Get_WithValidId_ShouldReturnTouristPlace()
        {
            
            var placeId = 1;
            var place = new TouristPlaces { TouristId = placeId, TouristName = "Test Place", Location = "Test Location" };
            _mockRepository.Setup(repo => repo.GetById(placeId)).Returns(place);

           
            var result = _controller.Get(placeId) as OkObjectResult;

           
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var returnedPlace = result.Value as TouristPlaces;
            Assert.IsNotNull(returnedPlace);
            Assert.AreEqual(placeId, returnedPlace.TouristId);
        }

    }
}