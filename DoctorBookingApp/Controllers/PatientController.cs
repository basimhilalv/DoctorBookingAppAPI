using DoctorBookingApp.AppResponse;
using DoctorBookingApp.Models.PatientModel.Dto;
using DoctorBookingApp.Services.PatientService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorBookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("Profile")]
        public async Task<IActionResult> getUserProfile()
        {
            try
            {

                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.GetPatientProfile(userIdguid);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile deosn't exists"));
                }
                return Ok(new ApiResponse<PatientResDto>(200, "User profile retrieved successfully", result, null));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("CreateProfile")]
        public async Task<IActionResult> createUserProfile([FromForm] PatientReqDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401,"User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.CreatePatientProfile(userIdguid, request);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile can't be created"));
                }
                return Ok(new ApiResponse<string>(200, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> updateUserProfile([FromForm] PatientReqDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.UpdatePatientProfile(userIdguid, request);
                if (result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile can't be updated"));
                }
                return Ok(new ApiResponse<string>(200, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("DeleteProfile")]
        public async Task<IActionResult> deleteUserProfile()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _patientService.DeletePatientProfile(userIdguid);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile deletion unsuccessfull"));
                return Ok(new ApiResponse<string>(200, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
