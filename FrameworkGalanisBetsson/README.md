# 🎯Test Automation Framework

A comprehensive end-to-end test automation framework that combines UI testing (Sauce Demo website) and API testing (Pet Store API) using Playwright, SpecFlow, and C#. The framework demonstrates modern testing practices and clean architecture principles.

## 📋Project Overview

The framework is divided into two main testing domains:

### 1. UI Testing (UISauceDemo)
Handles automated testing of the Sauce Demo e-commerce website with components:
- **Features**: BDD feature files for login, cart, and checkout flows
- **Pages**: Page Object Models for each webpage
- **StepDefinitions**: Implementation of BDD test steps
- **Utilities**: Helper classes for configuration, logging, navigation, and login

### 2. API Testing (ApiPetStore)
Manages automated testing of the Pet Store API with components:
- **Features**: BDD feature files for pet, store, and user endpoints
- **Helpers**: API client and request handling
- **Models**: Data models for API requests/responses
- **StepDefinitions**: Implementation of API test steps

## 🏗️Architecture Overview

### Core Components
1. **Test Organization**
```
├── ApiPetStore/
│   ├── Features/         # API test scenarios
│   ├── Helpers/         # API client and utilities
│   ├── Models/          # API data models
│   └── StepDefinitions/ # API test steps
├── UISauceDemo/
│   ├── Features/        # UI test scenarios
│   ├── Pages/          # Page Object Models
│   ├── StepDefinitions/# UI test steps
│   └── Utilities/      # Helper classes
│── Hooks/              # Test lifecycle management
└── Drivers/             #  Manages browser lifecycle using Playwright
```

2. **Framework Infrastructure**
- **Drivers**: Browser management using Playwright
- **Hooks**: Test lifecycle, dependency injection, and cleanup
- **Models**: Configuration and data models
- **Utilities**: Shared helper functions

## 🛠️Technical Implementation

### 1. UI Testing Architecture
- **Page Object Model**: Each page is represented by a class inheriting from BasePage
- **Step Definitions**: Implement BDD steps using page objects
- **Configuration**: Environment settings in appsettings.json
- **Navigation**: Centralized navigation management
- **Authentication**: Reusable login functionality

### 2. API Testing Architecture
- **API Client**: Wrapper for HTTP requests
- **Models**: Strongly-typed request/response objects
- **Step Definitions**: BDD implementation for API operations
- **Data Management**: Test data creation and cleanup

### 3. Core Framework Features
- **Dependency Injection**: Using SpecFlow's container
- **Configuration Management**: Using Microsoft.Extensions.Configuration
- **Logging**: Structured logging with Serilog
- **Screenshot Capture**: Automatic capture on test failure
- **Cross-browser Testing**: Support for multiple browsers
- **Clean Test Data**: Automatic cleanup after tests

## 💡Best Practices

### 1. Code Organization
- Clear separation between UI and API tests
- Reusable components and utilities
- Consistent naming conventions
- Strong typing and proper error handling

### 2. Test Design
- BDD approach with SpecFlow
- Independent test execution
- Clear test scenarios
- Comprehensive assertions
- Proper test cleanup

### 3. Framework Features
- Explicit waits for UI elements
- Screenshot capture on failure
- Structured logging
- Response validation for API tests
## ❓Why

### Why Playwright?

- **Modern Architecture**: Built for today's dynamic web applications
- **Auto-waiting**: Intelligent handling of dynamic elements
- **Cross-browser Support**: Chrome, Firefox, Safari, and Edge
- **Network Interception**: Built-in support for API mocking and interception
- **Trace Viewer**: Powerful debugging with step-by-step replay
- **Codegen**: Automatic test script generation
### Why SpecFlow?
- **Natural Language Tests**: Business-readable test scenarios using Gherkin syntax
- **Living Documentation**: Tests serve as up-to-date system documentation
- **Bridge Between Teams**: Facilitates communication between technical and non-technical stakeholders
- **Rich Ecosystem**: Strong .NET integration and extensive plugin support
- **Parallel Execution**: Built-in support for running tests concurrently
- **Data-Driven Testing**: Easy handling of test data through Examples and DataTables
- **Hooks & Context Injection**: Powerful test lifecycle management and dependency injection
- **Reusable Steps**: Share steps across multiple scenarios reducing code duplication

### Why Playwright & SpecFlow Together?
- **Modern Web Testing**: Playwright's modern architecture combined with SpecFlow's BDD approach
- **Cross-browser Validation**: Easy to test scenarios across different browsers
- **Auto-waiting**: Playwright's smart waiting mechanism makes SpecFlow steps more reliable
- **API & UI Testing**: Perfect combination for end-to-end testing covering both UI and API layers
- **Maintainable Framework**: Clean architecture with Page Object Model and step reusability
- **Debugging Features**: Playwright's trace viewer helps debug failed SpecFlow scenarios
- **Strong Typing**: Both tools work well with C#'s type system for robust test development
## 🚀Getting Started

### Prerequisites
```bash
dotnet restore
pwsh bin/Debug/net8.0/playwright.ps1 install
```
### Run Tests

```bash
dotnet test --framework net 8.0
```