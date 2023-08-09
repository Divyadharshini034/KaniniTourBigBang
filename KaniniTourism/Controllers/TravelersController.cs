using KaniniTourism.Models;
using KaniniTourism.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Net;
using System.Text;

namespace KaniniTourism.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelersController : ControllerBase
    {
        private readonly ITravelerRepository _travelerRepository;
        private readonly ITouristPlacesRepository _touristPlacesRepository;
        private readonly IBookingRepository _bookingRepository;
        public TravelersController(ITravelerRepository travelerRepository, ITouristPlacesRepository touristPlacesRepository, IBookingRepository bookingRepository)
        {
            _travelerRepository = travelerRepository;
            _touristPlacesRepository = touristPlacesRepository;
            _bookingRepository = bookingRepository;
        }

        [HttpGet("GetAllTravelers")]
        public IActionResult GetAllTravelers()
        {
            try
            {
                var travelers = _travelerRepository.GetAllTravelers();
                return Ok(travelers);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching all travelers.");
            }
        }


        [HttpPost("Traveler")]
        public IActionResult CreateTraveler([FromBody] Travelers traveler)
        {
            

            _travelerRepository.Add(traveler);

            return CreatedAtAction(nameof(GetTravelerById), new { id = traveler.TravelerId }, traveler);
        }


        [HttpPost("BookTourByTouristId/{touristId}")]
        public async Task<IActionResult> BookTourByTouristId(int touristId, [FromBody] Travelers travelerDetails)
        {
            if (string.IsNullOrEmpty(travelerDetails.TravelerName) || string.IsNullOrEmpty(travelerDetails.TravelerPass))
            {
                return BadRequest("Invalid traveler details.");
            }

            // Fetch the traveler based on the provided details
            var traveler = await _travelerRepository.GetTravelerByNameAndPassword(travelerDetails.TravelerName, travelerDetails.TravelerPass);

            if (traveler == null)
            {
                return BadRequest("Invalid traveler credentials.");
            }

            // Fetch the tourist place details by TouristId
            var touristPlace = await _touristPlacesRepository.GetTouristPlaceById(touristId);

            if (touristPlace == null)
            {
                return NotFound("Tourist place with the specified TouristId does not exist.");
            }

            // Create a booking entry for the traveler and tourist place
            var booking = new BookingData
            {
                TravelerId = traveler.TravelerId,
                TouristId = touristPlace.TouristId,
                // You can set other properties related to the booking here, such as BookingDate, etc.
            };

            // Save the booking entry to the database or perform other necessary operations
            _bookingRepository.AddBooking(booking);

            return Ok("Booking successful!");
        }


        [HttpGet("GetBillingPdf/{travelerId}")]
        public async Task<IActionResult> GetBillingPdf(int travelerId)
        {
            try
            {
                // Get the traveler details based on the travelerId
                var traveler = _travelerRepository.GetTravelerById(travelerId);
                if (traveler == null)
                {
                    return NotFound("Traveler with the specified ID does not exist.");
                }

                // Get the booking for the given travelerId (Assuming Booking is the model)
                var booking = _bookingRepository.GetBookingByTravelerId(travelerId);
                if (booking == null)
                {
                    return NotFound("Booking for the specified traveler ID does not exist.");
                }

                // Get the tourist place associated with the booking
                var touristPlace = _touristPlacesRepository.GetById(booking.TouristId);

                if (touristPlace == null)
                {
                    return NotFound("Tourist place with the specified ID does not exist.");
                }

                // Build the billing details text
                var billingDetails = $"Billing Details\n\n" +
                                     $"Traveler Name: {traveler.TravelerName}\n" +
                                     $"Email: {traveler.TravelerEmail}\n" +
                                     $"Tourist Name: {touristPlace.TouristName}\n" +
                                      $"Tourist Date: {touristPlace.TouristDate}\n" +
                                       $"Price: {touristPlace.Price}\n" +
                                         $"Our Contact Details:{touristPlace.AgencyName}\t+ {touristPlace.MobileNo}\n";
                                    

                // Set the response headers for downloading the text file
                Response.Headers.Add("Content-Disposition", "attachment; filename=billing_details.txt");
                Response.ContentType = "text/plain";

                // Convert the billing details to bytes
                var billingBytes = Encoding.UTF8.GetBytes(billingDetails);

                // Write the billing details to the response asynchronously
                await Response.Body.WriteAsync(billingBytes, 0, billingBytes.Length);

                return new EmptyResult();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while generating the billing details text.");
            }
        }


        // Helper method to fetch tourist place details by travelerId (Replace this with your actual implementation)
        [HttpGet("GetBillingPdfs/{travelerName}")]
        public async Task<IActionResult> GetBillingPdfs(string travelerName)
        {
            try
            {
                // Get the traveler details based on the travelerName
                var traveler = _travelerRepository.GetTravelerByName(travelerName);
                if (traveler == null)
                {
                    return NotFound("Traveler with the specified name does not exist.");
                }

                // Get the booking for the given travelerId (Assuming Booking is the model)
                var booking = _bookingRepository.GetBookingByTravelerId(traveler.TravelerId);
                if (booking == null)
                {
                    return NotFound("Booking for the specified traveler does not exist.");
                }

                // Get the tourist place associated with the booking
                var touristPlace = _touristPlacesRepository.GetById(booking.TouristId);

                if (touristPlace == null)
                {
                    return NotFound("Tourist place with the specified ID does not exist.");
                }

                // Build the billing details text
                var billingDetails = $"Billing Details\n\n" +
                                     $"Traveler Name: {traveler.TravelerName}\n" +
                                     $"Email: {traveler.TravelerEmail}\n" +
                                     $"Tourist Name: {touristPlace.TouristName}\n" +
                                      $"Tourist Date: {touristPlace.TouristDate}\n" +
                                       $"Price: {touristPlace.Price}\n" +
                                        $"Our Contact Details:{touristPlace.AgencyName}\t+ {touristPlace.MobileNo}\n" +
                                     $"Add more billing details here as needed.";

                // Set the response headers for downloading the text file
                Response.Headers.Add("Content-Disposition", "attachment; filename=billing_details.txt");
                Response.ContentType = "text/plain";

                // Convert the billing details to bytes
                var billingBytes = Encoding.UTF8.GetBytes(billingDetails);

                // Write the billing details to the response asynchronously
                await Response.Body.WriteAsync(billingBytes, 0, billingBytes.Length);

                return new EmptyResult();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while generating the billing details text.");
            }
        }








        

        [HttpGet("GetTravelerById/{id}")]
        public IActionResult GetTravelerById(int id)
        {
            var traveler = _travelerRepository.GetById(id);
            if (traveler == null)
            {
                return NotFound();
            }

            return Ok(traveler);
        }

        [HttpGet("GetTravelerDetailsByBookedTouristId/{touristId}")]
        public IActionResult GetTravelerDetailsByBookedTouristId(int touristId)
        {
            var travelers = _bookingRepository.GetTravelersByBookedTouristId(touristId);

            if (travelers == null || !travelers.Any())
            {
                return NotFound("No travelers found with the specified TouristId.");
            }

            var travelerDetails = travelers.Select(t => new
            {
                t.TravelerName,
                t.TravelerEmail,

                t.TravelerMobileNo,
                t.TravelerAddress,
                t.DOB,
                t.TravelerPass
            });

            return Ok(travelerDetails);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTraveler(int id)
        {
            try
            {
                _travelerRepository.Delete(id);
                return NoContent(); // Successful deletion, return 204 No Content
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the traveler.");
            }
        }


        [HttpPut("UpdateTraveler/{travelerName}")]
        public IActionResult UpdateTraveler(string travelerName, [FromBody] Travelers updatedTraveler)
        {
            try
            {
                var existingTraveler = _travelerRepository.GetTravelerByName(travelerName);

                if (existingTraveler == null)
                {
                    return NotFound("Traveler with the specified name does not exist.");
                }

                
                existingTraveler.TravelerName = updatedTraveler.TravelerName;
                existingTraveler.TravelerEmail = updatedTraveler.TravelerEmail;
                existingTraveler.TravelerPass = updatedTraveler.TravelerPass;

               
                _travelerRepository.Update(existingTraveler);

                return Ok(existingTraveler);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the traveler.");
            }
        }



    }
}
