using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.UnitTests
{
    public class RestaurantServiceTests
    {
        private RestaurantService _sut;
        private Mock<RestaurantDbContext> _dbContextMoq;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RestaurantService>> _loggerMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private delegate void UpdateDelegate(int id, UpdateRestaurantDto dto);

        [SetUp]
        public void Setup()
        {
            _dbContextMoq = new Mock<RestaurantDbContext>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RestaurantService>>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _userContextServiceMock = new Mock<IUserContextService>();

            _sut = new RestaurantService(_dbContextMoq.Object, _mapperMock.Object, _loggerMock.Object,
                _authorizationServiceMock.Object, _userContextServiceMock.Object);

        }

        [Test]
        public void Update_Wrong_Id_Sended_Throws_Exception()
        {
            var restaurants = GetRestaurants();
            var restaurantMock = CreateDbSetMock<Restaurant>(restaurants);
            _dbContextMoq.Setup(x => x.Restaurants).Returns(restaurantMock.Object);
            UpdateDelegate updateDelegate = _sut.Update;

            Assert.Throws<NotFoundException>(() => updateDelegate(0, new UpdateRestaurantDto()));
        }

        [Test]
        public void Update_Correct_Id_Sended_Restaurant_Updated()
        {
            var restaurants = GetRestaurants();
            var restaurantMock = CreateDbSetMock<Restaurant>(restaurants);
            // Here is a problem the Task.Result is null //
            var taskWithSuccessAuthorizationResult = new Task<AuthorizationResult>(() => AuthorizationResult.Success());
            _dbContextMoq.Setup(x => x.Restaurants).Returns(restaurantMock.Object);
            _authorizationServiceMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<Restaurant>(), 
                                                        It.IsAny<List<ResourceOperationRequirement>>()))
                .Returns(taskWithSuccessAuthorizationResult);

            var expectedName = "Expected Name";
            var expectedDesc = "Expected Desc";
            var expectedDelivery = true;

            var restaurantDto = new UpdateRestaurantDto();
            restaurantDto.Name = expectedName;
            restaurantDto.Description = expectedDesc;
            restaurantDto.HasDelivery = expectedDelivery;

            _sut.Update(1, restaurantDto);
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Id = 1,
                    Name = "TestName1",
                    Description = "First desc",
                    Category = "First category",
                    HasDelivery = false
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "TestName2",
                    Description = "Second desc",
                    Category = "Second category",
                    HasDelivery = false
                }
            };

            return restaurants;
        }

        private Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            var elementsAsQuerable = elements.AsQueryable();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQuerable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQuerable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQuerable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => elementsAsQuerable.GetEnumerator());

            return dbSetMock;
        }
    }
}