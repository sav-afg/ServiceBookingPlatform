# Service Booking Platform API

A comprehensive RESTful API for managing service bookings, user authentication, and service management built with .NET 10.

## Features

- **User Management**
  - User registration with validation
  - JWT-based authentication
  - Password hashing using ASP.NET Core Identity
  - Email uniqueness validation

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
  - JWT Bearer token authentication
  - Password strength validation
  - Role-based authorization (Customer, Staff, Admin)

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
  "expiresIn": 1800
}
```

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
- Cascade delete on foreign keys

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
│   ├── UserLogInController.cs        # Authentication endpoints
│   ├── UserRegistrationController.cs # User registration
│   └── UserServiceController.cs      # Service management
├── Data/
│   └── AppDbContext.cs               # EF Core DbContext
├── Migrations/                       # Database migrations
├── Models/
│   ├── Booking.cs                    # Booking entity
│   ├── Service.cs                    # Service entity
│   ├── User.cs                       # User entity
│   └── Dtos/                         # Data Transfer Objects
│       ├── Booking/
│       ├── Service/
│       └── User/
├── Services/
│   ├── IUserBookingService.cs        # Booking service interface
│   ├── UserBookingService.cs         # Booking service implementation
│   ├── IUserServiceService.cs        # Service service interface
│   ├── UserServiceService.cs         # Service service implementation
│   ├── IUserRegistrationService.cs   # Registration service interface
│   ├── UserRegistrationService.cs    # Registration service implementation
│   ├── IUserLogInService.cs          # Login service interface
│   ├── UserLogInService.cs           # Login service implementation
│   └── JwtService.cs                 # JWT token generation
├── Program.cs                        # Application entry point
└── appsettings.json                  # Configuration
```

## Security Features

- **Password Hashing:** Uses ASP.NET Core Identity's `PasswordHasher` with salt
- **Input Validation:** Data annotations and custom validators
- **SQL Injection Protection:** Entity Framework parameterized queries
- **CORS:** Configurable cross-origin resource sharing
- **HTTPS:** Enforced in production
- **JWT Signing:** HMAC-SHA256 algorithm

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

- None currently reported

## Planned Features

- [ ] Advanced booking scheduling with conflict detection
- [ ] Email notifications for bookings
- [ ] Payment integration
- [ ] Customer reviews and ratings
- [ ] Service availability calendar
- [ ] Admin dashboard
- [ ] Booking history and analytics

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

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
