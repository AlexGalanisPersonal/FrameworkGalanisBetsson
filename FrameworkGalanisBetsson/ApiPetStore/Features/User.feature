@api
Feature: User Management API Testing
    As a system administrator
    I want to manage users through the API
    So that I can maintain user accounts in the pet store

   Scenario: Create a new user
        Given I have a new user with the following details
            | Username | FirstName | LastName | Email           | Password    | Phone      | UserStatus |
            | AlexG    | Alex      | Galanis  | user1@email.com | password123 | 0987654321 | 1          |
        When I send a request to create the user
        Then the user should be successfully created

    Scenario: Create multiple users with array
        Given I have created a list of users
            | Username | FirstName | LastName | Email           | Phone      |
            | AlexG1   | Alex      | Galanis  | user1@email.com | 1234567890 |
            | AlexG2   | Alex      | Galanis2 | user2@email.com | 0987654321 |
        When I create multiple users with an array
        Then the user should be successfully created

    Scenario: Update user information
        Given I have a new user with the following details
            | Username | FirstName | LastName | Email           | Password    | Phone      | UserStatus |
            | AlexG    | Alex      | Galanis  | user@email.com  | password123 | 1234567890 | 1          |
        When I send a request to create the user
        And I update the user's email to "newemail@email.com"
        Then the user's email should be updated to "newemail@email.com"

    Scenario: User login
        Given I have a new user with the following details
            | Username    | FirstName | LastName | Email           | Password    | Phone      | UserStatus |
            | AlexGalanis | Alex      | Galanis  | user@email.com  | password123 | 1234567890 | 1          |
        When I send a request to create the user
        And I login with username "AlexGalanis" and password "password123"
        Then the login should be successful

    @DeleteUser
    Scenario: Delete a user
        Given I have a new user with the following details
            | Username | FirstName | LastName | Email            | Password    | Phone      | UserStatus |
            | AlexG    | Alex      | Galanis  | user@email.com   | password123 | 1234567890 | 1          |
        When I send a request to create the user
        And I delete the user
        Then retrieving the user should return not found