using Domain.Authentication.Services;
using System.Security.Claims;
using Core.Exceptions;
using Domain.Users.Enums;

namespace API.Services;


    public class HttpUserContext : IUserContext
    {
        private readonly HttpContext _httpContext;

        public HttpUserContext(IHttpContextAccessor _contextAccessor)
        {
            _httpContext = _contextAccessor.HttpContext ?? throw new ArgumentException("No HTTP Context");
        }

        public HttpUserContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public int UserId => Int32.Parse(ReadClaim(ClaimTypes.NameIdentifier));
        public string UserEmail => ReadClaim(ClaimTypes.NameIdentifier);
        public UserRole? Role => Enum.TryParse<UserRole>(ReadClaim(ClaimTypes.Role), out var role) ? role : null; 
        public bool IsAdmin => Role == UserRole.Admin;
		
        public bool IsAuthenticated => _httpContext.User.Identity?.IsAuthenticated ?? false;

        private string ReadClaim(string claimType)
        {
            return _httpContext!.User.Claims.FirstOrDefault(x => x.Type == claimType)?.Value
                   ?? throw new DomainException("Unauthorized", (int)CommonErrorCode.Unauthorized);
        }
    }