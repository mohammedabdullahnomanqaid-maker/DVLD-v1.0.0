<div align="center">

# 🚗 DVLD

### Driving & Vehicle License Department

A desktop management system built with **C# and .NET**, designed with a **layered architecture** that separates the Presentation, Business Logic, and Data Access responsibilities.

<br>

[![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WinForms](https://img.shields.io/badge/WinForms-512BD4?style=flat-square&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![OOP](https://img.shields.io/badge/OOP-Design-0A66C2?style=flat-square)](#)
[![Architecture](https://img.shields.io/badge/Architecture-3--Tier-success?style=flat-square)](#)

<br>

[📖 Overview](#-overview) ·
[🏗️ Architecture](#️-architecture) ·
[🧩 Modules](#-system-modules) ·
[🗂️ Structure](#️-project-structure) ·
[🚀 Getting Started](#-getting-started)

</div>

---

## 📌 Overview

**DVLD (Driving & Vehicle License Department)** is a desktop management system developed using **C# and .NET**.

The project is designed around a **3-Tier / Layered Architecture**, separating the application's responsibilities into three main layers:

- 🎨 **Presentation Layer** — User interface and user interaction
- 🧠 **Business Logic Layer** — Business rules and application logic
- 💾 **Data Access Layer** — Database communication and data operations

This separation of responsibilities helps keep the application organized, maintainable, and easier to extend.

---

## ✨ System Modules

The application is organized into several functional areas related to driving and vehicle license management.

| Module | Description |
|:--|:--|
| 👤 **People** | Manage people-related information |
| 👥 **Users** | Manage system users |
| 🚘 **Drivers** | Manage driver records |
| 📋 **Applications** | Manage application-related operations |
| 🪪 **Licenses** | Manage driving licenses and license classes |
| 🌍 **International Licenses** | Handle international license operations |
| 📝 **Tests** | Manage tests and test appointments |
| 🔐 **Login** | Application login and access entry |
| 🌐 **Countries** | Manage country-related data |

---

## 🏗️ Architecture

The project follows a **3-Tier / Layered Architecture**.

```mermaid
flowchart TD

    UI["🎨 Presentation Layer<br/>DVLD"]

    BL["🧠 Business Logic Layer<br/>DVLD_BussinseLayer"]

    DAL["💾 Data Access Layer<br/>DVLD_DataAccessLayer"]

    DB[("🗄️ Database")]

    UI --> BL
    BL --> DAL
    DAL --> DB│  Business Rules • Validation • Logic     │
└────────────────────┬─────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────┐
│          Data Access Layer               │
│        DVLD_DataAccessLayer              │
│                                          │
│  Database Communication • Data Access    │
└────────────────────┬─────────────────────┘
                     │
                     ▼
              ┌─────────────┐
              │  Database   │
              └─────────────┘

Why Layered Architecture?

Separating the system into layers provides several advantages:

- Separation of Concerns
- Easier maintenance
- Better code organization
- Reduced coupling between components
- Easier debugging
- Better scalability
- Clear responsibility for each part of the application

---

📂 Project Structure

DVLD-v1.0.0/
│
├── DVLD/
│   ├── Applications/
│   ├── Drivers/
│   ├── Global Class/
│   ├── License/
│   ├── Login/
│   ├── People/
│   ├── Properties/
│   ├── Resources/
│   ├── Tests/
│   ├── Users/
│   │
│   ├── frmDashboard.cs
│   ├── frmDashboard.Designer.cs
│   ├── Program.cs
│   └── PresentationLayer.csproj
│
├── DVLD_BussinseLayer/
│   ├── clsApplications.cs
│   ├── clsApplicationsTypes.cs
│   ├── clsBPeople.cs
│   ├── clsCountry.cs
│   ├── clsDetaind.cs
│   ├── clsDrivers.cs
│   ├── clsInternationalLicense.cs
│   ├── clsLicense.cs
│   ├── clsLicenseClass.cs
│   ├── clsLocalDrivingLicenseApplication.cs
│   ├── clsTest.cs
│   ├── clsTestAppointment.cs
│   ├── clsTestTypes.cs
│   ├── clsUsers.cs
│   └── BussinseLayer.csproj
│
├── DVLD_DataAccessLayer/
│   ├── clsApllicationTypesData.cs
│   ├── clsApplicationsData.cs
│   ├── clsConnectionString.cs
│   ├── clsCountryData.cs
│   ├── clsDatabaseInitializer.cs
│   ├── clsDetaindData.cs
│   ├── clsDriversData.cs
│   ├── clsInternationalLicenseData.cs
│   ├── clsLicenseClassData.cs
│   ├── clsLicenseData.cs
│   ├── clsLocalDrivingLicenseApplicationData.cs
│   ├── clsPeople.cs
│   ├── clsTestAppointmentData.cs
│   ├── clsTestData.cs
│   ├── clsTestTypesData.cs
│   ├── clsUserData.cs
│   └── DataAccessLayer.csproj
│
├── DVLD.sln
└── .gitignore

The repository structure currently reflects this separation between the Presentation, Business Logic, and Data Access projects.

---

🧩 Main Components

Component| Responsibility
"DVLD"| Presentation and user interface
"DVLD_BussinseLayer"| Business logic and domain operations
"DVLD_DataAccessLayer"| Database and data-access operations
"DVLD.sln"| Visual Studio solution

---

🛠️ Technologies & Concepts

Development

- C#
- .NET
- Windows Forms
- Visual Studio

Programming Concepts

- Object-Oriented Programming (OOP)
- Encapsulation
- Classes & Objects
- Layered Architecture
- Separation of Concerns
- Reusable Components

Architecture

- 3-Tier Architecture
- Presentation Layer
- Business Logic Layer
- Data Access Layer

Database

The Data Access Layer contains dedicated classes responsible for database-related operations and connection management, including "clsConnectionString" and "clsDatabaseInitializer".

---

🚀 Getting Started

1. Clone the Repository

git clone https://github.com/mohammedabdullahnomanqaid-maker/DVLD-v1.0.0.git

2. Open the Solution

Open:

DVLD.sln

using Visual Studio.

3. Restore Dependencies

Allow Visual Studio to restore the required project dependencies.

4. Configure the Database

Before running the application, make sure the database configuration matches your local SQL Server environment.

Check the Data Access Layer for the connection configuration:

DVLD_DataAccessLayer/
└── clsConnectionString.cs

5. Build the Solution

Build the complete solution from Visual Studio:

Build → Build Solution

6. Run the Application

Start the application from the Presentation Layer.

---

🔐 Application Areas

The project is organized around several functional areas visible in its implementation structure:

Login
│
├── Dashboard
│
├── People
│
├── Users
│
├── Drivers
│
├── Applications
│
├── Local Driving License
│
├── Tests
│
├── Licenses
│
└── International Licenses

The Presentation Layer contains dedicated folders for areas such as Login, People, Drivers, Applications, License, Tests, and Users.

---

🎯 Project Goals

The main goal of this project is to build a structured desktop application while applying practical software-development concepts rather than placing all application logic inside a single project.

The project demonstrates how a relatively complex desktop system can be divided into clear responsibilities:

User Interface
      ↓
Business Logic
      ↓
Data Access
      ↓
Database

This approach helps demonstrate practical understanding of:

- Software architecture
- Object-oriented programming
- Database interaction
- Business logic organization
- Multi-layer application design
- Maintainable code structure

---

📚 What I Learned

Working on this project provided practical experience with:

- Designing a multi-layer desktop application
- Applying Object-Oriented Programming
- Separating UI from business logic
- Separating business logic from database operations
- Organizing a large C# solution
- Working with SQL Server through a dedicated Data Access Layer
- Building reusable classes
- Structuring domain-specific modules
- Managing a project using Git and GitHub

---

📈 Version

"v1.0.0"

This repository represents the first version of the DVLD project.

DVLD
└── v1.0.0

Future versions can build upon this foundation by introducing additional functionality, refinements, and architectural improvements.

---

🗺️ Future Improvements

Possible future improvements include:

- Improving the user interface and overall UX
- Further refactoring and code cleanup
- Expanding validation and error handling
- Improving database performance
- Adding more comprehensive testing
- Enhancing documentation
- Introducing additional application features

---

👨‍💻 Author

Mohammed Abdullah Noman Qaid Mohammed

Computer Science Student
Taiz University

Connect With Me

- 💼 LinkedIn: "@dev-moh-noman" (https://www.linkedin.com/in/dev-moh-noman/)
- 📧 Email: "mohammedabdullahnomanqaid@gmail.com"

---

⭐ Support

If you find this project useful or interesting, consider giving the repository a ⭐ on GitHub.

Your feedback and suggestions are always welcome.

---

📄 License

Please refer to the repository for the applicable license information.

 
### 📌 Repository

[View DVLD on GitHub](https://github.com/mohammedabdullahnomanqaid-maker/DVLD-v1.0.0)

---

<p align="center">
  Built with C# • .NET • OOP • Layered Architecture
</p>

<p align="center">
  <b>DVLD — Driving & Vehicle License Department</b>
</p>

هذا الإصدار مبني على **البنية الفعلية الموجودة في الريبو**؛ مثل أسماء المشاريع والملفات والـ modules الموجودة، وليس على تخمينات عن المشروع.

**ملاحظة مهمة:** تعمدت عدم وضع ادعاءات مثل *CRUD، ADO.NET، Stored Procedures، Authentication، SQL Server* كـ Features مؤكدة إلا بالقدر الذي يظهر من بنية المشروع؛ لأن هدفك أن يكون الـ README احترافيًا **وصادقًا مع المشروع**، وليس README مليئًا بمميزات غير موجودة.
