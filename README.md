# Online Marketplace RESTful API

This RESTful API is designed to handle order functionality for an online marketplace. It allows users to create orders, sellers to mark orders as completed, and automatically deletes unpaid orders after 2 hours. Users can also retrieve a list of their orders.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
  - [Creating an Order](#creating-an-order)
  - [Updating order status](#updating-order-status)
  - [Automatic Deletion of Unpaid Orders](#automatic-deletion-of-unpaid-orders)
  - [Retrieving User's Orders](#retrieving-users-orders)
- [API Endpoints](#api-endpoints)

## Getting Started

### Prerequisites

Before running the API, ensure you have the following installed:

- Docker
- Docker Compose

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/emile-adform/ExamTask.git
2. Navigate to the project directory:
   ```bash
   cd Exam.WebApi
3. Run the Docker Compose command:
   ```bash
   docker-compose up -d
4. Access the API at https://localhost:52827/swagger
5. When Docker is running, navigate to liquibase migrations directory:
   ```bash
   cd ..
   cd src\Infrastructure\DBMigrations
6. And run liquibase update command:
    ```bash
    $ liquibase update --changelog-file changelog.xml 
7. If you want to rollback changes, you can use command:
    ```bash
    $ liquibase rollbackCount {count} --changelog-file changelog.xml

After running liquibase update, you have database with all required tables and some sample data in items table.

## Usage

### Creating an order

Use POST endpoint. Users list is from https://jsonplaceholder.typicode.com/users . Sample items are included in the database

### Updating order status

Use PUT endpoint. Status can be changed to "paid" or "completed". By default it is "created".

### Automatic Deletion of Unpaid Orders

Background service takes care of unpaid orders. If the order is not paid in 2 hours after creation, it is automatically deleted. Background service runs every minute to ensure precision.

### Retrieving User's Orders

It is possible to retrieve all the orders (that have not been deleted) by user Id.

## API Endpoints

### Creating an order

Use POST endpoint. For creation you will need to enter UserId (which user wants to buy the item) and ItemId (what item user wants to buy)

### Updating status of an order

Use PUT endpoint. You have to enter OrderId which you want to update and Status (as a string) - status can be "paid" or "completed"

### Getting all orders of a user

Use GET endpoint. Enter user Id and the list of orders will be returned.
