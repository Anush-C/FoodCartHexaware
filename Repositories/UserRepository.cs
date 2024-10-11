using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users> CreateUserAsync(CreateUserDTO createUserDto)
        {
            var errorMessages = new List<string>();

            // Validate required fields
            if (string.IsNullOrEmpty(createUserDto.UserName))
                errorMessages.Add("Username is required.");
            if (string.IsNullOrEmpty(createUserDto.Password) || createUserDto.Password.Length < 8)
                errorMessages.Add("Password must contain at least 8 characters.");
            if (string.IsNullOrEmpty(createUserDto.Email))
                errorMessages.Add("Email is required.");
            if (string.IsNullOrEmpty(createUserDto.PhoneNumber))
                errorMessages.Add("Phone Number is required.");

            // Role-specific validation
            switch (createUserDto.Role)
            {
                case "HotelOwner":
                    if (!createUserDto.RestaurantID.HasValue)
                        errorMessages.Add("Restaurant ID is required for Hotel Owner.");
                    else if (!await _context.Restaurants.AnyAsync(r => r.RestaurantID == createUserDto.RestaurantID))
                        errorMessages.Add("Invalid Restaurant ID.");
                    break;

                case "Customer":
                case "Admin":
                    if (createUserDto.RestaurantID.HasValue)
                        errorMessages.Add("Restaurant ID should not be provided for Customer or Admin.");
                    break;

                default:
                    errorMessages.Add("Invalid role specified.");
                    break;
            }

            // If there are any validation errors, throw an exception
            if (errorMessages.Any())
            {
                throw new InvalidOperationException(string.Join(", ", errorMessages));
            }

            // Create user entity
            var user = new Users
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                AlternativePhoneNumber = createUserDto.AlternativePhoneNumber,
                Role = createUserDto.Role,
                RestaurantID = createUserDto.RestaurantID, // Optional
                Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password) // Hash the password
            };

            // Add user to the context
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while saving the user.", ex);
            }

            return user; // Return the created user
        }


        public async Task UpdateUserAsync(UpdateUserDTO updateUserDto)
        {
            // Retrieve the existing user from the database
            var existingUser = await _context.Users.FindAsync(updateUserDto.UserID);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var errorMessages = new List<string>();

            // Validate required fields
            if (string.IsNullOrEmpty(updateUserDto.UserName))
                errorMessages.Add("Username is required.");
            if (string.IsNullOrEmpty(updateUserDto.Email))
                errorMessages.Add("Email is required.");
            if (string.IsNullOrEmpty(updateUserDto.PhoneNumber))
                errorMessages.Add("Phone Number is required.");

            // Role-specific validation
            switch (updateUserDto.Role)
            {
                case "HotelOwner":
                    if (!updateUserDto.RestaurantID.HasValue)
                        errorMessages.Add("Restaurant ID is required for Hotel Owner.");
                    else if (!await _context.Restaurants.AnyAsync(r => r.RestaurantID == updateUserDto.RestaurantID))
                        errorMessages.Add("Invalid Restaurant ID.");
                    break;

                case "Customer":
                case "Admin":
                    if (updateUserDto.RestaurantID.HasValue)
                        errorMessages.Add("Restaurant ID should not be provided for Customer or Admin.");
                    break;

                default:
                    errorMessages.Add("Invalid role specified.");
                    break;
            }

            // Return validation errors if any
            if (errorMessages.Any())
            {
                throw new ArgumentException(string.Join(", ", errorMessages));
            }

            // Update the user entity
            existingUser.UserName = updateUserDto.UserName;
            existingUser.Email = updateUserDto.Email;
            existingUser.PhoneNumber = updateUserDto.PhoneNumber;
            existingUser.AlternativePhoneNumber = updateUserDto.AlternativePhoneNumber;
            existingUser.Role = updateUserDto.Role;

            // Only update restaurantID if the role is Hotel Owner
            if (updateUserDto.Role == "HotelOwner")
            {
                existingUser.RestaurantID = updateUserDto.RestaurantID; // Assign the restaurant ID
            }
            else
            {
                existingUser.RestaurantID = null; // Clear it for other roles
            }

            // Save changes to the database
            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
