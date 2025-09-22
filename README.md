# 🛒 ShopSmart

ShopSmart is a full-stack **e-commerce web application** built with:
- **Backend:** ASP.NET Core Web API + SQL Server
- **Frontend:** Angular

🚧 Project is under development. 🚧

---

## 🔐 Authentication & Role Management (Backend)

This diagram shows the structure of authentication and role management in the backend:

- `ApplicationUser` stores common user information (Email, Password, Name, etc.)
- `Customer` stores customer-specific details (Shipping Address, Date of Birth, etc.)
- `Vendor` stores vendor-specific details (Shop Name, Verification status, etc.)
- Roles are managed using **ASP.NET Identity** (`Admin`, `Vendor`, `User`)

```mermaid
erDiagram
    ApplicationUser {
        string Id PK
        string UserName
        string Email
        string PasswordHash
        string FirstName
        string LastName
        DateTime CreatedAt
        bool IsActive
    }
    Customer {
        int Id PK
        string UserId FK
        DateOfBirth DateTime
        DefaultShippingAddress string
    }
    Vendor {
        int Id PK
        string UserId FK
        ShopName string
        IsVendorVerified bool
    }
    
