@api
Feature: Pet Store API Testing
As a pet store manager
I want to manage pets through the API
So that I can keep the pet inventory up to date

    Scenario: Add a new pet to the store
        Given I have a new pet with the following details
          | Name   | Category | Status    |
          | Alex   | Cat      | available |
        When I send a request to add the pet
        Then the pet should be successfully added
        And I can retrieve the pet by ID

    Scenario: Update an existing pet's information
        Given I have an existing pet in the store with name "Alex"
        When I update the pet's status to "sold"
        Then the pet's status should be updated successfully
        And the pet's new status should be "sold"

    Scenario: Delete a pet from the store
        Given I have an existing pet in the store with name "BillTheCat"
        When I delete the pet from the store
        Then the pet should be successfully deleted
        And retrieving the pet should return not found

    Scenario: Find pets by single tag
        Given I have created the following pets with tags
          | Pet Name | Status    | Tags            |
          | Alex     | available | friendly,kitten |
          | Maria    | available | shy,kitten      |
        When I search for pets with tag "kitten"
        Then I should find 2 pets in the results
        And the pets "Alex,Maria" should be in the results

    Scenario: Find pets by multiple tags intersection
        Given I have created the following pets with tags
          | Pet Name | Status    | Tags             |
          | Alex     | available | friendly,trained |
          | Maria    | available | friendly,guard   |
        When I search for pets with tags "friendly,trained"
        Then I should find 1 pet in the results
        And the pet "Alex" should be in the results

    Scenario: Find pets filtered by status and tag
        Given I have created the following pets with tags
          | Pet Name | Status    | Tags            |
          | Alex     | available | indoor,friendly |
          | Maria    | sold      | indoor,friendly |
        When I search for available pets with tag "indoor"
        Then I should find 1 pet in the results
        And the pet "Alex" should be in the results