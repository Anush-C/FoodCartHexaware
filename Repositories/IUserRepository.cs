using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(int id);
        Task<Users> CreateUserAsync(CreateUserDTO createUserDto);
        Task UpdateUserAsync(UpdateUserDTO updateUserDto);
        Task DeleteUserAsync(int id);
    }
}
