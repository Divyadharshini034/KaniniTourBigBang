using KaniniTourism.Models;
using KaniniTourism.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaniniTourism.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristPlacesController : ControllerBase
    {
        private readonly ITouristPlacesRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public TouristPlacesController(ITouristPlacesRepository repository, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

       
        [HttpGet]
        public IActionResult Get()
        {
            var places = _repository.GetAll();
            return Ok(places);
        }

       
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var place = _repository.GetById(id);
            if (place == null)
                return NotFound();

            return Ok(place);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TouristPlaces touristPlace)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(touristPlace);
                return CreatedAtAction(nameof(Get), new { id = touristPlace.TouristId }, touristPlace);
            }

            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] TouristPlacesViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageData;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        model.ImageFile.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        imageData = Convert.ToBase64String(imageBytes);
                    }
                }
                else
                {
                    
                    imageData = string.Empty;
                }

                var existingPlace = _repository.GetById(id);
                if (existingPlace == null)
                    return NotFound();

                existingPlace.TouristName = model.TouristName;
                existingPlace.Source=model.Source;
                existingPlace.Destination=model.Destination;
                existingPlace.CheckIn= model.CheckIn;
                existingPlace.CheckOut= model.CheckOut;
                existingPlace.TouristDate=model.TouristDate;
                existingPlace.Location=model.Location;
                existingPlace.AgencyName = model.AgencyName;
                existingPlace.MobileNo = model.MobileNo;
                existingPlace.Price = model.Price;
                existingPlace.ImageData = imageData; 

                _repository.Update(existingPlace);

                return Ok(existingPlace);
            }

            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingPlace = _repository.GetById(id);
            if (existingPlace == null)
                return NotFound();

            _repository.Delete(id);
            return Ok(existingPlace);
        }

        [HttpPost("ByImage")]
        public IActionResult Post([FromForm] TouristPlacesViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageData;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        model.ImageFile.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        imageData = Convert.ToBase64String(imageBytes);
                    }
                }
                else
                {
                  
                    imageData = string.Empty;
                }

                var touristPlace = new TouristPlaces
                {
                    TouristName = model.TouristName,
                    Location= model.Location,

                    Source = model.Source,
                    Destination= model.Destination,
                    CheckIn=model.CheckIn,
                    CheckOut=model.CheckOut,
                    TouristDate = model.TouristDate,
                    AgencyName = model.AgencyName,
                    MobileNo = model.MobileNo,
                    Price = model.Price,
                    ImageData = imageData
                };

                _repository.Add(touristPlace);
                return CreatedAtAction(nameof(Get), new { id = touristPlace.TouristId }, touristPlace);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> GetByCriteria(
            [FromQuery] string source,
            [FromQuery] string destination,
            [FromQuery] string checkIn,
            [FromQuery] string checkOut)
        {
            var places = await _repository.GetTouristPlacesByCriteria(source, destination, checkIn, checkOut);
            return Ok(places);
        }



    }
}
