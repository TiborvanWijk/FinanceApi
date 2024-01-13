﻿using Azure;
using FinanceApi.Controllers.ApiResponseHelpers;
using FinanceApi.Data.Dtos;
using FinanceApi.Mapper;
using FinanceApi.Models;
using FinanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        [Authorize]
        [HttpGet("current")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!userService.ExistsById(userId))
            {
                return NotFound();
            }

            UserDto userDto = Map.ToUserDto(userService.GetById(userId, false));

            int errorCode;
            string errorMessage;
            decimal balance;

            if(!userService.TryGetUserBalance(userId, out balance, out errorCode, out errorMessage))
            {
                return ApiResponseHelper.HandleErrorResponse(errorCode, errorMessage);
            }
            userDto.Balance = balance;
            
            return Ok(userDto);
        }
    }
}
