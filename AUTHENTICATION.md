# Authentication API Documentation

## Overview

The ECommerce API now includes JWT-based authentication with role-based authorization. The system supports two main roles: **Admin** and **SuperAdmin**.

## Pre-seeded Users

When the application starts, it automatically creates the following admin users:

| Email | Password | Roles |
|-------|----------|-------|
| admin@test.com | Admin123! | Admin |
| superadmin@test.com | SuperAdmin123! | SuperAdmin, Admin |

## Authentication Endpoints

### 1. Login
**POST** `/api/auth/login`

```json
{
  "email": "admin@test.com",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "id": "user-id",
  "email": "admin@test.com",
  "displayName": "Administrator",
  "token": "jwt-token-here",
  "roles": ["Admin"]
}
```

### 2. Register
**POST** `/api/auth/register`

```json
{
  "email": "newuser@test.com",
  "displayName": "New User",
  "password": "NewPass123!",
  "confirmPassword": "NewPass123!"
}
```

**Response:**
```json
{
  "id": "user-id",
  "email": "newuser@test.com",
  "displayName": "New User",
  "token": "jwt-token-here",
  "roles": []
}
```

### 3. Get Current User
**GET** `/api/auth/current-user`

**Headers:**
```
Authorization: Bearer {jwt-token}
```

**Response:**
```json
{
  "id": "user-id",
  "email": "admin@test.com",
  "displayName": "Administrator",
  "token": "refreshed-jwt-token",
  "roles": ["Admin"]
}
```

### 4. Assign Role (SuperAdmin Only)
**POST** `/api/auth/assign-role`

**Headers:**
```
Authorization: Bearer {super-admin-jwt-token}
```

**Body:**
```json
{
  "email": "user@test.com",
  "role": "Admin"
}
```

### 5. Check Email Exists
**GET** `/api/auth/check-email/{email}`

**Response:**
```json
true
```

### 6. Get User Roles
**GET** `/api/auth/user-roles/{userId}`

**Headers:**
```
Authorization: Bearer {jwt-token}
```

**Response:**
```json
["Admin", "SuperAdmin"]
```

## Role-Based Authorization

### Available Roles
- **Admin**: Can access admin-level endpoints
- **SuperAdmin**: Can access all endpoints including role management

### Using Roles in Controllers

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    // Admin-only endpoints
}

[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController : ControllerBase
{
    // SuperAdmin-only endpoints
}

[Authorize(Roles = "Admin,SuperAdmin")]
public IActionResult SomeEndpoint()
{
    // Accessible by Admin or SuperAdmin
}
```

## JWT Configuration

The JWT tokens are configured with the following settings:
- **Secret**: Configured in `appsettings.json`
- **Issuer**: ECommerceApi
- **Audience**: ECommerceApiUsers
- **Expiration**: 7 days

## Middleware

The application includes a custom `RoleBasedAuthorizationMiddleware` that logs user access with their roles for monitoring purposes.

## Password Requirements

- Minimum 8 characters
- At least one digit
- At least one lowercase letter
- At least one uppercase letter
- At least one non-alphanumeric character

## Getting Started

1. Start the application
2. The database will be automatically seeded with roles and admin users
3. Use the login endpoint with pre-seeded credentials
4. Include the returned JWT token in the Authorization header for protected endpoints

Example using curl:
```bash
# Login
curl -X POST "https://localhost:7277/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@test.com", "password": "Admin123!"}'

# Use token for protected endpoint
curl -X GET "https://localhost:7277/api/auth/current-user" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```