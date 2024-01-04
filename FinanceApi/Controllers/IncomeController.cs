﻿using FinanceApi.Data.Dtos;
using FinanceApi.Mapper;
using FinanceApi.Models;
using FinanceApi.Services;
using FinanceApi.Services.Interfaces;
using FinanceApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : Controller
    {
        private readonly IIncomeService incomeService;
        private readonly IUserService userService;

        public IncomeController(IIncomeService incomeService, IUserService userService)
        {
            this.incomeService = incomeService;
            this.userService = userService;
        }

        [HttpGet("current/incomes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUsersIncomes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || !userService.ExistsById(userId))
            {
                return NotFound();
            }

            ICollection<IncomeDto> incomeDtos = incomeService.GetAllByUserId(userId).Select(Map.ToIncomeDto).ToList();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(incomeDtos);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateIncome([FromBody] IncomeDto incomeDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = userService.GetUserById(userId);

            int errorCode;
            string errorMessage;

            if (!incomeService.Create(user, incomeDto, out errorCode, out errorMessage))
            {

                switch (errorCode)
                {
                    case 400:
                        return BadRequest(errorMessage);
                    case 500:
                        return StatusCode(errorCode, errorMessage);
                    default:
                        throw new InvalidOperationException("Unexpected error code encountered.");
                }
            }


            if (incomeDto.Status && !userService.UpdateBalance(user, incomeDto.Amount))
            {
                ModelState.AddModelError("UpdatingError", "Something went wrong while updating userbalance.");
            }

            return Ok("Income created succesfully.");
        }



        [HttpPost("AssociateCategories/{incomeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult AddCategoryToIncome(int incomeId, [FromBody] ICollection<int> categoryIds)
        {
            string errorMessage;
            int responseCode;
            if (!incomeService.AddCategories(User.FindFirst(ClaimTypes.NameIdentifier).Value, incomeId, categoryIds, out errorMessage, out responseCode))
            {
                switch (responseCode)
                {
                    case 400:
                        return BadRequest(errorMessage);
                    case 404:
                        return NotFound(errorMessage);
                    case 500:
                        return StatusCode(500, errorMessage);
                    default:
                        break;
                }
            }

            return Ok("Categories successfully added to income.");
        }

        [HttpPut("put")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateIncome(IncomeDto incomeDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = userService.GetUserById(userId);

            int errorCode;
            string errorMessage;
            decimal prevAmount;

            if (!incomeService.Update(user, incomeDto, out errorCode, out errorMessage, out prevAmount))
            {
                switch (errorCode)
                {
                    case 404:
                        return NotFound(errorMessage);
                    case 400:
                        return BadRequest(errorMessage);
                    case 500:
                        return StatusCode(errorCode, errorMessage);
                }
            }

            if(incomeDto.Status && !userService.UpdateBalance(user, incomeDto.Amount - prevAmount))
            {
                return StatusCode(500, "Something went wrong with updating users balance.");
            }
            else if (!incomeDto.Status && !userService.UpdateBalance(user, -prevAmount))
            {
                return StatusCode(500, "Something went wrong with updating users balance.");
            }


            return Ok("Updated income succesfully.");
        }


    }
}
