using JwtAuthApi.Models;
using JwtAuthApi.Core.Interfaces;

namespace JwtAuthApi.Core.Specifications.UserSpecifications
{
    /// <summary>
    /// Specification for users by email
    /// </summary>
    public class UserByEmailSpecification : BaseSpecification<User>
    {
        public UserByEmailSpecification(string email) 
            : base(u => u.Email.ToLower() == email.ToLower())
        {
        }
    }

    /// <summary>
    /// Specification for active users
    /// </summary>
    public class ActiveUsersSpecification : BaseSpecification<User>
    {
        public ActiveUsersSpecification() 
            : base(u => u.IsActive)
        {
            ApplyOrderBy(u => u.CreatedAt);
        }
    }

    /// <summary>
    /// Specification for users by role
    /// </summary>
    public class UsersByRoleSpecification : BaseSpecification<User>
    {
        public UsersByRoleSpecification(string role) 
            : base(u => u.Role == role && u.IsActive)
        {
            ApplyOrderBy(u => u.LastName);
        }
    }

    /// <summary>
    /// Specification for paginated users
    /// </summary>
    public class PaginatedUsersSpecification : BaseSpecification<User>
    {
        public PaginatedUsersSpecification(int pageIndex, int pageSize, string? role = null)
            : base(u => u.IsActive && (role == null || u.Role == role))
        {
            ApplyPaging(pageIndex * pageSize, pageSize);
            ApplyOrderBy(u => u.CreatedAt);
        }
    }
}
