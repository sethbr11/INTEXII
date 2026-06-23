# AuroraBricks (INTEX II)

AuroraBricks is an ASP.NET Core MVC e-commerce platform for buying and selling LEGO sets. Designed with modern web practices, it features user personalization, administrative dashboards, a machine learning fraud detection engine, and strong security defaults.

---

## 🚀 Key Features

* **Lego Shop Catalog**: Browse LEGO sets filtered dynamically by category (e.g., Star Wars, Technic, Creator) and brick color, with client-side/server-side pagination.
* **Shopping Cart & Checkout**: Interactive shopping cart using session-based storage.
* **Personalized Recommendations**: A collaborative filtering recommendation engine that displays customized LEGO sets for users based on their search/order history.
* **Administrative Control Panel**: Full CRUD operations for managing products and user accounts, and checking flagged transactions using a custom sidebar layout.
* **ONNX Machine Learning Model**: An integrated ML model (`wwwroot/fraud_model2.onnx`) running on ML.NET to analyze checkout details (amount, time, country) and classify transactions as *Fraud* or *Not Fraud* in real time.
* **Security Hardening**:
  * Strict Content Security Policy (CSP) headers.
  * HSTS (HTTP Strict Transport Security) configuration.
  * Identity password complexity enforcement (13-character minimum, uppercase, lowercase, numbers, and special characters).
  * Dev-mode SQLite configurations for local testing.

---

## 🛠️ Technology Stack

* **Backend Framework**: ASP.NET Core MVC (.NET 8.0)
* **ORM & Database**: Entity Framework Core with SQLite (local database providers)
* **Authentication**: ASP.NET Core Identity (with seeded default roles & users)
* **Machine Learning**: Microsoft.ML & Microsoft.ML.OnnxRuntime
* **Frontend**: Bootstrap 5, FontAwesome 6, and custom styling stylesheets

---

## 💻 Local Setup & Run Instructions

Follow these steps to set up and run the project locally on your machine:

### Prerequisites
Make sure you have the **.NET 8.0 SDK** and the **EF Core CLI tool** installed:
```bash
dotnet --version
dotnet ef --version
```
*(If EF Core is not installed, install it with: `dotnet tool install --global dotnet-ef`)*

### 1. Compile the Project
Build the application to restore all packages and compile code:
```bash
dotnet build
```

### 2. Initialize and Seed the Database
Run migrations to create and seed both the main application store database and the Identity authentication databases:
```bash
# Create the local shopping database and seed items
dotnet ef database update --context IntexW24datasetContext

# Create the authentication schema database
dotnet ef database update --context ApplicationDbContext
```

### 3. Run the Application
Start the Kestrel development server:
```bash
dotnet run
```

The application will launch. Open your browser and navigate to:
👉 **[http://localhost:5224](http://localhost:5224)**

---

## 🔑 Seeded Demo Credentials

To log in and test out the various features of the application, use the following pre-configured credentials:

### 👤 Administrator Account
Use this to access the **Manage Products**, **Manage Users**, and **Review Orders** dashboards.
* **Email**: `admin@aurorabricks.com`
* **Password**: `IntexDemo123456!`

### 👥 Standard Customer Account
* **Email**: `user@aurorabricks.com`
* **Password**: `IntexDemo123456!`

### 🎯 Personalized Recommendations
On the homepage, click **"Explore YOUR Recommendation!"**, and input the following username in the textbox:
* **Username**: `demo`