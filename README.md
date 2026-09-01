🚗 DVLD — Driving & Vehicle License Department

«A desktop-based Driving & Vehicle License Department Management System built with C# and .NET, following a layered architecture to organize presentation, business logic, and data access.»

"C#" (https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
".NET" (https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
"WinForms" (https://img.shields.io/badge/WinForms-512BD4?style=for-the-badge&logo=windows&logoColor=white)
"SQL Server" (https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
"OOP" (https://img.shields.io/badge/OOP-Design-blue?style=for-the-badge)
"Architecture" (https://img.shields.io/badge/Architecture-3--Tier-success?style=for-the-badge)

---

📌 Overview

DVLD (Driving & Vehicle License Department) is a desktop management system developed using C# and .NET.

The project is designed around a layered architecture, separating the application's responsibilities into independent layers:

- 🎨 Presentation Layer
- 🧠 Business Logic Layer
- 💾 Data Access Layer

This separation makes the application easier to understand, maintain, extend, and debug while keeping the responsibilities of each layer clearly defined.

The current repository is organized as a Visual Studio solution containing the three main application layers.

---

✨ Core Modules

The project contains several modules representing the main operations of a driving-license management system.

👤 People Management

Handles information related to people within the system.

Relevant implementation exists in both the Presentation and Business/Data Access layers.

👨‍💼 Users

Provides functionality related to system users and user data.

The project contains dedicated business and data-access classes for users.

🚘 Drivers

Contains functionality related to driver records and driver management.

🪪 Local Driving License Applications

Provides the application's domain and data-access components for local driving-license applications.

🌍 International Licenses

Includes dedicated classes for handling international driving-license functionality.

📝 Tests & Test Appointments

The system contains modules for:

- Test types
- Tests
- Test appointments

These components are represented across the business and data-access layers.

📄 Licenses & License Classes

The project includes dedicated components for:

- Licenses
- License classes
- International licenses

---

🏗️ Architecture

The project follows a 3-Tier / Layered Architecture:

┌──────────────────────────────────────────┐
│          Presentation Layer              │
│              DVLD                        │
│                                          │
│  Forms • UI • User Interaction           │
└────────────────────┬─────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────┐
│        Business Logic Layer              │
│        DVLD_BussinseLayer                │
│                                          │
│  Business Rules • Validation • Logic     │
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
