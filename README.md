# EquipShare - Equipment Rental Platform

## 🚀 Project Overview

EquipShare is a web-based equipment rental platform built with ASP.NET Core MVC that connects equipment owners with renters in their local community. The platform enables users to rent tools, machinery, electronics, and other equipment at affordable rates while providing owners with an opportunity to earn from their underutilized assets.

### 🎯 Mission
"Rent Smarter. Share Equipment Easily" - Our mission is to create a trusted community marketplace that makes equipment rental accessible, affordable, and convenient for everyone.

## ✨ Features Implemented

### 🔐 User Authentication & Authorization
- **User Registration & Login** - Secure account creation and authentication
- **Session Management** - Persistent login sessions with secure logout
- **Profile Management** - User profile viewing and management

### 🏠 Home Page
- **Hero Section** - Attractive landing page with call-to-action buttons
- **Features Showcase** - Why choose EquipShare (Trust, Affordability, Community)
- **Featured Equipment** - Displays 3 latest available equipment items
- **Responsive Design** - Mobile-first approach with Bootstrap 5

### 🔍 Equipment Discovery
- **Browse Equipment** - Comprehensive equipment listing page
- **Advanced Search** - Search by name and description
- **Category Filtering** - Filter by equipment categories:
  - Construction & Tools
  - Outdoor & Gardening
  - Electronics & Tech
  - Events & Party Supplies
  - Miscellaneous
- **Sorting Options** - Sort by:
  - Most Recent
  - Price: Low to High
  - Price: High to Low

### 📋 Equipment Management
- **Add Equipment** - List new equipment with multiple image uploads
- **Edit Equipment** - Modify existing listings
- **Delete Equipment** - Remove listings with confirmation
- **My Equipment** - View and manage owned equipment
- **Image Management** - Upload up to 5 images per listing

### 🎫 Booking System
- **View Equipment Details** - Detailed equipment information
- **Booking Requests** - Submit rental requests
- **My Bookings** - Track booking history and status
- **Booking Management** - Approve/decline booking requests (owners)

### 👤 User Profile
- **Profile Viewing** - User profile information display
- **Account Management** - Update personal information

### 🎨 UI/UX Features
- **Responsive Design** - Works on desktop, tablet, and mobile
- **Modern UI** - Clean, professional interface with gradient themes
- **Interactive Elements** - Hover effects, smooth transitions
- **Image Galleries** - Multiple image support with preview
- **Loading States** - Proper loading and error handling

## 🛠 Technical Architecture

### Technology Stack
- **Backend**: ASP.NET Core 6.0+ MVC
- **Frontend**: Razor Views, Bootstrap 5, JavaScript
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: ASP.NET Core Identity with Session State
- **Image Handling**: Local file system storage
- **Icons**: Bootstrap Icons

### Project Structure
```
EquipShare/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── BookingController.cs
│   ├── EquipmentController.cs
│   ├── HomeController.cs
│   └── ProfileController.cs
├── Models/               # Entity models
│   ├── Booking.cs
│   ├── Category.cs
│   ├── Equipment.cs
│   └── User.cs
├── Services/             # Business logic layer
│   ├── BookingService.cs
│   ├── EquipmentService.cs
│   └── IBookingService.cs
├── Views/                # Razor views
│   ├── Account/         # Authentication views
│   ├── Booking/         # Booking management
│   ├── Equipment/       # Equipment CRUD
│   ├── Home/           # Landing pages
│   └── Shared/         # Layout and partial views
├── wwwroot/             # Static assets
│   ├── css/           # Custom styles
│   ├── js/            # Client-side scripts
│   └── images/        # Image assets
└── Utilities/           # Helper classes
```

## 🚀 Setup Instructions

### Prerequisites
- **Visual Studio 2022** or later
- **.NET 6.0 SDK** or later
- **SQL Server** (LocalDB or Express)
- **Git** for version control

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd EquipShare
   ```

2. **Open in Visual Studio**
   - Launch Visual Studio
   - Open the `EquipShare.csproj` file
   - Wait for NuGet packages to restore

3. **Database Setup**
   - Open Package Manager Console (PMC)
   - Run migrations:
     ```powershell
     Update-Database
     ```

4. **Configuration**
   - Update `appsettings.json` if needed:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Your-Connection-String"
       }
     }
     ```

5. **Build and Run**
   - Press `F5` or click "Run" button
   - Application will start on `https://localhost:5001` or `http://localhost:5000`

### 🌱 Seeding Initial Data (Optional)
If you want to populate sample data for testing:

```csharp
// In Package Manager Console
Add-Migration SeedData
Update-Database
```

## 👥 Team Members & Contributions

### Development Team

**Ridham Patel** - *Project Lead & Full-Stack Developer*
- Overall project architecture and design
- Backend development (Controllers, Services, Models)
- Database design and Entity Framework implementation
- User authentication and authorization system
- Equipment management system (CRUD operations)
- Booking system implementation
- Frontend development and UI/UX design
- Image upload and management functionality
- Search and filtering functionality
- Sort by functionality implementation
- Category management and optimization
- Home page featured equipment optimization
- GitHub repository setup and documentation

### 🎯 Individual Contributions Summary

| Feature Category | Implementation Status | Owner |
|------------------|----------------------|-------|
| **Authentication** | ✅ Complete | Ridham Patel |
| **Equipment CRUD** | ✅ Complete | Ridham Patel |
| **Booking System** | ✅ Complete | Ridham Patel |
| **Search & Filter** | ✅ Complete | Ridham Patel |
| **Image Management** | ✅ Complete | Ridham Patel |
| **Responsive UI** | ✅ Complete | Ridham Patel |
| **Database Design** | ✅ Complete | Ridham Patel |
| **API Integration** | ✅ Complete | Ridham Patel |

## 📊 Recent Updates & Improvements

### ✅ Latest Changes (v1.1.0)
1. **Footer Updates**
   - Updated contact mobile number to +91 7861088838
   - Updated Facebook profile link
   - Updated Instagram profile link
   - Removed Twitter integration
   - Added LinkedIn profile link

2. **Search Functionality Enhancement**
   - Implemented working sort by functionality
   - Added price sorting (Low to High, High to Low)
   - Added recent items sorting
   - Fixed form layout and visibility issues

3. **Category Management**
   - Removed "Vehicles & Transport" category
   - Removed "Sports & Recreation" category
   - Removed "Agriculture & Farming" category
   - Updated both browse and add equipment forms

4. **Home Page Optimization**
   - Reduced featured equipment from 6 to 3 items
   - Improved page load performance
   - Enhanced visual presentation

## 🔧 Development Guidelines

### Code Style
- Follow C# naming conventions
- Use async/await for database operations
- Implement proper error handling
- Add XML documentation for public methods

### Git Workflow
- Create feature branches for new development
- Use meaningful commit messages
- Regular pushes to main branch
- Tag releases with version numbers

### Testing
- Test all CRUD operations
- Verify responsive design on different devices
- Test image upload functionality
- Validate search and filter operations

## 📝 License

This project is developed for educational and portfolio purposes.

## 📞 Contact

For questions or support regarding this project, please contact:
- **Developer**: Ridham Patel
- **LinkedIn**: https://www.linkedin.com/in/ridham-patel-853072275/
- **Email**: ridhampately@gmail.com

---

⭐ **Star this repository if you found it helpful!**
