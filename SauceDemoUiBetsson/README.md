# 🎯Sauce Demo UI Test Automation Framework

A comprehensive end-to-end UI test automation framework for the Sauce Demo application using Playwright, SpecFlow, and C#.

## 💡Technologies Used

- **Playwright**: Modern browser automation library
- **SpecFlow**: BDD framework for .NET
- **C#**: Programming language
- **NUnit**: Test framework
- **.NET 8+**: Framework version

## 🛠️Project Structure

```
├── Features/
│   ├── Cart.feature
│   ├── Checkout.feature
│   └── Login.feature
├── Pages/
│   ├── BasePage.cs
│   ├── CartPage.cs
│   ├── CheckoutPage.cs
│   └── LoginPage.cs
├── StepDefinitions/
│   ├── CartSteps.cs
│   ├── CheckoutSteps.cs
│   └── LoginSteps.cs
├── Models/
│   └── CartItemDetails.cs
├── Hooks/
│   ├── TestHooks.cs
│   └── ExternalReportHooks.cs
├── Utilities/
│   └── LoggerConfig.cs
└── appsettings.json
```

## 🔍Features Covered

1. **Login Functionality**
    - Successful login with valid credentials
    - Failed login with invalid credentials
    - Failed login with locked out user

2. **Shopping Cart Functionality**
    - Add single item to cart
    - Add multiple items to cart
    - Remove item from cart

3. **Checkout Process**
    - Complete checkout with valid information
    - Validate checkout information requirements
    - Verify item details on checkout summary
    - Cancel checkout process

## 🛠️Setup Instructions

1. Clone the repository
2. Install prerequisites:
    - .NET 8 SDK
    - JetBrains Rider IDE or VS
   - Playwright browsers: `pwsh bin/Debug/net8.0/playwright.ps1 install`
3. Restore NuGet packages:
```bash
dotnet restore
```
```xml
<ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.0"/>
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0"/>
    <PackageReference Include="Microsoft.Playwright" Version="1.41.2" />
    <PackageReference Include="Microsoft.Playwright.NUnit" Version="1.27.1"/>
    <PackageReference Include="NUnit" Version="4.0.1" />
    <PackageReference Include="NUnit3TestAdapter" Version="4.5.0"/>
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
    <PackageReference Include="SpecFlow.NUnit" Version="3.9.74" />
    <PackageReference Include="SpecFlow.Plus.LivingDocPlugin" Version="3.9.57" />
</ItemGroup>
```
4. Build the project:
```bash
dotnet build
```

## 🚀Running Tests

### Command Line
```bash
dotnet test --framework net8.0
```

### JetBrains Rider
1. Open Unit Tests window
2. Right-click on the test/folder and select 'Run' or use the play button
3. You can also run tests directly from the feature files or test classes

## ⚙️Configuration

The `appsettings.json` file contains test configuration:

```json
{
  "TestSettings": {
    "BaseUrl": "https://www.saucedemo.com",
    "DefaultTimeout": 30000,
    "Browser": "chromium",
    "Headless": true
  }
}
```

## 💻Framework Features

1. **Page Object Model**
    - Maintainable and reusable page objects
    - Base page with common functionality
    - Centralized element locators

2. **BDD Approach**
    - Feature files in Gherkin syntax
    - Step definitions mapping
    - Scenario context sharing

3. **Robust Test Design**
    - Explicit waits
    - Error handling
    - Clear assertions
    - Clean code practices

4. **Advanced Debugging Capabilities**
    - Automatic screenshot capture on test failure
    - Comprehensive logging system using Serilog
    - Detailed test execution logs with timestamps
    - Console and file logging support

## 📄Architecture Overview

### Core Design Principles

1. **Separation of Concerns**
    - Clear separation between test specifications (Features), test logic (Step Definitions), and page interactions (Page Objects)
    - Each component has a single responsibility and purpose
    - Modular design allows for easy maintenance and extensions

2. **Page Object Model Implementation**
    - BasePage class provides common functionality and error handling
    - Each page object encapsulates its own selectors and methods
    - Strong typing and clear method names improve maintainability
    - Reusable components reduce code duplication

3. **BDD Implementation**
    - Features written in Gherkin syntax for better readability
    - Step definitions map directly to page object methods
    - Scenario context allows for data sharing between steps
    - Clear separation between test specifications and implementation

4. **Test Data Management**
    - Test data defined in feature files for better maintainability
    - Data tables used for complex test data
    - Clear separation between test data and test logic

5. **Error Handling and Debugging**
    - Comprehensive logging using Serilog
    - Automatic screenshot capture on test failure
    - Detailed error messages and stack traces
    - Easy to debug and maintain

### Framework Components

1. **Base Components**
    - BasePage: Abstract class providing common functionality
    - TestHooks: Manages test lifecycle and setup/teardown
    - LoggerConfig: Configures logging system

2. **Page Objects**
    - Encapsulate page-specific functionality
    - Handle element interactions
    - Provide high-level business methods

3. **Step Definitions**
    - Map Gherkin steps to code
    - Coordinate between feature files and page objects
    - Handle test assertions

4. **Support Infrastructure**
    - Logging system for debugging
    - Screenshot capture for failure analysis
    - Configuration management

### Design Decisions

1. **Choice of Technologies**
    - Playwright: Modern, reliable browser automation
    - SpecFlow: Industry-standard BDD framework
    - Serilog: Flexible, structured logging
    - NUnit: Robust test framework

2. **Error Handling Strategy**
    - Automatic screenshot capture on failure
    - Detailed logging of all actions
    - Clear error messages
    - Stack traces for debugging

3. **Maintainability Features**
    - Centralized configuration
    - Reusable components
    - Clean code practices
    - Comprehensive documentation

## ✅Best Practices Implemented

1. **Code Organization**
    - Clear folder structure
    - Separation of concerns
    - Reusable components

2. **Test Reliability**
    - Proper element waits
    - Robust selectors
    - Error handling

3. **Maintainability**
    - DRY principles
    - Page Object Pattern
    - Base page abstraction