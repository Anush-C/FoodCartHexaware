using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItems> Menus { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }   
        public DbSet<Cart> Carts { get; set; }
        public DbSet<DeliveryAgent> DeliveryAgents { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notification { get; set; }

        public DbSet<NetBankingPayment> NetBankingPayment { get; set; }
        public DbSet<UpiPayment> UpiPayment { get; set; }
        public DbSet<CardPayment> CardPayment { get; set; }

        public DbSet<PasswordResetToken> PasswordResetToken { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Seed Menu Categories with descriptions
            modelBuilder.Entity<MenuCategory>().HasData(
                new MenuCategory { CategoryID = 1, CategoryName = "Breakfast", CategoryDescription = "Start your day with a hearty breakfast." },
                new MenuCategory { CategoryID = 2, CategoryName = "Lunch", CategoryDescription = "Enjoy a satisfying lunch." },
                new MenuCategory { CategoryID = 3, CategoryName = "Dinner", CategoryDescription = "End your day with a delicious dinner." },
                new MenuCategory { CategoryID = 4, CategoryName = "Burgers", CategoryDescription = "Juicy burgers with various toppings." },
                new MenuCategory { CategoryID = 5, CategoryName = "Pizza", CategoryDescription = "Pizza with a variety of toppings." },
                new MenuCategory { CategoryID = 6, CategoryName = "Italian", CategoryDescription = "Traditional Italian dishes." },
                new MenuCategory { CategoryID = 7, CategoryName = "Arabian", CategoryDescription = "Authentic Arabian cuisine." },
                new MenuCategory { CategoryID = 8, CategoryName = "Appetizers", CategoryDescription = "Small dishes to start your meal." },
                new MenuCategory { CategoryID = 9, CategoryName = "Main Dishes", CategoryDescription = "Hearty and fulfilling main courses." },
                new MenuCategory { CategoryID = 10, CategoryName = "Beverages", CategoryDescription = "Drinks to complement your meal." },
                new MenuCategory { CategoryID = 11, CategoryName = "Desserts", CategoryDescription = "Sweet treats to end your meal." }
            );
            modelBuilder.Entity<Restaurant>().HasData(
    new Restaurant
    {
        RestaurantID = 1,
        RestaurantName = "The Breakfast Club",
        RestaurantDescription = "Serving the best breakfast in town.",
        RestaurantPhone = "1234567890",
        RestaurantEmail = "contact@breakfastclub.com",
        RestaurantAddress = "123 Breakfast Ave",
        OpeningHours = new TimeSpan(6, 0, 0),  // 6 AM
        ClosingHours = new TimeSpan(11, 0, 0)   // 11 AM
    },
    new Restaurant
    {
        RestaurantID = 2,
        RestaurantName = "Lunch Haven",
        RestaurantDescription = "Delicious lunch options with a cozy ambiance.",
        RestaurantPhone = "2345678901",
        RestaurantEmail = "info@lunchhaven.com",
        RestaurantAddress = "456 Lunch Lane",
        OpeningHours = new TimeSpan(11, 0, 0),  // 11 AM
        ClosingHours = new TimeSpan(15, 0, 0)   // 3 PM
    },
    new Restaurant
    {
        RestaurantID = 3,
        RestaurantName = "Dinner Delight",
        RestaurantDescription = "Fine dining with exquisite dinner choices.",
        RestaurantPhone = "3456789012",
        RestaurantEmail = "hello@dinnerdelight.com",
        RestaurantAddress = "789 Dinner Dr",
        OpeningHours = new TimeSpan(17, 0, 0),  // 5 PM
        ClosingHours = new TimeSpan(22, 0, 0)   // 10 PM
    },
    new Restaurant
    {
        RestaurantID = 4,
        RestaurantName = "Burger Bonanza",
        RestaurantDescription = "The ultimate burger experience.",
        RestaurantPhone = "4567890123",
        RestaurantEmail = "contact@burgerbonanza.com",
        RestaurantAddress = "101 Burger Blvd",
        OpeningHours = new TimeSpan(10, 0, 0),  // 10 AM
        ClosingHours = new TimeSpan(23, 0, 0)   // 11 PM
    },
    new Restaurant
    {
        RestaurantID = 5,
        RestaurantName = "Pizza Palace",
        RestaurantDescription = "Amazing pizzas with fresh ingredients.",
        RestaurantPhone = "5678901234",
        RestaurantEmail = "info@pizzapalace.com",
        RestaurantAddress = "202 Pizza Place",
        OpeningHours = new TimeSpan(10, 0, 0),  // 10 AM
        ClosingHours = new TimeSpan(23, 0, 0)   // 11 PM
    },
    new Restaurant
    {
        RestaurantID = 6,
        RestaurantName = "Italian Bistro",
        RestaurantDescription = "Authentic Italian cuisine.",
        RestaurantPhone = "6789012345",
        RestaurantEmail = "contact@italianbistro.com",
        RestaurantAddress = "303 Italian St",
        OpeningHours = new TimeSpan(11, 0, 0),  // 11 AM
        ClosingHours = new TimeSpan(22, 0, 0)   // 10 PM
    },
    new Restaurant
    {
        RestaurantID = 7,
        RestaurantName = "Arabian Nights",
        RestaurantDescription = "Experience the taste of Arabian food.",
        RestaurantPhone = "7890123456",
        RestaurantEmail = "info@arabianights.com",
        RestaurantAddress = "404 Arabian Ave",
        OpeningHours = new TimeSpan(12, 0, 0),  // 12 PM
        ClosingHours = new TimeSpan(23, 0, 0)   // 11 PM
    },
    new Restaurant
    {
        RestaurantID = 8,
        RestaurantName = "Appetizer Alley",
        RestaurantDescription = "A variety of appetizers to start your meal.",
        RestaurantPhone = "8901234567",
        RestaurantEmail = "contact@appetizeralley.com",
        RestaurantAddress = "505 Appetizer Rd",
        OpeningHours = new TimeSpan(11, 0, 0),  // 11 AM
        ClosingHours = new TimeSpan(22, 0, 0)   // 10 PM
    },
    new Restaurant
    {
        RestaurantID = 9,
        RestaurantName = "Main Dish Masters",
        RestaurantDescription = "Hearty main dishes for all tastes.",
        RestaurantPhone = "9012345678",
        RestaurantEmail = "info@maindishmasters.com",
        RestaurantAddress = "606 Main St",
        OpeningHours = new TimeSpan(11, 0, 0),  // 11 AM
        ClosingHours = new TimeSpan(23, 0, 0)   // 11 PM
    },
    new Restaurant
    {
        RestaurantID = 10,
        RestaurantName = "Dessert Delights",
        RestaurantDescription = "Sweet and decadent desserts.",
        RestaurantPhone = "0123456789",
        RestaurantEmail = "contact@dessertdelights.com",
        RestaurantAddress = "707 Dessert Blvd",
        OpeningHours = new TimeSpan(12, 0, 0),  // 12 PM
        ClosingHours = new TimeSpan(22, 0, 0)   // 10 PM
    }


);
            modelBuilder.Entity<MenuItems>().HasData(
    // Breakfast
    new MenuItems
    {
        ItemID = 1,
        ItemName = "Pancakes",
        ItemDescription = "Fluffy pancakes served with syrup and butter.",
        ItemPrice = 5.99M,
        Ingredients = "Flour, Eggs, Milk, Butter, Syrup",
        CuisineType = "American",
        TasteInfo = "Sweet",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegetarian",
        ImageURL = "images/pancakes.jpg",
        CategoryID = 1
    },
    new MenuItems
    {
        ItemID = 2,
        ItemName = "Omelette",
        ItemDescription = "Classic omelette with cheese and vegetables.",
        ItemPrice = 6.49M,
        Ingredients = "Eggs, Cheese, Bell Peppers, Onions",
        CuisineType = "American",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegetarian",
        ImageURL = "images/omelette.jpg",
        CategoryID = 1
    },

    // Lunch
    new MenuItems
    {
        ItemID = 3,
        ItemName = "Grilled Chicken Salad",
        ItemDescription = "Fresh salad with grilled chicken and vinaigrette.",
        ItemPrice = 8.99M,
        Ingredients = "Chicken, Lettuce, Tomato, Cucumber, Vinaigrette",
        CuisineType = "American",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Low Carb",
        ImageURL = "images/chicken_salad.jpg",
        CategoryID = 2
    },
    new MenuItems
    {
        ItemID = 4,
        ItemName = "Turkey Sandwich",
        ItemDescription = "Turkey sandwich with lettuce, tomato, and mayo.",
        ItemPrice = 7.49M,
        Ingredients = "Turkey, Lettuce, Tomato, Mayo, Bread",
        CuisineType = "American",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "High Protein",
        ImageURL = "images/turkey_sandwich.jpg",
        CategoryID = 2
    },

    // Dinner
    new MenuItems
    {
        ItemID = 5,
        ItemName = "Steak Dinner",
        ItemDescription = "Grilled steak served with mashed potatoes and vegetables.",
        ItemPrice = 15.99M,
        Ingredients = "Steak, Potatoes, Vegetables",
        CuisineType = "American",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "High Protein",
        ImageURL = "images/steak_dinner.jpg",
        CategoryID = 3
    },
    new MenuItems
    {
        ItemID = 6,
        ItemName = "Lasagna",
        ItemDescription = "Layered pasta with meat sauce and cheese.",
        ItemPrice = 14.49M,
        Ingredients = "Pasta, Beef, Cheese, Tomato Sauce",
        CuisineType = "Italian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/lasagna.jpg",
        CategoryID = 3
    },

    // Burgers
    new MenuItems
    {
        ItemID = 7,
        ItemName = "Cheeseburger",
        ItemDescription = "Beef patty with cheese, lettuce, and tomato.",
        ItemPrice = 9.99M,
        Ingredients = "Beef, Cheese, Lettuce, Tomato, Bun",
        CuisineType = "American",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/cheeseburger.jpg",
        CategoryID = 4
    },
    new MenuItems
    {
        ItemID = 8,
        ItemName = "Veggie Burger",
        ItemDescription = "Vegetarian burger with grilled vegetables and cheese.",
        ItemPrice = 8.49M,
        Ingredients = "Vegetables, Cheese, Bun",
        CuisineType = "American",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegetarian",
        ImageURL = "images/veggie_burger.jpg",
        CategoryID = 4
    },

    // Pizza
    new MenuItems
    {
        ItemID = 9,
        ItemName = "Margherita Pizza",
        ItemDescription = "Classic pizza with tomato sauce and mozzarella cheese.",
        ItemPrice = 11.99M,
        Ingredients = "Tomato Sauce, Mozzarella, Basil",
        CuisineType = "Italian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/margherita_pizza.jpg",
        CategoryID = 5
    },
    new MenuItems
    {
        ItemID = 10,
        ItemName = "Pepperoni Pizza",
        ItemDescription = "Pizza with pepperoni and cheese.",
        ItemPrice = 12.99M,
        Ingredients = "Tomato Sauce, Mozzarella, Pepperoni",
        CuisineType = "Italian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/pepperoni_pizza.jpg",
        CategoryID = 5
    },

    // Italian
    new MenuItems
    {
        ItemID = 11,
        ItemName = "Spaghetti Carbonara",
        ItemDescription = "Spaghetti with creamy carbonara sauce and pancetta.",
        ItemPrice = 13.99M,
        Ingredients = "Spaghetti, Pancetta, Cream, Parmesan",
        CuisineType = "Italian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/carbonara.jpg",
        CategoryID = 6
    },
    new MenuItems
    {
        ItemID = 12,
        ItemName = "Chicken Alfredo",
        ItemDescription = "Fettuccine pasta with creamy Alfredo sauce and chicken.",
        ItemPrice = 14.49M,
        Ingredients = "Fettuccine, Chicken, Alfredo Sauce",
        CuisineType = "Italian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/chicken_alfredo.jpg",
        CategoryID = 6
    },

    // Arabian
    new MenuItems
    {
        ItemID = 13,
        ItemName = "Chicken Shawarma",
        ItemDescription = "Spiced chicken wrapped in pita with vegetables.",
        ItemPrice = 10.99M,
        Ingredients = "Chicken, Pita, Vegetables, Shawarma Spices",
        CuisineType = "Arabian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/chicken_shawarma.jpg",
        CategoryID = 7
    },
    new MenuItems
    {
        ItemID = 14,
        ItemName = "Falafel",
        ItemDescription = "Deep-fried chickpea balls with tahini sauce.",
        ItemPrice = 8.49M,
        Ingredients = "Chickpeas, Tahini, Spices",
        CuisineType = "Arabian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegan",
        ImageURL = "images/falafel.jpg",
        CategoryID = 7
    },

    // Appetizers
    new MenuItems
    {
        ItemID = 15,
        ItemName = "Spring Rolls",
        ItemDescription = "Crispy spring rolls filled with vegetables.",
        ItemPrice = 6.99M,
        Ingredients = "Vegetables, Spring Roll Wrappers",
        CuisineType = "Asian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegetarian",
        ImageURL = "images/spring_rolls.jpg",
        CategoryID = 8
    },
    new MenuItems
    {
        ItemID = 16,
        ItemName = "Garlic Bread",
        ItemDescription = "Toasted bread with garlic and herbs.",
        ItemPrice = 5.49M,
        Ingredients = "Bread, Garlic, Butter, Herbs",
        CuisineType = "Italian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegetarian",
        ImageURL = "images/garlic_bread.jpg",
        CategoryID = 8
    },

    // Main Dishes
    new MenuItems
    {
        ItemID = 17,
        ItemName = "Beef Stroganoff",
        ItemDescription = "Tender beef in a creamy mushroom sauce.",
        ItemPrice = 14.99M,
        Ingredients = "Beef, Mushrooms, Cream, Onions",
        CuisineType = "Russian",
        TasteInfo = "Savory",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/beef_stroganoff.jpg",
        CategoryID = 9
    },
    new MenuItems
    {
        ItemID = 18,
        ItemName = "Chicken Curry",
        ItemDescription = "Spicy chicken curry served with rice.",
        ItemPrice = 12.49M,
        Ingredients = "Chicken, Rice, Curry Spices",
        CuisineType = "Indian",
        TasteInfo = "Spicy",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/chicken_curry.jpg",
        CategoryID = 9
    },

    // Beverages
    new MenuItems
    {
        ItemID = 19,
        ItemName = "Lemonade",
        ItemDescription = "Freshly squeezed lemonade.",
        ItemPrice = 3.99M,
        Ingredients = "Lemon, Sugar, Water",
        CuisineType = "American",
        TasteInfo = "Sweet",
        AvailabilityStatus = "Available",
        DietaryInfo = "Vegan",
        ImageURL = "images/lemonade.jpg",
        CategoryID = 10
    },
    new MenuItems
    {
        ItemID = 20,
        ItemName = "Iced Coffee",
        ItemDescription = "Chilled coffee with a touch of cream.",
        ItemPrice = 4.49M,
        Ingredients = "Coffee, Cream, Sugar",
        CuisineType = "American",
        TasteInfo = "Bitter",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/iced_coffee.jpg",
        CategoryID = 10
    },

    // Desserts
    new MenuItems
    {
        ItemID = 21,
        ItemName = "Chocolate Cake",
        ItemDescription = "Rich chocolate cake with a creamy frosting.",
        ItemPrice = 6.49M,
        Ingredients = "Chocolate, Flour, Sugar, Eggs",
        CuisineType = "American",
        TasteInfo = "Sweet",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/chocolate_cake.jpg",
        CategoryID = 11
    },
    new MenuItems
    {
        ItemID = 22,
        ItemName = "Cheesecake",
        ItemDescription = "Creamy cheesecake with a graham cracker crust.",
        ItemPrice = 7.49M,
        Ingredients = "Cream Cheese, Sugar, Graham Crackers",
        CuisineType = "American",
        TasteInfo = "Sweet",
        AvailabilityStatus = "Available",
        DietaryInfo = "Contains Dairy",
        ImageURL = "images/cheesecake.jpg",
        CategoryID = 11
    }
        );
            modelBuilder.Entity<DeliveryAgent>().HasData(
    new DeliveryAgent
    {
        DeliveryAgentID = 1,
        Name = "John Doe",
        PhoneNumber = "123-456-7890",
        Email = "john.doe@example.com",
        Address = "123 Elm Street, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 2,
        Name = "Jane Smith",
        PhoneNumber = "234-567-8901",
        Email = "jane.smith@example.com",
        Address = "456 Oak Avenue, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 3,
        Name = "Emily Johnson",
        PhoneNumber = "345-678-9012",
        Email = "emily.johnson@example.com",
        Address = "789 Pine Road, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 4,
        Name = "Michael Brown",
        PhoneNumber = "456-789-0123",
        Email = "michael.brown@example.com",
        Address = "101 Maple Drive, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 5,
        Name = "Sarah Davis",
        PhoneNumber = "567-890-1234",
        Email = "sarah.davis@example.com",
        Address = "202 Birch Lane, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 6,
        Name = "David Wilson",
        PhoneNumber = "678-901-2345",
        Email = "david.wilson@example.com",
        Address = "303 Cedar Street, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 7,
        Name = "Laura Miller",
        PhoneNumber = "789-012-3456",
        Email = "laura.miller@example.com",
        Address = "404 Spruce Avenue, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 8,
        Name = "Daniel Taylor",
        PhoneNumber = "890-123-4567",
        Email = "daniel.taylor@example.com",
        Address = "505 Fir Street, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 9,
        Name = "Olivia Anderson",
        PhoneNumber = "901-234-5678",
        Email = "olivia.anderson@example.com",
        Address = "606 Redwood Road, Springfield",
        IsAvailable = true,
    },
    new DeliveryAgent
    {
        DeliveryAgentID = 10,
        Name = "James Martinez",
        PhoneNumber = "012-345-6789",
        Email = "james.martinez@example.com",
        Address = "707 Sequoia Boulevard, Springfield",
        IsAvailable = true,
    }
);

         
         




        }
    }
}