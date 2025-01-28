Feature: Login Functionality
    As a user of the sauce demo website
    I want to be able to log in
    So that I can access the inventory

    Scenario: Successful login with valid credentials
        Given I am on the login page
        When I log in as "standard"
        Then I should be redirected to the inventory page

    Scenario: Failed login with invalid credentials
        Given I am on the login page
        When I log in as "invalid"
        Then I should see an error message "Epic sadface: Username and password do not match any user in this service"

    Scenario: Failed login with locked out user
        Given I am on the login page
        When I log in as "locked"
        Then I should see an error message "Epic sadface: Sorry, this user has been locked out."