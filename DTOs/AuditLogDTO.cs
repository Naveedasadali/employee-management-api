namespace EmployeeManagementSystem.DTOs
{
    public class AuditLogDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Action { get; set; }


        public DateTime Timestamp { get; set; }
    }
}
