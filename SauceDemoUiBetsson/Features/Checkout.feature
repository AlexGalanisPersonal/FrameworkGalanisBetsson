Feature: Checkout Process
    As a customer
    I want to complete the checkout process
    So that I can purchase my selected items

    Background:
        Given I am logged in as "standard"
        And I have added items to cart:
            | Item Name             |
            | Sauce Labs Backpack   |
            | Sauce Labs Bike Light |

    Scenario: Fill checkout form successfully
        Given I navigate to the checkout page
        When I enter the following customer details:
          | First Name | Last Name | Zip Code |
          | Alex       | Galanis   | 15772    |
        Then I proceed to checkout overview successfully

    Scenario: Verify tax calculation
        Given I navigate to the checkout page
        When I complete checkout information
        Then the tax amount should be 8% of the subtotal

    Scenario: Verify total amount calculation
        Given I navigate to the checkout page
        When I complete checkout information
        Then the total should be the sum of subtotal and tax

    Scenario: Complete order successfully
        Given I am on the checkout overview page
        When I complete the checkout
        Then I should see the order confirmation

    Scenario: Validate empty first name
        Given I navigate to the checkout page
        When I proceed with empty fields
        Then I should see the error message "Error: First Name is required"

    Scenario: Validate empty last name
        Given I navigate to the checkout page
        When I fill in "First Name" with "Alex"
        And I proceed to checkout overview
        Then I should see the error message "Error: Last Name is required"

    Scenario: Validate empty postal code
        Given I navigate to the checkout page
        When I fill in "First Name" with "Alex"
        And I fill in "Last Name" with "Galanis"
        And I proceed to checkout overview
        Then I should see the error message "Error: Postal Code is required"

    Scenario: Verify item prices in summary
        Given I navigate to the checkout page
        When I complete checkout information
        Then I should see the correct item prices in the summary

    Scenario: Cancel checkout maintains cart items
        Given I navigate to the checkout page
        When I complete checkout information
        And I cancel the checkout
        Then I should return to the inventory page
        And the cart should have 2 items