namespace LastManagement.Constants;

public static class AppConstants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Guest = "Guest";
    }

    public static class Status
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
        public const string Suspended = "Suspended";
    }

    public static class ErrorMessages
    {
        public const string Unauthorized = "Authentication required";
        public const string Forbidden = "Insufficient permissions";
        public const string NotFound = "Resource not found";
        public const string BadRequest = "Invalid request data";
        public const string Conflict = "Resource already exists";
        public const string InternalError = "An error occurred processing your request";

        // Auth specific
        public const string InvalidCredentials = "Invalid username or password";
        public const string UserExists = "Username already exists";
        public const string AccountInactive = "Account is inactive";

        // Business rules
        public const string InsufficientStock = "Insufficient stock available";
        public const string InvalidQuantity = "Quantity must be greater than zero";
        public const string CannotDeleteHasRelations = "Cannot delete: resource has related records";
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;
    }
}