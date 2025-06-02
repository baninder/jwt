# JWT Authentication API Demo Script
# This script demonstrates the complete functionality of the JWT Auth API

Write-Host "🚀 JWT Authentication API Demo" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host ""

$baseUrl = "http://localhost:5192/api"

# Test 1: Get Public API Information
Write-Host "📋 1. Getting Public API Information..." -ForegroundColor Yellow
try {
    $apiInfo = Invoke-RestMethod -Uri "$baseUrl/public/info" -Method GET
    Write-Host "✅ API Name: $($apiInfo.applicationName)" -ForegroundColor Green
    Write-Host "✅ Version: $($apiInfo.version)" -ForegroundColor Green
    Write-Host "✅ Available Endpoints: $($apiInfo.supportedEndpoints.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Failed to get API info: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Test 2: Admin Login
Write-Host "🔐 2. Testing Admin Login..." -ForegroundColor Yellow
$adminLogin = @{
    email = "admin@example.com"
    password = "admin123"
} | ConvertTo-Json

try {
    $adminResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method POST -Body $adminLogin -ContentType "application/json"
    $adminToken = $adminResponse.token
    $adminHeaders = @{ "Authorization" = "Bearer $adminToken" }
    
    Write-Host "✅ Admin login successful!" -ForegroundColor Green
    Write-Host "✅ User: $($adminResponse.user.firstName) $($adminResponse.user.lastName)" -ForegroundColor Green
    Write-Host "✅ Role: $($adminResponse.user.role)" -ForegroundColor Green
    Write-Host "✅ Token expires: $($adminResponse.expiresAt)" -ForegroundColor Green
} catch {
    Write-Host "❌ Admin login failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Test 3: Regular User Login
Write-Host "👤 3. Testing Regular User Login..." -ForegroundColor Yellow
$userLogin = @{
    email = "user@example.com"
    password = "user123"
} | ConvertTo-Json

try {
    $userResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method POST -Body $userLogin -ContentType "application/json"
    $userToken = $userResponse.token
    $userHeaders = @{ "Authorization" = "Bearer $userToken" }
    
    Write-Host "✅ User login successful!" -ForegroundColor Green
    Write-Host "✅ User: $($userResponse.user.firstName) $($userResponse.user.lastName)" -ForegroundColor Green
    Write-Host "✅ Role: $($userResponse.user.role)" -ForegroundColor Green
} catch {
    Write-Host "❌ User login failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Test 4: Access User Profile with Admin Token
Write-Host "🏠 4. Testing User Profile Access (Admin)..." -ForegroundColor Yellow
try {
    $adminProfile = Invoke-RestMethod -Uri "$baseUrl/user/profile" -Method GET -Headers $adminHeaders
    Write-Host "✅ Admin profile access successful!" -ForegroundColor Green
    Write-Host "✅ Profile ID: $($adminProfile.id)" -ForegroundColor Green
    Write-Host "✅ Email: $($adminProfile.email)" -ForegroundColor Green
    Write-Host "✅ Claims count: $($adminProfile.claims.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Admin profile access failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 5: Access User Profile with Regular User Token
Write-Host "🏠 5. Testing User Profile Access (Regular User)..." -ForegroundColor Yellow
try {
    $userProfile = Invoke-RestMethod -Uri "$baseUrl/user/profile" -Method GET -Headers $userHeaders
    Write-Host "✅ User profile access successful!" -ForegroundColor Green
    Write-Host "✅ Profile ID: $($userProfile.id)" -ForegroundColor Green
    Write-Host "✅ Email: $($userProfile.email)" -ForegroundColor Green
} catch {
    Write-Host "❌ User profile access failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 6: Access Admin Endpoint with Admin Token
Write-Host "👑 6. Testing Admin Endpoint Access (Admin Token)..." -ForegroundColor Yellow
try {
    $allUsers = Invoke-RestMethod -Uri "$baseUrl/admin/users" -Method GET -Headers $adminHeaders
    Write-Host "✅ Admin endpoint access successful!" -ForegroundColor Green
    Write-Host "✅ Total users retrieved: $($allUsers.Count)" -ForegroundColor Green
    foreach ($user in $allUsers) {
        Write-Host "   - $($user.firstName) $($user.lastName) ($($user.role))" -ForegroundColor Cyan
    }
} catch {
    Write-Host "❌ Admin endpoint access failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 7: Try to Access Admin Endpoint with Regular User Token (Should Fail)
Write-Host "🚫 7. Testing Admin Endpoint Access (Regular User Token - Should Fail)..." -ForegroundColor Yellow
try {
    $unauthorizedAccess = Invoke-RestMethod -Uri "$baseUrl/admin/users" -Method GET -Headers $userHeaders
    Write-Host "❌ ERROR: Regular user should not have admin access!" -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq "Forbidden") {
        Write-Host "✅ Access correctly denied! Regular users cannot access admin endpoints." -ForegroundColor Green
    } else {
        Write-Host "❌ Unexpected error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""

# Test 8: Try to Access Protected Endpoint without Token (Should Fail)
Write-Host "🔒 8. Testing Protected Endpoint without Token (Should Fail)..." -ForegroundColor Yellow
try {
    $unauthorizedProfile = Invoke-RestMethod -Uri "$baseUrl/user/profile" -Method GET
    Write-Host "❌ ERROR: Should not be able to access without token!" -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq "Unauthorized") {
        Write-Host "✅ Access correctly denied! Authentication required for protected endpoints." -ForegroundColor Green
    } else {
        Write-Host "❌ Unexpected error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""

# Test 9: Register New User
Write-Host "📝 9. Testing User Registration..." -ForegroundColor Yellow
$newUser = @{
    email = "testuser$(Get-Random -Min 1000 -Max 9999)@example.com"
    firstName = "Test"
    lastName = "User"
    password = "testpassword123"
    role = "User"
} | ConvertTo-Json

try {
    $registrationResponse = Invoke-RestMethod -Uri "$baseUrl/auth/register" -Method POST -Body $newUser -ContentType "application/json"
    Write-Host "✅ User registration successful!" -ForegroundColor Green
    Write-Host "✅ New user ID: $($registrationResponse.user.id)" -ForegroundColor Green
    Write-Host "✅ Email: $($registrationResponse.user.email)" -ForegroundColor Green
    Write-Host "✅ Token provided upon registration" -ForegroundColor Green
} catch {
    Write-Host "❌ User registration failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 10: Get Analytics (Admin Only)
Write-Host "📊 10. Testing Analytics Endpoint (Admin Only)..." -ForegroundColor Yellow
try {
    $analytics = Invoke-RestMethod -Uri "$baseUrl/admin/analytics" -Method GET -Headers $adminHeaders
    Write-Host "✅ Analytics access successful!" -ForegroundColor Green
    Write-Host "✅ Total Users: $($analytics.totalUsers)" -ForegroundColor Green
    Write-Host "✅ Active Users: $($analytics.activeUsers)" -ForegroundColor Green
    Write-Host "✅ Server Uptime: $($analytics.systemStats.serverUptime)" -ForegroundColor Green
} catch {
    Write-Host "❌ Analytics access failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎉 JWT Authentication API Demo Complete!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Summary of Features Demonstrated:" -ForegroundColor White
Write-Host "✅ JWT Token Generation and Validation" -ForegroundColor Green
Write-Host "✅ User Authentication (Login/Register)" -ForegroundColor Green
Write-Host "✅ Role-Based Authorization (User vs Admin)" -ForegroundColor Green
Write-Host "✅ Claims-Based Identity" -ForegroundColor Green
Write-Host "✅ Protected API Endpoints" -ForegroundColor Green
Write-Host "✅ Security Controls (Authentication & Authorization)" -ForegroundColor Green
Write-Host ""
Write-Host "Frontend Demo: http://localhost:5192/index.html" -ForegroundColor Cyan
Write-Host "API Documentation: http://localhost:5192/swagger" -ForegroundColor Cyan
