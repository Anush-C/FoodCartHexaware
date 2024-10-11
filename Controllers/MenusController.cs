using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Customer")]
    public class MenusController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ApplicationDbContext _context;

        public MenusController(IMenuRepository menu , ApplicationDbContext context)
        {
            _menuRepository = menu;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetMenuItem()
        {
            try
            {
                var menuitems = await _menuRepository.GetMenuItem();
                return Ok(new
                {
                    menuItem = menuitems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMenuItembyId(int id)
        {
            try
            {
                var menuitems = await _menuRepository.GetMenuItembyId(id);
                if (menuitems == null)
                {
                    return NotFound("Menu item not found");
                }

                return Ok(new
                {
                    menuItem = new
                    {
                        itemID = menuitems.ItemID,
                        itemName = menuitems.ItemName,
                        itemDescription = menuitems.ItemDescription,
                        itemPrice = menuitems.ItemPrice,
                        ingredients = menuitems.Ingredients,
                        cuisineType = menuitems.CuisineType,
                        tasteInfo = menuitems.TasteInfo,
                        availabilityStatus = menuitems.AvailabilityStatus,
                        dietaryInfo = menuitems.DietaryInfo,
                        imageURL = menuitems.ImageURL,
                        rating=menuitems.Rating,
                        restaurants = menuitems.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("byName")]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItembyName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Name parameter is required");
                }

                var menuitems = await _menuRepository.GetMenuItembyName(name);
                if (menuitems == null || !menuitems.Any())
                {
                    return NotFound("Menu items not found");
                }

                return Ok(new
                {
                    menuItem = menuitems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating  ,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("byCategory")]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItemByCategory(string categoryname)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryname))
                {
                    return BadRequest("Category parameter is required");
                }

                var menuitems = await _menuRepository.GetMenuItembyCategory(categoryname);
                if (menuitems == null || !menuitems.Any())
                {
                    return NotFound("Menu items not found");
                }

                return Ok(new
                {
                    menuItem = menuitems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("byCuisine")]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItemsByCuisine(string cuisineName)
        {
            try
            {
                if (string.IsNullOrEmpty(cuisineName))
                {
                    return BadRequest("CuisineName parameter is required");
                }

                var menuitems = await _menuRepository.GetMenuItemsbyCuisine(cuisineName);
                if (menuitems == null || !menuitems.Any())
                {
                    return NotFound("Menu items not found");
                }

                return Ok(new
                {
                    menuItem = menuitems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("byAvailability")]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItemsbyAvailability()
        {
            try
            {
                var menuitems = await _menuRepository.GetMenuItemsbyAvailability();
                if (menuitems == null || !menuitems.Any())
                {
                    return NotFound("No available items");
                }

                return Ok(new
                {
                    menuItem = menuitems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("byPriceRange")]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItemsbyPrice(int maxprice, int minprice)
        {
            try
            {
                if (minprice < 0 || maxprice <= 0 || minprice >= maxprice)
                {
                    return BadRequest("Invalid price range");
                }

                var menuitems = await _menuRepository.GetMenuItemsbyPrice(maxprice, minprice);
                if (menuitems == null || !menuitems.Any())
                {
                    return NotFound("Items not found");
                }

                return Ok(new
                {
                    menuItem = menuitems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("byFilters")]
        public async Task<ActionResult> GetMenuByFilters(string?type=null,
    string? category = null,
    string? cuisine = null,
    int? minPrice = null,
    int? maxPrice = null)
        {
            try
            {
                // Validate price range if provided
                if (minPrice.HasValue && maxPrice.HasValue && (minPrice < 0 || maxPrice <= 0 || minPrice >= maxPrice))
                {
                    return BadRequest("Invalid price range.");
                }

                // Call repository method to get filtered menu items
                var menuItems = await _menuRepository.GetMenuItemsByFilters(type,category,  minPrice, maxPrice,cuisine);

                if (menuItems == null || !menuItems.Any())
                {
                    return NotFound("No menu items found based on the provided filters.");
                }

                return Ok(new
                {
                    menuItems = menuItems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult> GetMenuItemsBySearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query is required.");
            }

            try
            {
                var menuItems = await _menuRepository.GetMenuItemsbySearch(query);

                if (menuItems == null || !menuItems.Any())
                {
                    return NotFound("No items found matching the search query.");
                }

                return Ok(new
                {
                    menuItems = menuItems.Select(mi => new
                    {
                        itemID = mi.ItemID,
                        itemName = mi.ItemName,
                        itemDescription = mi.ItemDescription,
                        itemPrice = mi.ItemPrice,
                        ingredients = mi.Ingredients,
                        cuisineType = mi.CuisineType,
                        tasteInfo = mi.TasteInfo,
                        availabilityStatus = mi.AvailabilityStatus,
                        dietaryInfo = mi.DietaryInfo,
                        imageURL = mi.ImageURL,
                        rating = mi.Rating,
                        restaurants = mi.Restaurants.Select(r => new
                        {
                            restaurantID = r.RestaurantID,
                            name = r.RestaurantName,
                            email = r.RestaurantEmail,
                            address = r.RestaurantAddress,
                            phoneNumber = r.RestaurantPhone,
                            description = r.RestaurantDescription,
                        }).ToList()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost("{restaurantId}/menuitem/{menuItemId}")]
        public async Task<IActionResult> LinkMenuItemToRestaurant(int restaurantId, int menuItemId)
        {
            try
            {
                // Call the service method to link the menu item to the restaurant
                await _menuRepository.LinkMenuItemToRestaurant(restaurantId, menuItemId);

                // Return a 200 OK response (no content needed since we're just linking)
                return Ok("Menu item successfully linked to restaurant.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error linking menu item to restaurant: {ex.Message}");
            }
        }

        [HttpGet("Location")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetMenuItemsByLocation(string location)
        {
            try
            {
                var res = await _menuRepository.GetMenuItemsByLocation(location);

                return Ok(new
                {
                    Restaurants = res.Select(mi => new
                    {
                        restaurantID = mi.RestaurantID,
                        restaurantName = mi.RestaurantName,
                        restaurantDescription = mi.RestaurantDescription,
                        restaurantPhone = mi.RestaurantPhone,
                        restaurantEmail = mi.RestaurantEmail,
                        menuitems = mi.MenuItems.Select(mi => new
                        {
                            itemID = mi.ItemID,
                            itemName = mi.ItemName,
                            itemDescription = mi.ItemDescription,
                            itemPrice = mi.ItemPrice,
                            ingredients = mi.Ingredients,
                            cuisineType = mi.CuisineType,
                            tasteInfo = mi.TasteInfo,
                            availabilityStatus = mi.AvailabilityStatus,
                            dietaryInfo = mi.DietaryInfo,
                            imageURL = mi.ImageURL,
                        })
                    })
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("restaurants")]
        public IActionResult GetRestaurantsByAddress(string address)
        {
            var restaurants = _context.Restaurants
                .Where(r => r.RestaurantAddress.Contains(address)) // Use Contains for partial matching
                .Select(r => new
                {
                    r.RestaurantID,
                    r.RestaurantName,
                    r.RestaurantDescription,
                    MenuItems = r.MenuItems.ToList()
                })
                .ToList();

            return Ok(restaurants);
        }

        [HttpGet]
        [Route("byRestaurant/{restaurantId}")]
        public async Task<ActionResult> GetMenuItemsByRestaurant(int restaurantId)
        {
            try
            {
                var menuItems = await _menuRepository.GetMenuItemsByRestaurantId(restaurantId);
                if (menuItems == null || !menuItems.Any())
                {
                    return NotFound("No menu items found for the specified restaurant.");
                }

                return Ok(menuItems.Select(mi => new
                {
                    itemID = mi.ItemID,
                    itemName = mi.ItemName,
                    itemDescription = mi.ItemDescription,
                    itemPrice = mi.ItemPrice,
                    ingredients = mi.Ingredients,
                    cuisineType = mi.CuisineType,
                    tasteInfo = mi.TasteInfo,
                    availabilityStatus = mi.AvailabilityStatus,
                    dietaryInfo = mi.DietaryInfo,
                    imageURL = mi.ImageURL
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpGet("other/{id}")]
        public async Task<ActionResult<ItemCartDTO>> GetMenuItemById(int id)
        {
            var itemNameDto = await _menuRepository.GetMenuItemNamebyItemId(id);

            if (itemNameDto == null)
            {
                return NotFound(); // Return 404 if the item is not found
            }

            return Ok(itemNameDto); // Return the item name wrapped in DTO
        }



    }
}
