# Service Booking Platform API

A comprehensive RESTful API for managing service bookings, user authentication, and service management built with .NET 10.

## Features

- **User Management**
- User registration with validation
- JWT-based authentication with access and refresh tokens
- Token refresh and rotation for enhanced security
- Secure logout with token revocation
- Password hashing using ASP.NET Core Identity
- Email uniqueness validation
- Automatic cleanup of expired/revoked tokens

- **Booking Management**
  - Create, read, update, and delete bookings
  - View all bookings with user and service details
  - Get booking by ID
  - Booking status tracking (Pending, Confirmed, Completed, Cancelled)

- **Service Management**
  - Create, read, update, and delete services
  - Service categorization by type
  - Service descriptions

- **Security**
- JWT Bearer token authentication (access + refresh tokens)
- Refresh token rotation to prevent token reuse attacks
- Token revocation on logout
- Password strength validation
- Role-based authorization (Customer, Staff, Admin)
- Automatic expiration of access tokens (30 minutes)
- Long-lived refresh tokens (7 days) with revocation support

- **API Documentation**
  - Interactive API documentation with Scalar
  - OpenAPI/Swagger specification
  - JWT authentication integration in API explorer

## Technologies

- **.NET 10** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Relational database
- **JWT (JSON Web Tokens)** - Stateless authentication
- **Scalar** - Modern API documentation
- **ASP.NET Core Identity** - Password hashing and user management

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
    "Key": "YOUR_SECRET_KEY_HERE",
    "TokenValidityMins": 30
  }
}
```

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ServiceBookingPlatform;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## Testing

### Using Scalar 
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
│   ├── UserBookingController.cs      # Booking endpoints
│   ├── UserLogInController.cs        # Login & logout endpoints
│   ├── UserRegistrationController.cs # User registration
│   ├── UserServiceController.cs      # Service management
│   └── RefreshController.cs          # Token refresh endpoint
├── Data/
│   └── AppDbContext.cs               # EF Core DbContext with RefreshTokens
├── Migrations/                       # Database migrations
├── Models/
│   ├── Booking.cs                    # Booking entity
│   ├── Service.cs                    # Service entity
│   ├── User.cs                       # User entity
│   ├── RefreshToken.cs               # Refresh token entity
│   └── Dtos/                         # Data Transfer Objects
│       ├── Booking/
│       ├── Service/
│       └── User/
│           ├── UserLogInResponseDto.cs       # Includes refresh token
│           └── RefreshTokenRequestDto.cs     # For refresh/logout
├── Services/
│   ├── IUserBookingService.cs        # Booking service interface
│   ├── UserBookingService.cs         # Booking service implementation
│   ├── IUserServiceService.cs        # Service service interface
│   ├── UserServiceService.cs         # Service service implementation
│   ├── IUserRegistrationService.cs   # Registration service interface
│   ├── UserRegistrationService.cs    # Registration service implementation
│   ├── IUserLogInService.cs          # Login/logout service interface
│   ├── UserLogInService.cs           # Login/logout service implementation
│   ├── JwtService.cs                 # JWT + refresh token generation
│   ├── RefreshService.cs             # Token refresh & rotation logic
│   └── TokenService.cs               # Secure token generation utility
├── UnitTests/
│   ├── UserBookingServiceTests.cs    # 30 booking tests
│   ├── UserLogInServiceTests.cs      # 15 login/logout tests
│   ├── UserRegistrationServiceTests.cs # 17 registration tests
│   ├── UserServiceServiceTests.cs    # 7 service tests
│   └── RefreshServiceTests.cs        # 10 refresh token tests
├── Program.cs                        # Application entry point
└── appsettings.json                  # Configuration
```

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

None currently. The test suite catches most issues before they make it to production.

## Recent Changes

### v2.0 - Token Management & Security Enhancements
- **Refresh Token System** - Implemented dual-token authentication (access + refresh)
- **Token Rotation** - Automatic refresh token rotation for enhanced security
- **Logout Functionality** - Added `/api/UserLogIn/logout` endpoint with token revocation
- **Refresh Endpoint** - Added `/auth/refresh` route aligned with frontend expectations
- **Token Cleanup** - Automatic removal of expired/revoked tokens
- **Secure Token Generation** - Cryptographically secure 64-byte refresh tokens
- **RefreshTokens Database Table** - New table to track and manage refresh tokens
- **Comprehensive Test Suite** - **79 unit tests** covering all functionality including 10 new refresh token tests

### v1.0 - Core Features
- **JWT Authentication** - Implemented proper JWT tokens with NameId, Email, and Role claims
- **Role-Based Authorization** - Customers can only see their own bookings, Staff/Admin see everything
- **Booking Authorization** - Services now use ClaimsPrincipal for proper authorization checks
- **Error Handling** - Controllers catch and properly return authorization exceptions
- **User ID from Token** - CreateBooking extracts user ID from JWT instead of request body
- **Booking Conflict Detection** - Can't double-book services anymore
- **Claims Verification** - Tests handle multiple JWT claim type formats

## What's Next

Completed:
- [x] **Refresh tokens** - Implemented with 7-day expiry and token rotation
- [x] **Logout functionality** - Token revocation on logout
- [x] **Token security** - Automatic cleanup and secure generation

Thinking about adding:
- [ ] Email notifications when bookings are confirmed/cancelled
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
