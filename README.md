# 🚀 MaxEndLabs

> A layered ASP.NET Core MVC e-commerce application supporting product 
> and variant management, shopping cart functionality, order processing, 
> and secure payments via Stripe, built with Entity Framework Core.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple) 
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

------------------------------------------------------------------------

## Live Demo

**🔗 [MaxEndLabs Store on Render](https://maxendlabstest.onrender.com)**  

**Demo Credentials:**
 - Email: admin@labs.com
 - Password: admin

------------------------------------------------------------------------

## 📋 Table of Contents

-   [About the Project](#about-the-project)
-   [Technologies Used](#technologies-used)
-   [Prerequisites](#prerequisites)
-   [Getting Started](#getting-started)
-   [Project Structure](#project-structure)
-   [Features](#features)
-   [Usage](#usage)
-   [Database Setup](#database-setup)
-   [Configuration](#configuration)
-   [Contributing](#contributing)
-   [License](#license)
-   [Contact](#contact)

------------------------------------------------------------------------

## 📖 About the Project

MaxEndLabs is a layered ASP.NET Core (.NET 8) MVC web application that 
simulates a modern e-commerce platform. It enables users to browse 
products, explore detailed product variants, manage a personalized 
shopping cart, and place orders, while administrators can manage the 
product catalog through full CRUD operations and order control.

The application is built using a clean, multi-layered architecture, 
separating the presentation layer (MVC), business logic (services), and 
data access (Entity Framework Core). This structure promotes
maintainability, scalability, and testability while demonstrating best
practices such as service abstraction, dependency injection, and 
Fluent API configurations.

The project also integrates ASP.NET Core Identity for authentication and
user management, providing a solid foundation for role-based access 
control and secure user interactions.

------------------------------------------------------------------------

## 🛠️ Technologies Used

  Technology              Version   Purpose
  ----------------------- --------- ------------------------------------
  ASP.NET Core MVC        8.0       Web framework
  Entity Framework Core   8.0       ORM / database access
  ASP.NET Core Identity   8.0       Authentication and user management
  SQL Server              --        Application database
  Bootstrap               5.x       Frontend styling
  Razor Views             --        Server-side UI rendering

------------------------------------------------------------------------

## ✅ Prerequisites

-   .NET SDK 8.0+
-   Visual Studio 2022 or VS Code
-   SQL Server (LocalDB or full SQL Server)
-   Git

------------------------------------------------------------------------

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

```bash
git clone https://github.com/sg1345/MaxEndLabs
cd MaxEndLabs
```
### 2. Restore dependencies

```bash
dotnet restore
```
### 3. Apply database migrations
Make sure the connection string is correct and run:
```bash
dotnet ef database update --project MaxEndLabs.Data --startup-project MaxEndLabs.Web
```
### 4. Run the application

```bash
dotnet run --project MaxEndLabs
```
------------------------------------------------------------------------

## 📁 Project Structure

    MaxEndLabs.sln
    │
    ├── MaxEndLabs.Web/
    ├── MaxEndLabs.Data/
    ├── MaxEndLabs.Data.Models/
    ├── MaxEndLabs.Services.Core/
    ├── MaxEndLabs.ViewModels/
    └── MaxEndLabs.GCommon

------------------------------------------------------------------------

## ✨ Features

- **Auth & 2FA:** User registration, login, and Google Authenticator (QR code) support.
- **Product Catalog:** Categories, search, and detailed variants.
- **Shopping Cart:** Real-time cart management and Stripe payment integration.
- **Admin Dashboard:** Full CRUD for products/variants and order status management.
- **Bot Protection:** Integrated Google reCAPTCHA for secure forms.
- **Architecture:** Multi-layered project structure with EF Core Code-First

------------------------------------------------------------------------

## 💻 Usage

### Account & Security
* **Access:** Login/Register is required to shop.
* **User Manager:** All users can update profiles and enable **2FA** (Authenticator App/QR Code) for 6-digit security codes.

---

### 👤 Customer Workflow
1. **Shop:** Browse the catalogue and select product.
2. **Cart:** Add, or remove items (only available when logged in).
3. **Checkout:** Create an order and pay securely via **Stripe**.
4. **Orders:** Track personal order history and status on the homepage.

---

### 🛡️ Administrator Workflow
* **Product Management:** Full **CRUD** (Create/Edit/Delete) for products and variants.
* **Order Management:** View all customer orders and update status.
* **Catalogue:** Quick link in the header to view products as they appear to users.
* *Note: Admins cannot use the shopping cart or make purchases.*

---

### 🚦 Quick Access Guide
| Feature | Guest | Customer | Admin |
| :--- | :---: | :---: | :---: |
| Browse Products | ✅ | ✅ | ✅ |
| Shopping Cart | ❌ | ✅ | ❌ |
| Order/Pay | ❌ | ✅ | ❌ |
| Product CRUD | ❌ | ❌ | ✅ |
| Manage 2FA | ❌ | ✅ | ✅ |

### 🔐 Administrative access

**Creating, editing and deleting** products and product variants is
restricted to users with the **Admin role**.

The application contains a **pre-seeded administrator account**. To access
the product and variant management functionality, log in with:

-   **Email: admin@labs.com**
-   **Password: admin**

The admin account is seeded automatically when the application starts.

Only users with the Admin role can create, edit and delete products and
product variants.

------------------------------------------------------------------------

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=aspnet_MaxEndLabs_2026;Trusted_Connection=True;Encrypt=False;"
  }
```

To create and seed the database:

``` bash
dotnet ef database update --project MaxEndLabs.Data --startup-project MaxEndLabs.Web
```

------------------------------------------------------------------------

## ⚙️ Configuration

``` json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=aspnet_MaxEndLabs_2026_Test2;Trusted_Connection=True;Encrypt=False;",
    "PostgresConnection": "Host=dpg-d79vb4k50q8c73b967s0-a;maxendlabs_dbnewgen;Username=maxendlabs_dbnewgen_user;Password=oN6X9w2Qs5l0RCTxj1xRaCHnGLxMLc4I;Port=5432;SSL Mode=Require;Trust Server Certificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "GoogleReCaptcha": {
    "SiteKey": "",
    "SecretKey": ""
  },
  "Stripe": {
    "SecretKey": "",
    "PublishableKey": ""
  }
}
```

------------------------------------------------------------------------

## 🔑 Secret Keys Configuration

Please set the following Environment Variables in your IDE 
(Visual Studio/VS Code) before running the project:

| Environment Variable Name | Value |
| :--- | :--- |
| **Stripe__SecretKey** | `sk_test_51TBnryPsf4pfhoRSGR5vAM4TuqXfs9O8eQ7KjSBX2ut13bgmq40TSMhZuHQhTLzBMm5tvPgj0i0Qqjc3S8swM4Nj00m4bXPAAA` |
| **Stripe__PublishableKey** | `pk_test_51TBnryPsf4pfhoRSXTAZ7CxTrn1Nt9fQgOBcdL3jPKXbJl05iu5uzEy5z8ygc2fOek7EQq4875Id0HpkcN42oT5h00ilFIILSf` |
| **GoogleReCaptcha__SiteKey** | `6Lf3BpQsAAAAAFXo0dIMuCEakNWG8dCNenga8TmG` |
| **GoogleReCaptcha__SecretKey** | `6Lf3BpQsAAAAANp82boiFqoLLaBtuMUzy2NF9Grz` |

------------------------------------------------------------------------

## 🤝 Contributing

Fork the repository and create a pull request.

------------------------------------------------------------------------

## 📄 License

MIT License.

------------------------------------------------------------------------

## 📬 Contact

**Dimitar Karabashev** – [https://github.com/sg1345]

Project Link: [https://github.com/sg1345/MaxEndLabs](https://github.com/sg1345/MaxEndLabs)
