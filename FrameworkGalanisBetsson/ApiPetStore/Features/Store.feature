@api
Feature: Store Order API Testing
As a store manager
I want to manage orders through the API
So that I can track pet purchases

    Scenario: Place a new order for a pet
        Given I have an existing pet in the store
        When I place an order with the following details
          | Quantity | Status | Complete |
          | 1        | placed | false    |
        Then the order should be successfully placed
        And I can retrieve the order details

    Scenario: Get order by ID
        Given there is an existing order in the store
        When I request the order by its ID
        Then the order details should be correctly returned
        And the order status should be "placed"
   
    @DeleteOrder
    Scenario: Delete an order
        Given there is an existing order in the store
        When I delete the order
        Then the order should be successfully deleted
        And retrieving the order should return not found