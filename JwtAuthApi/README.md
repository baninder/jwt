# JWT Authentication API – Enterprise Edition

A comprehensive .NET Web API project demonstrating JWT token authentication, claims-based authorization, and enterprise-level architecture using best practices and design patterns.

---

## 🚀 Features

- **JWT Token Generation & Validation** (Strategy, Factory patterns)
- **User Authentication**: Login and registration endpoints
- **Role-Based Authorization**: User/Admin roles
- **Claims-Based Identity**: Rich user information in JWT claims
- **CORS Support**: Frontend integration ready
- **In-Memory User Store**: Easy demo setup (replace with DB for production)
- **Enterprise Patterns**:
  - Strategy, Factory, Repository, Specification, Result, Unit of Work, Dependency Injection
  - Structured Logging, Middleware Pipeline, Fluent Validation, Configuration Options
- **Swagger/OpenAPI**: Interactive API docs
- **Frontend Demo**: HTML/JS demo in `wwwroot/index.html`

---

## 🏗️ Project Structure

```
JwtAuthApi/
├── Configuration/         # JWT and app settings
├── Controllers/           # API endpoints (Auth, User, Admin, Public)
├── Core/
│   ├── Common/            # Result pattern
│   ├── Configuration/     # AppSettings, CORS
│   ├── Extensions/        # Service registration
│   ├── Factories/         # Token strategy factory
│   ├── Interfaces/        # Repository, Strategy, UnitOfWork
│   ├── Logging/           # Logging abstraction
│   ├── Middleware/        # Request logging
│   ├── Repositories/      # In-memory repository
│   ├── Services/          # Enterprise services
│   ├── Specifications/    # Specification pattern
│   ├── Strategies/        # Token strategies
│   ├── UnitOfWork/        # Unit of Work
│   └── Validators/        # FluentValidation
├── Models/                # DTOs and entities
├── Services/              # Legacy services (for reference)
├── wwwroot/               # Frontend demo
├── Program.cs             # Main entry
├── appsettings.json       # Config
├── enterprise-test-script.ps1 # PowerShell test suite
└── ...
```

---

## 🌐 API Endpoints

### Public (No Auth)
- `GET /api/public/info` – API info, architecture, test credentials
- `GET /api/public/health` – Health check
- `POST /api/auth/login` – User login
- `POST /api/auth/register` – User registration
- `POST /api/auth/validate-token` – Validate JWT token

### Protected (JWT Required)
- `GET /api/user/profile` – Current user profile
- `GET /api/user/dashboard` – User dashboard data

### Admin (Admin Role Required)
- `GET /api/admin/users` – List all users
- `GET /api/admin/analytics` – System analytics
- `POST /api/admin/system-settings` – Update settings

---

## 🧩 Enterprise Patterns & Architecture

- **Strategy Pattern**: Pluggable JWT token generation/validation
- **Factory Pattern**: Token strategy factory
- **Repository Pattern**: Data access abstraction
- **Specification Pattern**: Query logic encapsulation
- **Result Pattern**: Consistent operation outcomes
- **Unit of Work**: Transaction management
- **Dependency Injection**: Service lifetime management
- **Logging Abstraction**: Structured logging, request correlation
- **Fluent Validation**: Input validation with detailed errors
- **Configuration Options**: Strongly-typed settings
- **Middleware Pipeline**: Request/response logging

---

## 🛠️ Getting Started

### Prerequisites
- .NET 10.0 or later
- Windows (PowerShell), Linux, or macOS

### Run the API
```pwsh
cd JwtAuthApi
# Restore dependencies
pwsh -c "dotnet restore"
# Run the API
pwsh -c "dotnet run"
```
- API Base: `http://localhost:5192/api`
- Swagger: [http://localhost:5192/swagger](http://localhost:5192/swagger)
- Frontend Demo: [http://localhost:5192/index.html](http://localhost:5192/index.html)

### Run Enterprise Test Suite
```pwsh
cd JwtAuthApi
pwsh -File .\enterprise-test-script.ps1
```

---

## 🧪 Test Credentials

| Role   | Email                    | Password     |
|--------|--------------------------|--------------|
| Admin  | jane.smith@example.com   | admin123     |
| User   | john.doe@example.com     | password123  |

---

## ⚙️ Configuration

Edit `appsettings.json` for JWT, CORS, and app settings:
```json
{
  "JwtSettings": {
    "SecretKey": "MyVerySecretKeyThatIsAtLeast256BitsLongForSecurity123456789",
    "Issuer": "JwtAuthApi-Dev",
    "Audience": "JwtAuthApiUsers-Dev",
    "ExpiryInMinutes": 60,
    "RefreshTokenExpiryInDays": 7
  },
  "AppSettings": {
    "ApplicationName": "JWT Auth API Enterprise",
    "Version": "2.0.0",
    "EnableDetailedErrors": true,
    "EnableRequestLogging": true,
    "Cors": {
      "AllowedOrigins": ["http://localhost:3000", "http://localhost:5173", "http://localhost:8080"],
      "AllowedMethods": ["GET", "POST", "PUT", "DELETE", "OPTIONS"],
      "AllowedHeaders": ["*"],
      "AllowCredentials": true
    }
  }
}
```

---

## 📝 Example Usage (JavaScript)

```js
// Login
const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email: 'john.doe@example.com', password: 'password123' })
});
const authData = await response.json();
const token = authData.token;

// Use token for authenticated requests
const userProfile = await fetch('/api/user/profile', {
    headers: { 'Authorization': `Bearer ${token}` }
});
```

---

## 🔒 JWT Claims Structure
- `user_id`, `email`, `first_name`, `last_name`, `role`, `iat`, `exp`

---

## 🛡️ Security & Production Notes
- Use a real database for users (see Repository/UnitOfWork patterns)
- Use strong password hashing (bcrypt/Argon2)
- Store secrets securely (Azure Key Vault, etc.)
- Enable HTTPS in production
- Add rate limiting, monitoring, and advanced logging

---

## 🧪 Testing with PowerShell

```pwsh
# Login as admin
$body = @{ email = 'jane.smith@example.com'; password = 'admin123' } | ConvertTo-Json
$response = Invoke-RestMethod -Uri 'http://localhost:5192/api/auth/login' -Method Post -Body $body -ContentType 'application/json'
$response.token

# Use token for protected endpoint
$headers = @{ Authorization = "Bearer $($response.token)" }
Invoke-RestMethod -Uri 'http://localhost:5192/api/admin/users' -Headers $headers
```

---

## 📖 Documentation & Demo
- **Swagger UI**: [http://localhost:5192/swagger](http://localhost:5192/swagger)
- **Frontend Demo**: [http://localhost:5192/index.html](http://localhost:5192/index.html)
- **Enterprise Test Script**: `enterprise-test-script.ps1`

---

## 📄 License

This project is for educational purposes and can be used as a starting point for JWT authentication in .NET enterprise applications.

---
