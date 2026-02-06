# Service Booking Platform API

A production-ready RESTful API for managing service bookings, user authentication, and service management built with .NET 10 and industry-standard best practices.

## Features

### **User Management**
- User registration with comprehensive validation
- JWT-based authentication with access and refresh tokens
- Token refresh and rotation for enhanced security
- Secure logout with token revocation
- Password hashing using ASP.NET Core Identity
- Email uniqueness validation
- Automatic cleanup of expired/revoked tokens
- Interface-based architecture for testability

### **Booking Management**
- Create, read, update, and delete bookings
- Role-based filtering (Customers see only their bookings, Staff/Admin see all)
- View all bookings with user and service details
- Get booking by ID with authorization checks
- Booking status tracking (Pending, Confirmed, Completed, Cancelled)
- Automatic conflict detection (prevents double-booking)

### **Service Management**
- Create, read, update, and delete services
- Service categorization by type
- Service descriptions
- Admin-only service creation/modification

### **Security & Reliability**
- **JWT Bearer Authentication:** Access tokens (30 min) + Refresh tokens (7 days)
- **Token Rotation:** Prevents token reuse attacks
- **Token Revocation:** Immediate invalidation on logout
- **Password Strength Validation:** Enforced complexity rules
- **Role-Based Authorization:** Customer, Staff, Admin roles
- **Global Exception Handling:** Centralized error handling with ProblemDetails
- **Rate Limiting:** 100 requests per minute per user
- **CORS Support:** Configurable cross-origin policies
- **Response Compression:** Optimized bandwidth usage
- **Connection Resilience:** Automatic retry on database failures (max 5 retries, 30s delay)
- **Structured Logging:** Console and debug logging with configurable levels

### **Performance & Monitoring**
- Response compression for reduced bandwidth
- Database connection pooling with retry logic
- Efficient query optimization
- Development-only detailed error messages
- Production-safe error responses

### **API Documentation**
- Interactive API documentation with Scalar
- OpenAPI/Swagger specification
- JWT authentication integration in API explorer
- Comprehensive endpoint descriptions

## Technologies

- **.NET 10** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework with industry-standard patterns
- **Entity Framework Core** - ORM with connection resilience and retry logic
- **SQL Server** - Relational database with optimized queries
- **JWT (JSON Web Tokens)** - Stateless authentication with claims-based authorization
- **Scalar** - Modern, interactive API documentation
- **ASP.NET Core Identity** - Password hashing and user management
- **xUnit** - Comprehensive test framework (92 passing tests)
- **Dependency Injection** - Interface-based, loosely-coupled architecture
- **ProblemDetails** - Standardized error responses (RFC 7807)

## Architecture Highlights

### **Enterprise Patterns**
- **Interface-Based Design:** All services use interfaces (`IUserBookingService`, `IJwtService`, etc.) for better testability and flexibility
- **Dependency Injection:** Constructor injection throughout the application
- **Repository Pattern:** EF Core DbContext acts as unit of work
- **DTO Pattern:** Separation between domain models and API contracts
- **Result Pattern:** Consistent return types with success/failure indicators
- **Global Exception Handling:** Centralized middleware for all unhandled exceptions

### **Security Features**
- **Claims-Based Authorization:** JWT tokens contain `NameId`, `Email`, and `Role` claims
- **Multiple Claim Type Support:** Handles `ClaimTypes.NameIdentifier`, `JwtRegisteredClaimNames.NameId`, and `nameid`
- **Secure Token Generation:** Cryptographically secure random number generator (64 bytes, base64-encoded)
- **Token Lifecycle Management:** Automatic cleanup of expired/revoked tokens
- **HTTPS Enforcement:** Required in production environments
- **SQL Injection Protection:** Parameterized queries via Entity Framework

### **Error Handling**
The API uses a global exception handler middleware that:
- Catches all unhandled exceptions
- Maps exceptions to appropriate HTTP status codes:
  - `UnauthorizedAccessException` → 403 Forbidden
  - `ArgumentException`, `InvalidOperationException` → 400 Bad Request
  - `KeyNotFoundException` → 404 Not Found
  - All others → 500 Internal Server Error
- Returns standardized `ProblemDetails` responses
- Includes stack traces and trace IDs in development mode
- Logs all errors for debugging

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express
- [Visual Studio 2026](https://visualstudio.microsoft.com/) (optional, recommended for development)

## Installation

### 1. Clone the repository
```bash
git clone https://github.com/sav-afg/ServiceBookingPlatform.git
cd ServiceBookingPlatform
```

### 2. Update connection string
Edit `appsettings.json` and update the connection string to match your SQL Server instance:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ServiceBookingPlatform;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Apply database migrations
```bash
dotnet ef database update
```

### 4. Run the application
```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7022`
- HTTP: `http://localhost:5146`
- Scalar Documentation: `https://localhost:7022/scalar/v1`

## API Endpoints

### Authentication

#### Register User
```http
POST /api/UserRegistration/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "Pass123!@",
  "phoneNumber": "07700900123",
  "role": "Customer"
}
```

**Validation Rules:**
- First/Last Name: 2-50 characters
- Email: Valid email format
- Password: 6-10 characters, must include uppercase, lowercase, digit, and special character
- Phone Number: Valid UK format
- Role: Customer, Staff, or Admin

#### Login
```http
POST /api/UserLogIn
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "Pass123!@"
}
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "john.doe@example.com",
  "expiresIn": 1800,
  "refreshToken": "base64-encoded-refresh-token"
}
```

**Note:** Store both tokens securely. Use the access token for API requests and the refresh token to obtain new access tokens when they expire.

#### Check Email Availability
```http
GET /api/UserRegistration/check-email?email=test@example.com
```

#### Validate Credentials
```http
POST /api/UserLogIn/validate
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "Pass123!@"
}
```

#### Refresh Access Token
```http
POST /auth/refresh
Content-Type: application/json

{
  "refreshToken": "your-refresh-token-here"
}
```

**Response:**
```json
{
  "accessToken": "new-jwt-token...",
  "email": "john.doe@example.com",
  "expiresIn": 1800,
  "refreshToken": "new-refresh-token"
}
```

**Use Cases:**
- When your access token expires (after 30 minutes)
- Frontend receives 401 Unauthorized on API calls
- Automatically refresh without requiring user to log in again

**Important:** The refresh endpoint implements token rotation - your old refresh token is automatically revoked and a new one is issued. Always store the new refresh token.

#### Logout
```http
POST /api/UserLogIn/logout
Content-Type: application/json

{
  "refreshToken": "your-refresh-token-here"
}
```

**Response:**
```json
{
  "message": "Logged out successfully"
}
```

**What happens:**
- Revokes your refresh token in the database
- Prevents the token from being used for future refresh operations
- Access tokens remain valid until expiry (stateless nature of JWT)
- Best practice: Clear both tokens from client storage

### Bookings

#### Get All Bookings
```http
GET /api/UserBooking
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "id": 1,
    "scheduledStart": "2024-01-26T10:00:00",
    "scheduledEnd": "2024-01-26T11:00:00",
    "status": "Confirmed",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "serviceName": "Haircut"
  }
]
```

#### Get Booking by ID
```http
GET /api/UserBooking/{id}
Authorization: Bearer {token}
```

#### Create Booking
```http
POST /api/UserBooking
Authorization: Bearer {token}
Content-Type: application/json

{
  "userId": 1,
  "serviceId": 1,
  "scheduledStart": "2024-01-26T10:00:00",
  "scheduledEnd": "2024-01-26T11:00:00",
  "status": "Pending"
}
```

#### Update Booking
```http
PUT /api/UserBooking/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "scheduledStart": "2024-01-26T14:00:00",
  "scheduledEnd": "2024-01-26T15:00:00",
  "status": "Confirmed"
}
```

#### Delete Booking
```http
DELETE /api/UserBooking/{id}
Authorization: Bearer {token}
```

### Services

#### Get All Services
```http
GET /api/UserService
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "id": 1,
    "serviceName": "Haircut",
    "serviceType": "Barbering",
    "serviceDescription": "Professional haircut service"
  }
]
```

#### Get Service by ID
```http
GET /api/UserService/{id}
Authorization: Bearer {token}
```

#### Create Service
```http
POST /api/UserService
Authorization: Bearer {token}
Content-Type: application/json

{
  "serviceName": "Haircut",
  "serviceType": "Barbering",
  "serviceDescription": "Professional haircut service"
}
```

#### Update Service
```http
PATCH /api/UserService/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "serviceDescription": "Updated description"
}
```

#### Delete Service
```http
DELETE /api/UserService/{id}
Authorization: Bearer {token}
```

## Authentication

The API uses JWT (JSON Web Token) for authentication. After logging in, include the token in the `Authorization` header:

```
Authorization: Bearer {your-jwt-token}
```

**Token Configuration:**
- **Issuer:** `https://localhost:7022/`
- **Audience:** `https://localhost:7022/`
- **Expiry:** 30 minutes
- **Algorithm:** HS256

### Using Authentication in Scalar

1. Navigate to `https://localhost:7022/scalar/v1`
2. Click the **Lock Icon** 🔒 or **"Authenticate"** button
3. Select **"Bearer"** authentication
4. Paste your JWT token (without "Bearer" prefix)
5. Click **"Authorize"**
6. All subsequent requests will include the authentication header

## Database Schema

### Users Table
- `Id` (PK)
- `FirstName`
- `LastName`
- `Email` (Unique)
- `PasswordHash`
- `PhoneNumber`
- `Role`

### RefreshTokens Table
- `Id` (PK)
- `UserId` (FK → Users.Id)
- `Token` (Base64-encoded, 64 bytes)
- `CreatedAt`
- `ExpiresAt` (7 days from creation)
- `IsRevoked` (Boolean flag for logout/rotation)

### Services Table
- `Id` (PK)
- `ServiceName`
- `ServiceType`
- `ServiceDescription`

### Bookings Table
- `Id` (PK)
- `UserId` (FK → Users.Id)
- `ServiceId` (FK → Services.Id)
- `ScheduledStart`
- `ScheduledEnd`
- `Status`

**Relationships:**
- One User can have many Bookings (1:N)
- One Service can have many Bookings (1:N)
- One User can have many RefreshTokens (1:N)
- Cascade delete on foreign keys

**Token Lifecycle:**
1. **Login:** User receives access token (30 min) + refresh token (7 days)
2. **Active Use:** Access token used for all API requests via Authorization header
3. **Token Expiry:** After 30 minutes, access token expires
4. **Refresh:** Client sends refresh token to `/auth/refresh` to get new access + refresh tokens
5. **Token Rotation:** Old refresh token is revoked, new one issued (security best practice)
6. **Logout:** Refresh token revoked, preventing further use
7. **Cleanup:** Expired/revoked tokens automatically removed on next login

## Configuration

### JWT Settings (`appsettings.json`)
```json
{
  "JwtConfig": {
    "Issuer": "https://localhost:7022/",
    "Audience": "https://localhost:7022/",
    "Key": "YOUR_SECRET_KEY_HERE_MIN_32_CHARS",
    "TokenValidityMins": 30,
    "RefreshTokenValidityDays": 7
  },
  "AllowedOrigins": [
    "http://localhost:3000",
    "https://localhost:3000"
  ],
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

**Strongly-Typed Configuration:**
The application uses `JwtConfig` class for type-safe configuration access via `IOptions<JwtConfig>`.

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ServiceBookingPlatform;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Connection Resilience:**
- Automatic retry on transient failures (max 5 retries)
- 30-second max retry delay with exponential backoff
- 30-second command timeout
- Sensitive data logging enabled only in Development mode

## Testing

The project includes **92 comprehensive tests** with 100% pass rate:
- **77 Unit Tests** - Fast, isolated tests for individual components
- **15 Integration Tests** - End-to-end tests using WebApplicationFactory

### Test Coverage

#### Unit Tests (77 tests)
- **UserBookingServiceTests** (31 tests)
  - Booking creation, retrieval, updates, deletion
  - Role-based filtering (Customers vs Staff/Admin)
  - Authorization checks
  - Conflict detection
  
- **UserLogInServiceTests** (15 tests)
  - Login validation
  - Logout functionality
  - Token revocation
  
- **UserRegistrationServiceTests** (17 tests)
  - User registration
  - Email validation
  - Password strength validation
  - Duplicate email detection
  
- **UserServiceServiceTests** (7 tests)
  - Service CRUD operations
  - Validation rules
  
- **RefreshServiceTests** (10 tests)
  - Token refresh and rotation
  - Expired token handling
  - Revoked token detection
  - JWT claim verification

#### Integration Tests (15 tests)
- **Authentication Flow Tests**
  - Login → Refresh → Protected Endpoint flow
  - Logout with token revocation
  - 401 responses for unauthorized access
  
- **Booking Ownership Tests**
  - Customers see only their own bookings
  - Staff/Admin see all bookings
  - Authorization boundary enforcement
  
- **Token Security Tests**
  - JWT token structure and claims
  - Multiple refresh cycles with rotation
  - Token reuse prevention
  
- **End-to-End Workflows**
  - Complete customer booking lifecycle
  - Service CRUD operations
  - Conflict detection
  - Registration validation

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~RefreshServiceTests"

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Using Scalar API Documentation
1. Start the application
2. Navigate to `https://localhost:7022/scalar/v1`
3. Use the interactive UI to test endpoints
4. Authenticate using JWT tokens

### Using Postman/Thunder Client
1. Import the OpenAPI specification from `https://localhost:7022/openapi/v1.json`
2. Set up authentication with Bearer token
3. Test endpoints

## Project Structure

```
ServiceBookingPlatform/
├── Controllers/
│   ├── UserBookingController.cs      # Booking endpoints with authorization
│   ├── UserLogInController.cs        # Login & logout with JWT
│   ├── UserRegistrationController.cs # User registration
│   ├── UserServiceController.cs      # Service management (Admin only)
│   └── RefreshController.cs          # Token refresh endpoint (/auth/refresh)
├── Data/
│   └── AppDbContext.cs               # EF Core DbContext with RefreshTokens
├── Middleware/
│   └── GlobalExceptionHandlerMiddleware.cs  # Centralized error handling
├── Models/
│   ├── Booking.cs                    # Booking entity
│   ├── Service.cs                    # Service entity
│   ├── User.cs                       # User entity
│   ├── RefreshToken.cs               # Refresh token entity
│   ├── Configuration/
│   │   └── JwtConfig.cs              # Strongly-typed JWT configuration
│   └── Dtos/                         # Data Transfer Objects
│       ├── Booking/
│       │   ├── BookingDto.cs
│       │   ├── CreateBookingDto.cs
│       │   └── UpdateBookingDto.cs
│       ├── Service/
│       │   ├── ServiceDto.cs
│       │   ├── CreateServiceDto.cs
│       │   └── UpdateServiceDto.cs
│       └── User/
│           ├── UserDto.cs
│           ├── UserLogInRequestDto.cs
│           ├── UserLogInResponseDto.cs      # Includes access + refresh tokens
│           └── RefreshTokenRequestDto.cs    # For refresh/logout
├── Services/
│   ├── Interfaces/
│   │   ├── IUserBookingService.cs    # Booking service interface
│   │   ├── IUserServiceService.cs    # Service service interface
│   │   ├── IUserRegistrationService.cs
│   │   ├── IUserLogInService.cs
│   │   ├── IJwtService.cs            # JWT generation interface
│   │   └── IRefreshService.cs        # Token refresh interface
│   ├── UserBookingService.cs         # Claims-based authorization
│   ├── UserServiceService.cs         # Service CRUD operations
│   ├── UserRegistrationService.cs    # User registration with validation
│   ├── UserLogInService.cs           # Login/logout with token management
│   ├── JwtService.cs                 # JWT + refresh token generation
│   ├── RefreshService.cs             # Token refresh & rotation logic
│   └── TokenService.cs               # Secure random token generation
├── UnitTests/
│   ├── UserBookingServiceTests.cs    # 31 booking tests
│   ├── UserLogInServiceTests.cs      # 15 login/logout tests
│   ├── UserRegistrationServiceTests.cs # 17 registration tests
│   ├── UserServiceServiceTests.cs    # 7 service tests
│   ├── RefreshServiceTests.cs        # 10 refresh token tests
│   ├── JwtServiceTests.cs            # JWT generation tests
│   └── IntegrationTests.cs           # 15 end-to-end integration tests
├── Program.cs                        # Application startup with middleware pipeline
├── appsettings.json                  # Configuration (JWT, DB, CORS, Logging)
└── appsettings.Development.json      # Development-specific settings
```

## Middleware Pipeline Order

The application processes requests through the following middleware (order matters):

1. **GlobalExceptionHandlerMiddleware** - Catches all unhandled exceptions
2. **ResponseCompressionMiddleware** - Compresses responses (gzip, brotli)
3. **DeveloperExceptionPage** - Detailed errors (Development only)
4. **HttpsRedirection** - Redirects HTTP to HTTPS
5. **CORS** - Handles cross-origin requests
6. **RateLimiter** - Enforces rate limits (100 req/min per user)
7. **Authentication** - Validates JWT tokens
8. **Authorization** - Checks user permissions
9. **Controllers** - Handles API requests

## Security Features

- **Password Hashing:** Uses ASP.NET Core Identity's `PasswordHasher` with salt (no plaintext passwords anywhere)
- **JWT Claims:** User ID, email, and role embedded in token for authorization checks
- **Dual Token System:** 
  - **Access Token:** Short-lived (30 min), used for API authorization
  - **Refresh Token:** Long-lived (7 days), used only for obtaining new access tokens
- **Token Rotation:** Automatic refresh token rotation prevents reuse attacks
- **Token Revocation:** Logout immediately revokes refresh tokens in database
- **Secure Token Generation:** Uses cryptographically secure random number generator (64 bytes, base64-encoded)
- **Automatic Cleanup:** Expired/revoked tokens removed on next user login
- **Role-Based Authorization:** Different access levels for Customers, Staff, and Admins
- **Input Validation:** Data annotations and custom validators using FieldValidatorAPI
- **SQL Injection Protection:** Entity Framework parameterized queries
- **Authorization Checks:** Services verify user identity before returning/modifying data
- **CORS:** Configurable cross-origin resource sharing
- **HTTPS:** Enforced in production
- **JWT Signing:** HMAC-SHA256 algorithm with configurable secret key

### How Authorization Works

When you make a request to a protected endpoint:
1. The API extracts your JWT token from the Authorization header
2. It validates the token signature and expiration
3. It reads the claims (NameId, Email, Role) from the token
4. Controllers and services use `User.FindFirst(ClaimTypes.Role)` to check permissions
5. Services like `UserBookingService` filter data based on role (Customers see only their bookings)

## Common Issues & Solutions

### "Unauthorized" when accessing bookings
Make sure you:
1. Have logged in and received a JWT token
2. Include the **access token** in the Authorization header: `Bearer {access-token}`
3. Your access token hasn't expired (tokens last 30 minutes)
4. If expired, use your refresh token to get a new access token via `/auth/refresh`
5. You have the right role (GetAllBookings requires Staff or Admin)

### Access token expired but refresh token still valid
Don't ask the user to log in again! Instead:
1. Call `POST /auth/refresh` with the refresh token
2. Receive new access token + new refresh token
3. Store both new tokens
4. Retry the original failed request with the new access token

This is the expected flow for seamless user experience.

### Refresh token returns 401 or "Invalid refresh token"
Possible causes:
1. User logged out (token was revoked)
2. Token has expired (7 days passed)
3. Token was already used (token rotation - old tokens can't be reused)
4. Token doesn't exist in database

Solution: Force user to log in again to get fresh tokens.

### Tests failing with "Sequence contains no elements"
This usually means you're trying to test a method that queries the database without seeding test data. Make sure to call helper methods like `SeedTestUser()` before testing.

### JWT claims not found in tests
If you're testing authorization, create a `ClaimsPrincipal` with the required claims:
```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
    new Claim(ClaimTypes.Email, email),
    new Claim(ClaimTypes.Role, role)
};
var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
```

## Validation Rules

### User Registration
- **First Name:** Required, 2-50 characters
- **Last Name:** Required, 2-50 characters
- **Email:** Required, valid email format, must be unique
- **Password:** Required, 6-10 characters, must contain:
  - At least one uppercase letter
  - At least one lowercase letter
  - At least one digit
  - At least one special character (!@#$%^&*()_+)
- **Phone Number:** Required, valid UK format
- **Role:** Optional, defaults to "Customer"

### Service Creation
- **Service Name:** Required, 2-100 characters
- **Service Type:** Required, 2-50 characters
- **Service Description:** Optional, max 500 characters

## Known Issues

None currently. The comprehensive test suite (92 tests with 100% pass rate) catches issues before they reach production.

## Performance Considerations

- **Response Compression:** Reduces bandwidth by 60-80% for JSON responses
- **Rate Limiting:** Prevents abuse and ensures fair resource allocation
- **Connection Pooling:** EF Core automatically manages database connections
- **Query Optimization:** Includes related entities efficiently to minimize round trips
- **Async/Await:** All I/O operations are asynchronous for better scalability
- **Caching:** Static `JsonSerializerOptions` cached in middleware

## Production Deployment Checklist

Before deploying to production:

- [ ] Update `JwtConfig:Key` to a secure, random 32+ character string
- [ ] Set `RequireHttpsMetadata = true` in JWT configuration
- [ ] Configure production database connection string
- [ ] Update `AllowedOrigins` with actual frontend domain(s)
- [ ] Review and adjust rate limiting thresholds
- [ ] Enable Application Insights or similar monitoring
- [ ] Configure HSTS headers
- [ ] Set up database backups
- [ ] Review and update logging levels
- [ ] Test all endpoints in staging environment
- [ ] Run full test suite (`dotnet test`)

## API Best Practices Implemented

✅ **RESTful Design** - Proper HTTP verbs and status codes  
✅ **Versioning Ready** - Structure supports API versioning  
✅ **Authentication** - Secure JWT-based authentication  
✅ **Authorization** - Role-based and claims-based authorization  
✅ **Error Handling** - Consistent error responses with ProblemDetails  
✅ **Validation** - Input validation at DTO level  
✅ **Logging** - Comprehensive logging throughout the application  
✅ **Testing** - 92 tests covering unit and integration scenarios  
✅ **Documentation** - Interactive API docs with Scalar  
✅ **CORS** - Configurable cross-origin support  
✅ **Rate Limiting** - Protection against abuse  
✅ **Compression** - Response optimization  
✅ **HTTPS** - Enforced secure communication  
✅ **DI Container** - Proper dependency injection  
✅ **Separation of Concerns** - Controllers, Services, DTOs, Models  

## Contributing

This is a portfolio project demonstrating enterprise-grade ASP.NET Core development. For questions or suggestions, please open an issue on GitHub.

## License

This project is for educational and portfolio purposes.

## Recent Changes

### v3.0 - Enterprise-Grade Improvements 
**Infrastructure & Architecture:**
- **Global Exception Handling** - Centralized middleware with standardized `ProblemDetails` responses
- **Strongly-Typed Configuration** - `JwtConfig` class with `IOptions<T>` pattern
- **Rate Limiting** - 100 requests per minute per user/IP
- **CORS Support** - Configurable cross-origin policies for frontend integration
- **Response Compression** - Automatic gzip/brotli compression
- **Connection Resilience** - Database retry logic (5 attempts, exponential backoff)
- **Enhanced Logging** - Structured logging with configurable levels
- **Interface-Based Services** - All services use interfaces (`IJwtService`, `IRefreshService`, etc.)

**Error Handling:**
- Centralized exception mapping to HTTP status codes
- Development mode includes stack traces and trace IDs
- Production mode returns safe, generic error messages
- Logs all errors with full context

**Testing:**
- **92 passing tests** (100% success rate)
- **15 integration tests** using WebApplicationFactory
- **77 unit tests** with full coverage
- Test isolation with unique database per test run
- Comprehensive test scenarios (auth flows, booking ownership, token rotation)

**Security Enhancements:**
- JWT event handlers for expired token detection
- Multiple claim type fallback for better compatibility
- Token expiry headers in 401 responses
- Custom challenge responses with JSON error messages

**Developer Experience:**
- Detailed OpenAPI documentation with descriptions
- Better error messages with actionable information
- Development-only sensitive data logging
- Scalar UI improvements

### v2.0 - Token Management & Security
- **Refresh Token System** - Implemented dual-token authentication (access + refresh)
- **Token Rotation** - Automatic refresh token rotation for enhanced security
- **Logout Functionality** - Added `/api/UserLogIn/logout` endpoint with token revocation
- **Refresh Endpoint** - Added `/auth/refresh` route aligned with frontend expectations
- **Token Cleanup** - Automatic removal of expired/revoked tokens
- **Secure Token Generation** - Cryptographically secure 64-byte refresh tokens
- **RefreshTokens Database Table** - New table to track and manage refresh tokens
- **Comprehensive Test Suite** - 79 unit tests covering all functionality

### v1.0 - Core Features
- **JWT Authentication** - Implemented proper JWT tokens with NameId, Email, and Role claims
- **Role-Based Authorization** - Customers can only see their own bookings, Staff/Admin see everything
- **Booking Authorization** - Services now use ClaimsPrincipal for proper authorization checks
- **Error Handling** - Controllers catch and properly return authorization exceptions
- **User ID from Token** - CreateBooking extracts user ID from JWT instead of request body
- **Booking Conflict Detection** - Can't double-book services anymore
- **Claims Verification** - Tests handle multiple JWT claim type formats

## What's Next

 Completed Features:
-  **Refresh tokens** - Implemented with 7-day expiry and token rotation
-  **Logout functionality** - Token revocation on logout
-  **Token security** - Automatic cleanup and secure generation
-  **Global exception handling** - Centralized error handling middleware
-  **Rate limiting** - Protection against API abuse
-  **CORS support** - Frontend integration ready
-  **Response compression** - Optimized bandwidth usage
-  **Comprehensive testing** - 92 tests with 100% pass rate
-  **Production-ready error handling** - Standardized ProblemDetails
-  **Connection resilience** - Database retry logic

 Future Enhancements:
-  Email notifications for booking confirmations/cancellations
-  API versioning implementation (v1, v2)
-  Redis caching for frequently accessed data
-  Background job for token cleanup (hosted service)
-  Health check endpoints
-  Metrics and monitoring (Application Insights)
-  Distributed caching for multi-instance deployments
-  Webhook support for third-party integrations

---

**Built with love using .NET 10 and industry best practices**
- [ ] Payment integration (Stripe maybe?)
- [ ] Customer reviews and ratings for services
- [ ] Service availability calendar (no more manual conflict checks)
- [ ] Admin dashboard to see all the stats
- [ ] Booking history and analytics
- [ ] Integration tests using WebApplicationFactory (the unit tests are solid, but end-to-end tests would be nice)
- [ ] Rate limiting to prevent abuse
- [ ] Remember me / extended sessions (optional longer refresh token expiry)
- [ ] Multi-device session management (view/revoke tokens from different devices)
- [ ] Token blacklist for immediate access token revocation

## Contributing

If you want to contribute:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/CoolNewThing`)
3. Write tests for your changes (aim for the same coverage we have now)
4. Make sure all **79 tests** pass (`dotnet test`)
5. Commit your changes (`git commit -m 'Add cool new thing'`)
6. Push to the branch (`git push origin feature/CoolNewThing`)
7. Open a Pull Request

Please write tests. Seriously, write tests. The test suite has 79 tests with excellent coverage. The CI will thank you.

## License

This project is licensed under the MIT License.

## Author

**sav-afg**
- GitHub: [@sav-afg](https://github.com/sav-afg)
- Repository: [ServiceBookingPlatform](https://github.com/sav-afg/ServiceBookingPlatform)

## Acknowledgments

- Built with .NET 10 and ASP.NET Core
- API documentation powered by Scalar
- Authentication with JWT Bearer tokens
- Database management with Entity Framework Core
