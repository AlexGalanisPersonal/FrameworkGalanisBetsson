Feature: Shopping Cart Functionality
As a logged in user
I want to be able to add items to cart
So that I can purchase them later

    Scenario: Add single item to cart
        Given I am logged in as "standard_user"
        When I add "Sauce Labs Backpack" to the cart
        Then the cart count should be 1

    Scenario: Add multiple items to cart
        Given I am logged in as "standard_user"
        When I add "Sauce Labs Backpack" to the cart
        And I add "Sauce Labs Bike Light" to the cart
        Then the cart count should be 2

    Scenario: Remove item from cart
        Given I am logged in as "standard_user"
        And I have added "Sauce Labs Backpack" to the cart
        When I remove "Sauce Labs Backpack" from the cart
        Then the cart should be empty