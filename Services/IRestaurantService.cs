using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.DTOs;
using FoodCart_Hexaware.Models;
using System.Collections.Generic;

namespace FoodCart_Hexaware.Services
{
    public interface IRestaurantService
    {
        // Fetch the dashboard data 
        DashBoardDTO GetDashboardData(int userId);

        Task<Restaurant> GetRestaurantByIdAsync(int restaurantId);

        // Add a new menu item 
        void AddMenuItem(int userId, MenuItemDTO menuItemDTO);

        // Get all menu items 
        IEnumerable<MenuItemDTO> GetMenuItems(int restaurantId);

        // Update an existing menu item
        void UpdateMenuItem(int userId, int itemId, MenuItemDTO menuItemDTO);

        // Delete a menu item
        void DeleteMenuItem(int userId, int itemId);

        // Fetch restaurant information for a user
        Restaurant GetRestaurantByUserId(int userId);

        // Fetch all categories 
        List<CategoryDTO> GetCategories(int restaurantId);

        // Add a new category
        MenuCategory AddCategory(int restaurantId, MenuCategoryDTO menuCategoryDTO);

        // Update an existing category
        void UpdateCategory(int restaurantId, int categoryId, EditCategoryDTO editCategoryDTO);

        // Delete a category by its ID
        void DeleteCategory(int restaurantId, int categoryId);

        // Fetch all orders for a restaurant
        List<OrderDTO> GetOrders(int restaurantId);

        // Update the status of an order
        void UpdateOrderStatus(int userId, int orderId, string status);

        // Get details of a specific order
        OrderDTO GetOrderDetails(int userId, int orderId);

        //Mark menu availability as OUT OF STOCK
        void MarkMenuItemAsOutOfStock(int userId, int itemId);
    }
}
