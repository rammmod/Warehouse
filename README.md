Develop Warehouse Microservice
 
Warehouse has products      (human readable id, name and category)

Each product has stock. (Number)

Product can have stock thresholds

OutOfStock when available items is less then OutOfStock threshold for category

LowStock when available items is less then LowStock threshold for category

Available when its not OutOfStock and LowStock

Application should have functionality to reserve stock items for products and later approve or decline those requests.

Each reservation of stock is and Order (business term)

When User is trying to reserve stock for product which is Available - do it.

When User is trying to reserve stock for product which is LowStock - reserve stock and communicate with 3rd party system for manual approval or rejection.  Put and order under review status. (Rabbit MQ integration, you can create mock service to respond with data. Contracts up to u. Communication is fully async Request\Callback events) Note: You have distributed transaction here.

When user is trying to reserve stock for product which is in OutOfStock

Support 2 modes - ReserveWhenAvailable, None.

When ReserveWhenAvailable - do not reject request, create a request and wait until stock will be low stock of available

When none - reject request and return an error for the client.

System should support manually changing stock amount for products. (Per product). Cannot be negative.

Prepare endpoint for creating such orders.

I can specify product, stock.

Prepare endpoint for viewing order history (no query filters for now).

Prepare endpoints for CRU products (no delete)

Prepare endpoints for CRUD categories. (You can delete category if there are no products for this category)

All endpoints should be available from swagger (have OpenAPI specification)
 
Stack to use
1. .NET 5 or .NET 6
2. MassTransit for RabbitMQ and Sagas
3. Optional FluentValidator
4. MediatR
5. xUnit
6. NSubstitute
7. FluentAssertions
8. MongoDB
9. Swashbuckle or NSwag
