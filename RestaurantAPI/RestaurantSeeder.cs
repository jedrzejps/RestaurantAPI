using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }


                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role
                {
                    Name = "User"
                },
                new Role
                {
                    Name = "Manager"
                },
                new Role
                {
                    Name = "Admin"
                }
            };

            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "Kentucky Fried Chicken",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Longer",
                            Price = 6.40M
                        },

                        new Dish()
                        {
                            Name = "Zinger",
                            Price = 8.80M
                        },
                    },
                    Address = new Address()
                    {
                        City = "Szczecin",
                        Street = "Bułgarska 12",
                        PostalCode = "73-115"
                    }
                },

                new Restaurant()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description = "McDonald’s Corporation",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Cheeseburger",
                            Price = 5.00M
                        },

                        new Dish()
                        {
                            Name = "Hamburger",
                            Price = 4.5M
                        },
                    },
                    Address = new Address()
                    {
                        City = "Szczecin",
                        Street = "aleja Wyzwolenia 10",
                        PostalCode = "73-115"
                    }
                }
            };

            return restaurants;
        }
    }
}
