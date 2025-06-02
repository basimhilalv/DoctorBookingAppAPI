﻿using DoctorBookingApp.AppResponse;
using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.PatientModel.Dto;
using DoctorBookingApp.Models.TimeSlotModel.Dto;
using DoctorBookingApp.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorBookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [Authorize(Roles ="Doctor")]
        [HttpPost("SetTimeSlot")]
        public async Task<IActionResult> SetTimeSlot(SetScheduleDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.GenerateTimeSlot(userIdguid, request);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Time slot can't be created"));
                }
                return Ok(new ApiResponse<string>(200, result));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles ="Doctor")]
        [HttpDelete("RemoveTimeSlot/{id}")]
        public async Task<IActionResult> RemoveOneTimeSlot(Guid id)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.RemoveOneTimeSlot(userIdguid, id);
                if(result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Time Slot deletion unsuccessfull"));
                }
                return Ok(new ApiResponse<string>(200, result));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles ="Doctor")]
        [HttpDelete("RemoveAllTimeSlot")]
        public async Task<IActionResult> RemoveAllTimeSlot()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.RemoveAllTimeSlot(userIdguid);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Time Slots deletion unsuccessfull"));
                return Ok(new ApiResponse<string>(200, result));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetDoctorProfile()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.GetDoctorProfile(userIdguid);
                if (result is null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile deosn't exists"));
                }
                return Ok(new ApiResponse<DoctorResDto>(200, "User profile retrieved successfully", result, null));
            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost("CreateProfile")]
        public async Task<IActionResult> CreateDoctorProfile([FromForm] DoctorReqDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.CreateDoctorProfile(userIdguid, request);
                if (result is null)
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
        [Authorize(Roles = "Doctor")]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateDoctorProfile([FromForm] DoctorReqDto request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.UpdateDoctorProfile(userIdguid, request);
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
        [Authorize(Roles = "Doctor")]
        [HttpDelete("DeleteProfile")]
        public async Task<IActionResult> DeleteDoctorProfile()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User is not authorized"));
                }
                Guid userIdguid = Guid.Parse(userId);
                var result = await _doctorService.DeleteDoctorProfile(userIdguid);
                if (result is null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "User Profile deletion unsuccessfull"));
                return Ok(new ApiResponse<string>(200, result));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
