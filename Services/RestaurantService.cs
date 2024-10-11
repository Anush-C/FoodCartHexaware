using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.DTOs;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodCart_Hexaware.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Restaurant> GetRestaurantByIdAsync(int restaurantId)
        {
            // Fetch the restaurant by ID, including MenuItems if necessary
            var restaurant = await _context.Restaurants
                .Include(r => r.MenuItems) // Include related menu items if needed
                .FirstOrDefaultAsync(r => r.RestaurantID == restaurantId);

            return restaurant;
        }

        public DashBoardDTO GetDashboardData(int userId)
        {
            // Fetch restaurant details for the user
            var user = _context.Users.Include(u => u.restaurant)
                                     .FirstOrDefault(u => u.UserID == userId);

            if (user == null || user.RestaurantID == null)
            {
                throw new InvalidOperationException("User or restaurant not found.");
            }

            var restaurant = _context.Restaurants
                                      .Include(r => r.MenuItems)
                                      .Include(r => r.Orders)
                                      .FirstOrDefault(r => r.RestaurantID == user.RestaurantID);

            // Return the data needed for the dashboard
            return new DashBoardDTO
            {
                Restaurant = restaurant,
                Orders = restaurant.Orders.ToList(),
                MenuItems = restaurant.MenuItems.ToList()
            };
        }

        public void AddMenuItem(int userId, MenuItemDTO menuItemDTO)
        {
            var restaurant = GetRestaurantByUserId(userId);

            if (restaurant == null)
            {
                throw new InvalidOperationException("Restaurant not found for this user.");
            }

            var menuItem = new MenuItems
            {
                ItemName = menuItemDTO.Name,
                ItemDescription = menuItemDTO.Description,
                ItemPrice = menuItemDTO.Price,
                Ingredients = menuItemDTO.Ingredients,
                CuisineType = menuItemDTO.CuisineType,
                TasteInfo = menuItemDTO.TasteInfo,
                AvailabilityStatus = menuItemDTO.AvailabilityStatus,
                DietaryInfo = menuItemDTO.DietaryInfo,
                CategoryID = menuItemDTO.CategoryID,
                ImageURL = menuItemDTO.ImageURL,
            };

            restaurant.MenuItems.Add(menuItem);
            _context.SaveChanges();
        }

        public IEnumerable<MenuItemDTO> GetMenuItems(int restaurantId)
        {
            return _context.Menus
                           .Where(mi => mi.Restaurants.Any(r => r.RestaurantID == restaurantId))
                           .Select(mi => new MenuItemDTO
                           {
                               ItemID = mi.ItemID,
                               Name = mi.ItemName,
                               Description = mi.ItemDescription,
                               Price = mi.ItemPrice,
                               Ingredients = mi.Ingredients,
                               CuisineType = mi.CuisineType,
                               TasteInfo = mi.TasteInfo,
                               AvailabilityStatus = mi.AvailabilityStatus,
                               DietaryInfo = mi.DietaryInfo,
                               CategoryID = mi.CategoryID,
                               ImageURL = mi.ImageURL
                           }).ToList();
        }

        public void UpdateMenuItem(int userId, int itemId, MenuItemDTO menuItemDTO)
        {
            var restaurant = GetRestaurantByUserId(userId);

            if (restaurant == null)
            {
                throw new InvalidOperationException("Restaurant not found for this user.");
            }

            var menuItem = _context.Menus.FirstOrDefault(mi => mi.ItemID == itemId && mi.Restaurants.Any(r => r.RestaurantID == restaurant.RestaurantID));

            if (menuItem == null)
            {
                throw new InvalidOperationException("Menu item not found.");
            }

            menuItem.ItemName = menuItemDTO.Name;
            menuItem.ItemDescription = menuItemDTO.Description;
            menuItem.ItemPrice = menuItemDTO.Price;
            menuItem.Ingredients = menuItemDTO.Ingredients;
            menuItem.CuisineType = menuItemDTO.CuisineType;
            menuItem.TasteInfo = menuItemDTO.TasteInfo;
            menuItem.AvailabilityStatus = menuItemDTO.AvailabilityStatus;
            menuItem.DietaryInfo = menuItemDTO.DietaryInfo;
            menuItem.CategoryID = menuItemDTO.CategoryID;
            menuItem.ImageURL = menuItemDTO.ImageURL;

            _context.SaveChanges();
        }

        public void DeleteMenuItem(int userId, int itemId)
        {
            var restaurant = GetRestaurantByUserId(userId);

            if (restaurant == null)
            {
                throw new InvalidOperationException("Restaurant not found for this user.");
            }

            var menuItem = _context.Menus.FirstOrDefault(mi => mi.ItemID == itemId && mi.Restaurants.Any(r => r.RestaurantID == restaurant.RestaurantID));

            if (menuItem == null)
            {
                throw new InvalidOperationException("Menu item not found.");
            }

            _context.Menus.Remove(menuItem);
            _context.SaveChanges();
        }

        public Restaurant GetRestaurantByUserId(int userId)
        {
            return _context.Users.Include(u => u.restaurant)
                                 .Where(u => u.UserID == userId)
                                 .Select(u => u.restaurant)
                                 .FirstOrDefault();
        }





        public List<CategoryDTO> GetCategories(int restaurantId)
        {
            var categories = _context.MenuCategories
                .Where(c => c.MenuItems.Any(m => m.Restaurants.Any(r => r.RestaurantID == restaurantId)))
                .Select(c => new CategoryDTO
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.CategoryDescription,
                    MenuItems = c.MenuItems
                        .Where(m => m.Restaurants.Any(r => r.RestaurantID == restaurantId))
                        .Select(m => new MenuItemDTO
                        {
                            ItemID = m.ItemID,
                            Name = m.ItemName,
                            Description = m.ItemDescription,
                            Price = m.ItemPrice,
                            Ingredients = m.Ingredients,
                            CuisineType = m.CuisineType,
                            DietaryInfo = m.DietaryInfo,
                            TasteInfo = m.TasteInfo,
                            AvailabilityStatus= m.AvailabilityStatus,
                            CategoryID = m.CategoryID
                        })
                        .ToList() // Convert to list
                })
                .ToList();

            return categories;
        }


        public MenuCategory AddCategory(int restaurantId, MenuCategoryDTO menuCategoryDTO)
        {
            var restaurant = _context.Restaurants
                .FirstOrDefault(r => r.RestaurantID == restaurantId);

            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found for the given ID.");

            // Use menuCategoryDTO to create the new MenuCategory
            var newCategory = new MenuCategory
            {
                CategoryName = menuCategoryDTO.CategoryName, // Corrected to use menuCategoryDTO
                CategoryDescription = menuCategoryDTO.Description // Corrected to use menuCategoryDTO
            };

            _context.MenuCategories.Add(newCategory);
            _context.SaveChanges(); // CategoryID will be assigned here

            return newCategory; // Return the created category
        }

        public void UpdateCategory(int restaurantId, int categoryId, EditCategoryDTO editCategoryDTO)
        {
            var restaurant = _context.Restaurants
                .FirstOrDefault(r => r.RestaurantID == restaurantId);

            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found for the given ID.");

            var category = _context.MenuCategories
                .FirstOrDefault(c => c.CategoryID == categoryId);

            if (category == null)
                throw new KeyNotFoundException("Category not found for the given ID.");

            // Update category properties
            category.CategoryName = editCategoryDTO.CategoryName;
            category.CategoryDescription = editCategoryDTO.Description;

            // Save changes to the database
            _context.SaveChanges();
        }


        public void DeleteCategory(int restaurantId, int categoryId)
        {
            var restaurant = _context.Restaurants
                .FirstOrDefault(r => r.RestaurantID == restaurantId);

            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found for the given ID.");

            var category = _context.MenuCategories
                .FirstOrDefault(c => c.CategoryID == categoryId);

            if (category == null)
                throw new KeyNotFoundException("Category not found for the given ID.");

            _context.MenuCategories.Remove(category);
            _context.SaveChanges();
        }

        public List<OrderDTO> GetOrders(int restaurantId)
        {
            var orders = _context.Orders
                .Where(o => o.RestaurantID == restaurantId)
                .Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    TotalPrice = o.TotalPrice,
                    ShippingAddress = o.ShippingAddress,
                    OrderStatus = o.OrderStatus,
                    OrderTime = o.OrderTime,
                    DeliveryTime = o.DeliveryTime,
                    UserID = o.UserID,
                    RestaurantID = o.RestaurantID,
                    DeliveryAgentID = o.DeliveryAgentID,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        OrderItemID = oi.OrderItemID,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        Discount = oi.Discount,
                        ItemID = oi.ItemID,
                        ItemName = oi.MenuItems.ItemName,
                        ItemPrice = oi.MenuItems.ItemPrice
                    }).ToList()
                })
                .ToList();

            return orders;
        }

        public void UpdateOrderStatus(int userId, int orderId, string status)
        {

            var order = _context.Orders
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
                throw new KeyNotFoundException("Order not found for the given ID.");

            order.OrderStatus = status;

            _context.SaveChanges();
        }

        public OrderDTO GetOrderDetails(int userId, int orderId)
        {
            var order = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItems).FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
                throw new KeyNotFoundException("Restaurant not found for the given user.");



            return new OrderDTO
            {
                OrderID = order.OrderID,
                TotalPrice = order.TotalPrice,
                ShippingAddress = order.ShippingAddress,
                OrderStatus = order.OrderStatus,
                OrderTime = order.OrderTime,
                DeliveryTime = order.DeliveryTime,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                DeliveryAgentID = order.DeliveryAgentID,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    OrderItemID = oi.OrderItemID,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    Discount = oi.Discount,
                    ItemID = oi.ItemID,
                    ItemName = oi.MenuItems.ItemName,
                    ItemPrice = oi.MenuItems.ItemPrice
                }).ToList()
            };
        }



        public void MarkMenuItemAsOutOfStock(int userId, int itemId)
        {
            // Fetch the user and their associated RestaurantID
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Fetch the MenuItem and check if it belongs to the user's RestaurantID
            var menuItem = _context.Menus.FirstOrDefault(mi => mi.ItemID == itemId);
            if (menuItem == null)
            {
                throw new InvalidOperationException("Menu item not found.");
            }

            var restaurant = _context.Restaurants.FirstOrDefault(r => r.RestaurantID == user.RestaurantID);
            if (restaurant == null || !restaurant.MenuItems.Contains(menuItem))
            {
                throw new InvalidOperationException("Menu item does not belong to the user's restaurant.");
            }

            // Mark the menu item as out of stock
            menuItem.AvailabilityStatus = "Out of Stock";
            _context.Menus.Update(menuItem);
            _context.SaveChanges();
        }
    }
}


