using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RestaurantAPI.Controllers;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.UnitTests
{
    public class RestaurantControllerTests
    {
        private RestaurantController _sut;
        private Mock<IRestaurantService> _restaurantServiceMock;

        [SetUp]
        public void SetUp()
        {
            _restaurantServiceMock = new Mock<IRestaurantService>();
            _sut = new RestaurantController(_restaurantServiceMock.Object);
        }

        [Test]
        public void Update_Returns_OkResult()
        {
            var actionResult = _sut.Update(0, new UpdateRestaurantDto());

            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public void Delete_Returns_NoContentResult()
        {
            var actionResult = _sut.Delete(0);

            Assert.IsInstanceOf<NoContentResult>(actionResult);
        }

        [Test]
        public void Create_Returns_CreatedResult_With_Correct_Uri()
        {
            var expectedId = 77;
            var expectedUri = $"/api/restaurant/{expectedId}";
            _restaurantServiceMock.Setup(service => service.Create(It.IsAny<CreateRestaurantDto>()))
                .Returns(expectedId);

            var actionResult = _sut.Create(new CreateRestaurantDto());

            Assert.IsInstanceOf<CreatedResult>(actionResult);
            var result = actionResult as CreatedResult;
            var returnedUri = result.Location;
            Assert.AreEqual(expectedUri, returnedUri);
        }



        [Test]
        public void GetAll_Returns_OkObjectResult_With_Correct_PagedResult()
        {
            var dtosList = GetRestaurantDtos();
            var expectedPageResult = new PagedResult<RestaurantDto>(dtosList, dtosList.Count, 5, 1);
            _restaurantServiceMock.Setup(service => service.GetAll(It.IsAny<RestaurantQuery>()))
                .Returns(expectedPageResult);

            var actionResult = _sut.GetAll(new RestaurantQuery());

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var result = actionResult.Result as OkObjectResult;
            var returnedPagedResult = result.Value as PagedResult<RestaurantDto>;
            Assert.AreEqual(expectedPageResult, returnedPagedResult);
        }

        [Test]
        public void Get_Returns_OkObjectResult_With_Correct_Dto()
        {
            var expectedDto = new RestaurantDto
            {
                Id = 1,
                Name = "Test1",
                Description = "First desc",
                Category = "First category",
                HasDelivery = true
            };
            _restaurantServiceMock.Setup(service => service.GetById(It.IsAny<int>()))
                .Returns(expectedDto);

            var actionResult = _sut.Get(0);

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var result = actionResult.Result as OkObjectResult;
            var returnedDto = result.Value as RestaurantDto;
            Assert.AreEqual(expectedDto, returnedDto);
        }

        private List<RestaurantDto> GetRestaurantDtos()
        {
            var restaurantDos = new List<RestaurantDto>
            {
                new RestaurantDto
                {
                    Id = 1,
                    Name = "Test1",
                    Description = "First desc",
                    Category = "First category",
                    HasDelivery = true
                },
                new RestaurantDto
                {
                    Id = 2,
                    Name = "Test2",
                    Description = "Second desc",
                    Category = "Second category",
                    HasDelivery = true
                },
                new RestaurantDto
                {
                    Id = 3,
                    Name = "Test3",
                    Description = "Third desc",
                    Category = "Third category",
                    HasDelivery = false
                }
            };

            return restaurantDos;
        }
    }
}
