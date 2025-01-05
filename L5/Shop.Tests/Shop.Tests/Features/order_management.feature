Feature: Order Management

  Scenario: Add product to an order
    Given I am an authenticated user
    When I add "Laptop" to my order
    Then my order should contain "Laptop"
