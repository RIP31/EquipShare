# Contributing to EquipShare

Thank you for your interest in contributing to EquipShare! This document provides guidelines for contributing to our equipment rental platform.

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022 or later
- .NET 6.0 SDK or later
- SQL Server (LocalDB or Express)
- Git

### Development Setup
1. Fork the repository
2. Clone your fork: `git clone <your-fork-url>`
3. Open `EquipShare.csproj` in Visual Studio
4. Run `Update-Database` in Package Manager Console
5. Press F5 to start development

## 📝 Contribution Process

### 1. Choose an Issue
- Check existing issues in the repository
- Create a new issue if needed
- Discuss your approach with the team

### 2. Create a Feature Branch
```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/issue-description
```

### 3. Make Your Changes
- Follow the existing code style
- Add tests if applicable
- Update documentation if needed
- Ensure all tests pass

### 4. Commit Your Changes
```bash
git add .
git commit -m "Add: brief description of changes"
```

### 5. Push and Create Pull Request
```bash
git push origin feature/your-feature-name
```

## 💻 Development Guidelines

### Code Style
- Use C# naming conventions
- Follow async/await patterns
- Add XML documentation for public methods
- Use meaningful variable names

### Database Changes
- Create migrations for schema changes
- Test migrations thoroughly
- Update model classes accordingly

### UI Changes
- Test responsive design
- Follow Bootstrap 5 conventions
- Ensure accessibility standards

## 🐛 Reporting Bugs

1. Check if the bug is already reported
2. Create a new issue with:
   - Clear description
   - Steps to reproduce
   - Expected vs actual behavior
   - Environment details

## ✨ Feature Requests

1. Create an issue describing the feature
2. Provide use cases and benefits
3. Discuss implementation approach
4. Submit a pull request if approved

## 📊 Project Structure

```
EquipShare/
├── Controllers/     # Request handling
├── Models/         # Data models
├── Services/       # Business logic
├── Views/          # UI components
├── wwwroot/        # Static assets
└── Utilities/      # Helper classes
```

## 🔧 Common Commands

```bash
# Update database after model changes
Update-Database

# Create new migration
Add-Migration MigrationName

# Run tests
dotnet test

# Build project
dotnet build
```

## 📞 Contact

- **Project Lead**: Ridham Patel
- **LinkedIn**: https://www.linkedin.com/in/ridham-patel-853072275/

---

Thank you for contributing to EquipShare! 🎉