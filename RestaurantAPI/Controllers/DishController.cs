using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var newDishId = _service.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet("{id}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int id)
        {
            var dishDto = _service.GetById(restaurantId, id);

            return Ok(dishDto);
        }

        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            var dishesDtos = _service.GetAll(restaurantId);

            return Ok(dishesDtos);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int id)
        {
            _service.Delete(restaurantId, id);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policy = "Atleast20")]
        public ActionResult DeleteAll([FromRoute] int restaurantId)
        {
            _service.DeleteAllFromRestaurant(restaurantId);

            return NoContent();
        }
    }
}
