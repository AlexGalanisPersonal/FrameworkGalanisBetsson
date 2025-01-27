Feature: Login Functionality
As a user of the sauce demo website
I want to be able to log in
So that I can access the inventory

    Scenario: Successful login with valid credentials
        Given I am on the login page
        When I enter valid username "standard_user" and password "secret_sauce"
        Then I should be redirected to the inventory page

    Scenario: Failed login with invalid credentials
        Given I am on the login page
        When I enter invalid username "invalid_user" and password "invalid_pass"
        Then I should see an error message

    Scenario: Failed login with locked out user
        Given I am on the login page
        When I enter locked out username "locked_out_user" and password "secret_sauce"
        Then I should see a locked out error message