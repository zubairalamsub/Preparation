# .NET Web API Security Complete Guide
## Comprehensive Security Best Practices

---

## Table of Contents
1. [Authentication Fundamentals](#1-authentication-fundamentals)
2. [JWT Authentication](#2-jwt-authentication)
3. [OAuth 2.0 & OpenID Connect](#3-oauth-20--openid-connect)
4. [API Key Authentication](#4-api-key-authentication)
5. [Authorization](#5-authorization)
6. [CORS Configuration](#6-cors-configuration)
7. [Input Validation](#7-input-validation)
8. [Data Protection](#8-data-protection)
9. [HTTPS & TLS](#9-https--tls)
10. [Rate Limiting](#10-rate-limiting)
11. [SQL Injection Prevention](#11-sql-injection-prevention)
12. [XSS Prevention](#12-xss-prevention)
13. [CSRF Protection](#13-csrf-protection)
14. [Security Headers](#14-security-headers)
15. [Secrets Management](#15-secrets-management)
16. [API Versioning](#16-api-versioning)
17. [Logging & Monitoring](#17-logging--monitoring)
18. [Testing Security](#18-testing-security)

---

## 1. Authentication Fundamentals

### Authentication vs Authorization

```
Authentication: Who are you?
- Verifies identity
- Login process
- Credentials validation

Authorization: What can you do?
- Verifies permissions
- Access control
- Role/Claim based
```

### Common Authentication Schemes

```csharp
// Program.cs (.NET 6+)
var builder = WebApplication.CreateBuilder(args);

// 1. JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* config */ });

// 2. Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { /* config */ });

// 3. Multiple schemes
builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options => { /* config */ })
    .AddCookie("Cookies", options => { /* config */ });

var app = builder.Build();

// Middleware order is important!
app.UseAuthentication(); // First
app.UseAuthorization();  // Second
app.MapControllers();
```

---

## 2. JWT Authentication

### Complete JWT Implementation

```csharp
// appsettings.json
{
  "Jwt": {
    "Secret": "YourSuperSecretKeyThatIsAtLeast32CharactersLong",
    "Issuer": "https://yourdomain.com",
    "Audience": "https://yourdomain.com",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  }
}

// Models/JwtSettings.cs
public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}

// Models/LoginRequest.cs
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}

// Models/AuthResponse.cs
public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; }
}

// Services/ITokenService.cs
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Task<bool> ValidateRefreshToken(string token);
}

// Services/TokenService.cs
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("tenant_id", user.TenantId.ToString()),
            new Claim("full_name", $"{user.FirstName} {user.LastName}")
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false // Don't validate expiration
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public async Task<bool> ValidateRefreshToken(string token)
    {
        // Implement validation logic
        // Check if token exists in database and not expired
        return await Task.FromResult(true);
    }
}

// Controllers/AuthController.cs
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepo;

    public AuthController(
        IUserService userService,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepo)
    {
        _userService = userService;
        _tokenService = tokenService;
        _refreshTokenRepo = refreshTokenRepo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        // Validate credentials
        var user = await _userService.ValidateCredentials(
            request.Email,
            request.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Save refresh token
        await _refreshTokenRepo.SaveRefreshToken(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        });

        return Ok(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            }
        });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> RefreshToken(
        RefreshTokenRequest request)
    {
        // Validate access token (even if expired)
        var principal = _tokenService.GetPrincipalFromExpiredToken(
            request.AccessToken);

        if (principal == null)
            return BadRequest(new { message = "Invalid access token" });

        // Get user ID from token
        var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
            return BadRequest(new { message = "Invalid token claims" });

        var userId = int.Parse(userIdClaim.Value);

        // Validate refresh token
        var storedToken = await _refreshTokenRepo
            .GetRefreshToken(userId, request.RefreshToken);

        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        // Get user
        var user = await _userService.GetById(userId);

        // Generate new tokens
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Delete old refresh token
        await _refreshTokenRepo.DeleteRefreshToken(storedToken.Id);

        // Save new refresh token
        await _refreshTokenRepo.SaveRefreshToken(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        });

        return Ok(new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            }
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);

        // Delete all refresh tokens for user
        await _refreshTokenRepo.DeleteAllUserTokens(userId);

        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
        var user = await _userService.GetById(userId);

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        });
    }
}

// Program.cs - JWT Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero // No clock skew tolerance
    };

    // Configure events
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // Custom response for unauthorized requests
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                error = "You are not authorized",
                message = context.ErrorDescription
            });
            return context.Response.WriteAsync(result);
        },
        OnTokenValidated = context =>
        {
            // Additional validation logic
            var userService = context.HttpContext.RequestServices
                .GetRequiredService<IUserService>();

            var userId = int.Parse(context.Principal.FindFirst(
                JwtRegisteredClaimNames.Sub).Value);

            // Check if user still exists and is active
            var user = userService.GetById(userId).Result;
            if (user == null || !user.IsActive)
            {
                context.Fail("User not found or inactive");
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();
```

---

## 3. OAuth 2.0 & OpenID Connect

### Using Azure AD / Microsoft Identity

```csharp
// appsettings.json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "yourdomain.onmicrosoft.com",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id",
    "Scopes": "access_as_user",
    "CallbackPath": "/signin-oidc"
  }
}

// Program.cs
using Microsoft.Identity.Web;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Controller
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(new { userEmail, userName });
    }
}
```

### Using IdentityServer / Duende

```csharp
// API Configuration
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://your-identity-server";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api");
    });
});

// Protected endpoint
[Authorize(Policy = "ApiScope")]
[ApiController]
[Route("api/[controller]")]
public class ProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}
```

---

## 4. API Key Authentication

### Custom API Key Authentication

```csharp
// Models/ApiKey.cs
public class ApiKey
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public List<string> Scopes { get; set; } = new();
}

// Authentication/ApiKeyAuthenticationHandler.cs
public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private readonly IApiKeyRepository _apiKeyRepository;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IApiKeyRepository apiKeyRepository)
        : base(options, logger, encoder, clock)
    {
        _apiKeyRepository = apiKeyRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if API key header exists
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyValue))
        {
            return AuthenticateResult.Fail("API Key not found");
        }

        // Validate API key
        var apiKey = await _apiKeyRepository.GetByKey(apiKeyValue);

        if (apiKey == null)
        {
            return AuthenticateResult.Fail("Invalid API Key");
        }

        if (!apiKey.IsActive)
        {
            return AuthenticateResult.Fail("API Key is inactive");
        }

        if (apiKey.ExpiresAt.HasValue && apiKey.ExpiresAt < DateTime.UtcNow)
        {
            return AuthenticateResult.Fail("API Key has expired");
        }

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, apiKey.UserId.ToString()),
            new Claim(ClaimTypes.Name, apiKey.Name),
            new Claim("api_key_id", apiKey.Id.ToString())
        };

        // Add scope claims
        foreach (var scope in apiKey.Scopes)
        {
            claims.Add(new Claim("scope", scope));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

// Program.cs
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        "ApiKey", null);

builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();

// Controller
[Authorize(AuthenticationSchemes = "ApiKey")]
[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var apiKeyId = User.FindFirst("api_key_id")?.Value;
        return Ok(new { message = "Authenticated with API Key", apiKeyId });
    }
}

// API Key Management Controller
[Authorize] // Requires user authentication
[ApiController]
[Route("api/[controller]")]
public class ApiKeysController : ControllerBase
{
    private readonly IApiKeyRepository _apiKeyRepo;

    [HttpPost]
    public async Task<ActionResult<ApiKeyResponse>> CreateApiKey(
        CreateApiKeyRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        // Generate secure API key
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        var apiKey = new ApiKey
        {
            Key = key,
            Name = request.Name,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = request.ExpiresInDays.HasValue
                ? DateTime.UtcNow.AddDays(request.ExpiresInDays.Value)
                : null,
            IsActive = true,
            Scopes = request.Scopes ?? new List<string>()
        };

        await _apiKeyRepo.Create(apiKey);

        return Ok(new ApiKeyResponse
        {
            Key = key,
            Name = apiKey.Name,
            CreatedAt = apiKey.CreatedAt,
            ExpiresAt = apiKey.ExpiresAt
        });
    }

    [HttpGet]
    public async Task<ActionResult<List<ApiKeyDto>>> GetMyApiKeys()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var apiKeys = await _apiKeyRepo.GetByUserId(userId);

        return Ok(apiKeys.Select(k => new ApiKeyDto
        {
            Id = k.Id,
            Name = k.Name,
            CreatedAt = k.CreatedAt,
            ExpiresAt = k.ExpiresAt,
            IsActive = k.IsActive,
            // Don't return actual key!
            KeyPreview = $"{k.Key.Substring(0, 8)}..."
        }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RevokeApiKey(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var apiKey = await _apiKeyRepo.GetById(id);

        if (apiKey == null || apiKey.UserId != userId)
            return NotFound();

        apiKey.IsActive = false;
        await _apiKeyRepo.Update(apiKey);

        return NoContent();
    }
}
```

---

## 5. Authorization

### Role-Based Authorization

```csharp
// Add roles to user claims
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Role, "Admin"),
    new Claim(ClaimTypes.Role, "Manager") // User can have multiple roles
};

// Controller with role authorization
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Admin only content");
    }

    [Authorize(Roles = "Admin,Manager")] // Either Admin OR Manager
    [HttpPost]
    public IActionResult Post()
    {
        return Ok("Admin or Manager can access");
    }
}
```

### Claims-Based Authorization

```csharp
// Add claims
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim("permission", "orders.read"),
    new Claim("permission", "orders.write"),
    new Claim("department", "sales")
};

// Program.cs - Define policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanReadOrders", policy =>
        policy.RequireClaim("permission", "orders.read"));

    options.AddPolicy("CanWriteOrders", policy =>
        policy.RequireClaim("permission", "orders.write"));

    options.AddPolicy("SalesDepartment", policy =>
        policy.RequireClaim("department", "sales"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("CanManageUsers", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin") ||
            context.User.HasClaim(c => c.Type == "permission" && c.Value == "users.manage")));
});

// Controller
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [Authorize(Policy = "CanReadOrders")]
    [HttpGet]
    public IActionResult GetOrders()
    {
        return Ok("Orders list");
    }

    [Authorize(Policy = "CanWriteOrders")]
    [HttpPost]
    public IActionResult CreateOrder()
    {
        return Ok("Order created");
    }
}
```

### Resource-Based Authorization

```csharp
// Authorization/DocumentAuthorizationHandler.cs
public class DocumentAuthorizationHandler :
    AuthorizationHandler<SameAuthorRequirement, Document>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SameAuthorRequirement requirement,
        Document resource)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && resource.AuthorId == int.Parse(userId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class SameAuthorRequirement : IAuthorizationRequirement { }

// Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SameAuthor", policy =>
        policy.Requirements.Add(new SameAuthorRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, DocumentAuthorizationHandler>();

// Controller
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IDocumentRepository _documentRepo;

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateDocumentDto dto)
    {
        var document = await _documentRepo.GetById(id);

        if (document == null)
            return NotFound();

        // Resource-based authorization
        var authResult = await _authorizationService
            .AuthorizeAsync(User, document, "SameAuthor");

        if (!authResult.Succeeded)
            return Forbid();

        // Update document
        document.Title = dto.Title;
        await _documentRepo.Update(document);

        return Ok(document);
    }
}
```

---

## 6. CORS Configuration

### CORS Setup

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    // Development - Allow specific origin
    options.AddPolicy("Development", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    // Production - Strict policy
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com", "https://app.yourdomain.com")
              .WithHeaders("Content-Type", "Authorization")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });

    // Public API - Allow all origins
    options.AddPolicy("PublicApi", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .WithMethods("GET");
        // Note: Can't use AllowCredentials with AllowAnyOrigin
    });
});

var app = builder.Build();

// Use CORS
app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Production");

// Or apply per controller
[EnableCors("PublicApi")]
[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    // ...
}
```

---

## 7. Input Validation

### Model Validation

```csharp
// DTOs with validation attributes
public class CreateUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, digit, and special character")]
    public string Password { get; set; }

    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    public string Website { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [CreditCard]
    public string CreditCard { get; set; }
}

// Custom validation attribute
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is DateTime date && date > DateTime.Now)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Date must be in the future");
    }
}

// Usage
public class CreateEventDto
{
    [Required]
    public string Name { get; set; }

    [FutureDate]
    public DateTime EventDate { get; set; }
}

// Controller
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateUserDto dto)
    {
        // ModelState automatically validated
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Process valid data
        return Ok();
    }
}

// Fluent Validation (alternative)
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]").WithMessage("Must contain uppercase")
            .Matches(@"[a-z]").WithMessage("Must contain lowercase")
            .Matches(@"\d").WithMessage("Must contain digit")
            .Matches(@"[@$!%*?&]").WithMessage("Must contain special character");

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 120);
    }
}

// Register FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
```

---

**[Continue in Part 2 with remaining security topics...]**

## Quick Security Checklist

### Essential Security Measures
- [ ] Use HTTPS everywhere
- [ ] Implement authentication (JWT/OAuth)
- [ ] Implement authorization (roles/claims)
- [ ] Validate all inputs
- [ ] Use parameterized queries (prevent SQL injection)
- [ ] Encode outputs (prevent XSS)
- [ ] Configure CORS properly
- [ ] Add security headers
- [ ] Implement rate limiting
- [ ] Use secrets management (Key Vault)
- [ ] Enable logging and monitoring
- [ ] Keep dependencies updated
- [ ] Regular security audits

### OWASP Top 10 API Security
1. Broken Object Level Authorization
2. Broken Authentication
3. Broken Object Property Level Authorization
4. Unrestricted Resource Consumption
5. Broken Function Level Authorization
6. Unrestricted Access to Sensitive Business Flows
7. Server Side Request Forgery
8. Security Misconfiguration
9. Improper Inventory Management
10. Unsafe Consumption of APIs
