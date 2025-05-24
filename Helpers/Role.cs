namespace EmployeeManagementSystem.Helpers
{
    /// <summary>
    /// Static class containing predefined roles used for authorization.
    /// </summary>
    public static class Role
    {
        /// <summary>
        /// Administrator role: Full access to all system features.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Manager role: Can manage employees, leave requests, and reviews.
        /// </summary>
        public const string Manager = "Manager";

        /// <summary>
        /// Employee role: Can view and manage their own profile and leave.
        /// </summary>
        public const string Employee = "Employee";

        // You can add more roles as needed, e.g.,
        // public const string HR = "HR";
        // public const string IT = "IT";
    }
}
