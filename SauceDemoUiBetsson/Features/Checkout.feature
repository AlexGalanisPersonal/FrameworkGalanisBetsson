Feature: Checkout Process
    As a customer
    I want to complete the checkout process
    So that I can purchase my selected items

    Background:
        Given I am logged in as "standard_user"
        And I have the following items in cart:
          | Item Name              | Price  |
          | Sauce Labs Backpack    | $29.99 |
          | Sauce Labs Bike Light  | $9.99  |

    Scenario: Complete checkout with valid information
        Given I navigate to the checkout page
        When I enter the following customer details:
          | First Name | Last Name | Zip Code |
          | Alex       | Galanis   | 15772    |
        And I proceed to checkout overview
        Then the total amount should be "$43.18" including tax
        When I complete the checkout
        Then I should see the order confirmation

    Scenario: Validate checkout information requirements
        Given I navigate to the checkout page
        When I try to continue with empty fields
        Then I should see the error message "Error: First Name is required"
        When I enter only the following details:
          | Field      | Value     |
          | First Name | Alex      |
        And I try to continue
        Then I should see the error message "Error: Last Name is required"
        When I enter only the following details:
          | Field      | Value     |
          | First Name | Alex      |
          | Last Name  | Galanis   |
        And I try to continue
        Then I should see the error message "Error: Postal Code is required"

    Scenario: Verify item details on checkout summary
        Given I navigate to the checkout page
        When I enter valid customer information
        And I proceed to checkout overview
        Then I should see the following item details:
          | Item Name             | Quantity | Price  |
          | Sauce Labs Backpack   | 1        | $29.99 |
          | Sauce Labs Bike Light | 1        | $9.99  |
        And the subtotal should be "$39.98"
        And the tax should be "$3.20"
        And the total should be "$43.18"

    Scenario: Cancel checkout process
        Given I navigate to the checkout page
        When I enter valid customer information
        And I proceed to checkout overview
        And I click cancel
        Then I should be returned to the inventory page
        And my cart should still contain 2 items