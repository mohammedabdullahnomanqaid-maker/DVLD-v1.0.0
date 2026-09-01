<div align="center">

# 🚗 DVLD — Driving & Vehicle License Department

### Driving & Vehicle License Department Management System

A desktop application developed with **C# and .NET Framework**, designed to manage the core entities and operations of a Driving & Vehicle License Department using a **Three-Tier Architecture**.

<br>

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-Desktop-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Architecture](https://img.shields.io/badge/Architecture-Three--Tier-0A66C2?style=for-the-badge)](#-architecture)

<br>

[📖 About](#-about-the-project) ·
[✨ Features](#-key-features) ·
[🏗️ Architecture](#️-architecture) ·
[🛠️ Technologies](#️-technologies) ·
[📂 Structure](#-project-structure) ·
[🚀 Setup](#-getting-started)

</div>

---

# 📖 About The Project

**DVLD — Driving & Vehicle License Department** is a desktop management application developed using **C#**, **.NET Framework 4.7.2**, and **Windows Forms**.

The project is structured using a **Three-Tier Architecture**, separating the application into:

- 🖥️ **Presentation Layer**
- ⚙️ **Business Logic Layer**
- 🗄️ **Data Access Layer**

The main purpose of the project is to organize a complete domain-oriented desktop application while applying **Object-Oriented Programming**, layered architecture, database communication, and reusable business/data-access components.

The solution is divided into three independent projects that communicate through clear responsibilities rather than placing the entire application inside a single project.

---

# 🎯 Project Goals

The project was developed with the following goals in mind:

- Apply **Object-Oriented Programming** in a real desktop application.
- Practice building a multi-project Visual Studio solution.
- Separate the user interface from business logic.
- Separate business logic from database operations.
- Build reusable business and data-access classes.
- Organize a complex domain into independent modules.
- Practice working with SQL Server through a dedicated Data Access Layer.
- Improve maintainability through layered architecture.
- Apply practical software development concepts to a real-world style system.

---

# ✨ Key Features

The project is organized around the following major functional areas.

## 👤 People Management

The system contains a dedicated People module responsible for working with person-related information.

### Includes

- Add and manage people records
- Find existing people
- Update person information
- Display person information
- Connect people with other system entities

---

## 👥 User Management

The system contains a dedicated Users module for handling application users.

### Includes

- Add users
- Find users
- Update user information
- Manage user-related data
- Connect users with their related person information

---

## 🚗 Drivers Management

The Drivers module provides the domain and data-access components required for driver-related operations.

### Includes

- Driver records
- Driver information
- Driver lookup
- Relationship between drivers and their related information

---

## 📝 Applications

The system contains a dedicated Applications module for application-related operations.

The Business Layer defines application-related entities and states, while the Data Access Layer provides the corresponding database operations.

### Application Types

The Business Layer includes application types for:

- New Applications
- Renewal Applications
- Replacement for Lost License
- Replacement for Damaged License
- Release of Detained License
- New International License
- Retake Test

### Application Status

Applications can have different states, including:

- `New`
- `Canceled`
- `Completed`

---

## 🧪 Tests & Test Appointments

The project contains dedicated modules for managing tests and test appointments.

### Includes

- Test types
- Tests
- Test appointments
- Test-related business logic
- Test-related data access

The Business Layer contains dedicated classes such as:

```text
clsTest
clsTestAppointment
clsTestTypes
