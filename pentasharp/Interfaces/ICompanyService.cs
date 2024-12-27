namespace pentasharp.Interfaces
{
    public interface ICompanyService
    {
        /// <summary>
        /// Retrieves the company ID associated with the currently logged-in user.
        /// </summary>
        /// <returns>The company ID if found; otherwise, throws an exception.</returns>
        Task<int?> GetCompanyForCurrentUserAsync();
    }
}
