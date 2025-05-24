namespace EmployeeManagementSystem.Interfaces
{
    /// <summary>
    /// Defines the contract for logging user actions into the audit log.
    /// </summary>
    public interface IAuditLogService
    {
        /// <summary>
        /// Records an action taken by a user.
        /// </summary>
        /// <param name="userId">ID of the user performing the action.</param>
        /// <param name="action">Description of the action performed.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task LogAsync(int userId, string action);
    }
}
