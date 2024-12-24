using pentasharp.Models.Entities;
using pentasharp.ViewModel.Authenticate;
using pentasharp.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace pentasharp.Interfaces
{
    /// <summary>
    /// Defines the contract for authentication-related operations, including user registration, login, and management.
    /// </summary>
    public interface IAuthenticateService
    {
        /// <summary>
        /// Registers a new user based on the provided registration model.
        /// </summary>
        Task<User> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// Authenticates a user using the provided email and password.
        /// </summary>
        Task<User> LoginAsync(string email, string password);

        /// <summary>
        /// Retrieves a list of all registered users.
        /// </summary>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        Task<User> GetUserByIdAsync(int id);

        /// <summary>
        /// Initiates the Google login process.
        /// </summary>
        IActionResult InitiateGoogleLogin(string redirectUrl);

        /// <summary>
        /// Handles the Google login response and processes the user information.
        /// </summary>
        Task<User> HandleGoogleResponseAsync(string returnUrl);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Updates the details of an existing user based on the provided view model.
        /// </summary>
        Task<bool> EditUserAsync(int userId, EditUserViewModel model);

        /// <summary>
        /// Performs a soft delete on a user, marking them as inactive without removing their data from the database.
        /// </summary>
        Task<bool> SoftDeleteUserAsync(int id);

        /// <summary>
        /// Restores a previously soft-deleted user, marking them as active again.
        /// </summary>
        Task<bool> RestoreUserAsync(int id);

        /// <summary>
        /// Retrieves the current authenticated user's details based on their unique identifier.
        /// </summary>
        Task<User> GetCurrentUserAsync(int userId);

        /// <summary>
        /// Sets the current user's session information.
        /// </summary>
        void SetUserSession(User user);

        /// <summary>
        /// Clears the current user's session information.
        /// </summary>
        void ClearUserSession();

        /// <summary>
        /// Retrieves the unique identifier of the current authenticated user from the session.
        /// </summary>
        int? GetCurrentUserId();

        /// <summary>
        /// Retrieves the unique identifier of the current authenticated user's company from the session.
        /// </summary>
        int? GetCurrentCompanyId();
    }
}