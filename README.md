# 🚀 MaxEndLabs

> An ASP.NET Core MVC e-commerce application for managing products,
> product variants and a shopping cart, built with layered architecture
> and Entity Framework Core.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple) 
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

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

**MaxEndLabs** is a multi-layered ASP.NET Core MVC web application that
represents a small online store. The system allows users to browse
products, view detailed information and manage a shopping cart, while
administrators can create, edit and manage products and product
variants.

The project is structured using separate layers for data access,
business logic and presentation in order to demonstrate clean
architecture, service abstraction and Entity Framework Core usage.

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
dotnet ef database update --project MaxEndLabs.Data --startup-project MaxEndLabs
```
### 4. Run the application

```bash
dotnet run --project MaxEndLabs
```
------------------------------------------------------------------------

## 📁 Project Structure

    MaxEndLabs.sln
    │
    ├── MaxEndLabs/
    ├── MaxEndLabs.Data/
    ├── MaxEndLabs.Data.Models/
    ├── MaxEndLabs.Services.Core/
    ├── MaxEndLabs.ViewModels/
    └── MaxEndLabs.GCommon

------------------------------------------------------------------------

## ✨ Features

-   User registration and login
-   Product catalog with categories
-   Product details with variants
-   Product and variant management
-   Shopping cart and checkout
-   Layered architecture

------------------------------------------------------------------------

## 💻 Usage

After starting the application:

1.  Register a new user from the Register page.
2.  Browse products from the main products page.
3.  Open a product to view its details and available variants.
4.  Add Products to your shopping cart.
5.  Open the shopping cart and proceed to checkout.

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
dotnet ef database update --project MaxEndLabs.Data --startup-project MaxEndLabs
```

------------------------------------------------------------------------

## ⚙️ Configuration

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=aspnet_MaxEndLabs_2026;Trusted_Connection=True;Encrypt=False;",
    "MaxEndLabsDbContextConnection": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

------------------------------------------------------------------------

## 🤝 Contributing

Fork the repository and create a pull request.

------------------------------------------------------------------------

## 📄 License

MIT License.

------------------------------------------------------------------------

## 📬 Contact

**Dimitar Karabashev** – [@your-github] (https://github.com/sg1345)

Project Link: [https://github.com/sg1345/MaxEndLabs](https://github.com/sg1345/MaxEndLabs)
