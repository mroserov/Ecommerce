# eShop Solución Test


## Overview
This document details the tools, frameworks, patterns, and libraries used to build our e-commerce application. The application follows a microservices architecture with a frontend in React and a backend in .Net Core.


[![Arquitecture](https://mroserov.blob.core.windows.net/mroserov/EcommerceDiagram.png)](https://www.digitalocean.com/products/app-platform)

Diagram build in [Diagrams](https://app.diagrams.net/)

## Tools and Technologies

### Frontend
- **React**: A JavaScript library for building user interfaces, used for creating the web frontend of the e-commerce application.
- **Typescript**: A superset of JavaScript that adds static types, used to enhance code quality and maintainability.
- **Redux**: A predictable state container for JavaScript apps, used for managing the application state.
- **Vite**: A build tool that provides a fast development server and optimized builds.
- **MUI (Material-UI)**: A popular React UI framework for building responsive and accessible user interfaces.

### Backend
- **.Net Core 8**: A cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications.
- **API Gateway (Ocelot)**: A .NET API Gateway for running a microservices/API-based application. Used for request routing and load balancing.
- **Load Balancer**: Used for distributing network or application traffic across a number of servers.

### Microservices Architecture
- (TODO) **RabbitMQ**: A message broker used for communication between microservices.
- **GraphQL**: A query language for APIs used to enable clients to request only the data they need.
- **Redis**: An in-memory data structure store, used as a database, cache, and message broker.
- **SQL Server**: A relational database management system used for storing structured data.

### Design Patterns and Principles
- **Clean Architecture**: A design philosophy that separates the concerns of the application into layers, improving maintainability and testability.
- **CQRS (Command Query Responsibility Segregation)**: A pattern that separates read and write operations to optimize performance, scalability, and security.
- **MediatR**: A .NET library used to implement the Mediator pattern, facilitating communication between components.
- **Unit of Work**: A pattern that ensures a series of operations are completed as a single unit.
- **Repository Pattern**: A design pattern that abstracts data access, making the application more testable and maintainable.

### Additional Tools and Libraries
- **Automapper**: A convention-based object-object mapper used to eliminate the need for manual property mapping.
- **Docker**: A platform used for developing, shipping, and running applications in containers.
- **Elasticsearch**: A distributed, RESTful search and analytics engine used for log and event data.
- **Serilog**: A logging library for .NET, used to log structured event data.
- **GRPC**: A high-performance RPC framework used for communication between microservices.
- **Azure Blob Storage**: A Microsoft cloud storage solution for storing large amounts of unstructured data.

## Folder Structure
**Dockerfile**: Used to containerize each service

### frontend
- **Dockerfile**: Used to containerize the frontend application.

### Gateway
- **YARP**: Used for routing and authentication between microservices.


### Infraestructure
- **Ecommerce.Common**: Error handler Middleware.
- TODO **EventBus.Messages**: Manages RabbitMQ message definitions.

### Ecommerce.Authentication
- TODO **JWT Authentication**: Manages user authentication and token generation.
- **Dockerfile**: Used to containerize the authentication service.

### Ecommerce.Catalog
- **Clean architecture**: Ensures separation of concerns.
- **GraphQL**: Exposes a GraphQL endpoint for querying product data.
- **GRPC Server**: Provides product stock information via gRPC.
- **Dockerfile**: Used to containerize the catalog service.

### Ecommerce.Order
- TODO **JWT Authorization**: Ensures secure access to order services.
- **Dockerfile**: Used to containerize the order service.
- **GRPC Client**: Communicates with the catalog service for product stock information.

### Ecommerce.Basket
- **Redis**: Used for storing shopping cart data.

 
> [!NOTE]  
> Some functionalities of the architecture were not implemented, they were added for reference of how the final architecture was intended.


## Aditional feature

The option to search the complete product catalog has been implemented.
## Docker Compose
The `docker-compose.yml` file is used to orchestrate the deployment of all services and dependencies, including SQL Server, Redis, RabbitMQ(TODO), Elasticsearch, and Kibana. Database passwords and other sensitive information are managed in a separate configuration file not included in the repository.

To execute it you must run the command 

```
docker-compose up --build
```

If you want to delete the containers and recreate them you must run

```
docker-compose down
docker-compose up --build
```

Services are executed at the following urls

> Frontend react -> [http://localhost:3000/](http://localhost:3000/)

> Authentication API -> [http://localhost:8091/swagger/index.html](http://localhost:8091/swagger/index.html)

> Authentication SQL Database 
```
Server=localhost,1433;Database=AuthenticationDb;User Id=sa;Password=Sqlserver123**;TrustServerCertificate=true
```

> Catalog API Rest [http://localhost:8092/swagger/index.html](http://localhost:8092/swagger/index.html)

> Catalog API GraphQL [http://localhost:8092/graphql/](http://localhost:8092/graphql/)

> Catalog SQL Database
```
Server=localhost,1434;Database=CatalogDb;User Id=sa;Password=Sqlserver123**;TrustServerCertificate=true
```

Insert test data in Catalog with `InsertTestData.sql` script, change @AmountData.


> Basket API [http://localhost:8093/swagger/index.html](http://localhost:8093/swagger/index.html)

> Basket Redis
```
localhost:6379
```

> Orders API [http://localhost:8094/swagger/index.html](http://localhost:8094/swagger/index.html)

> Orders SQL Database
```
Server=localhost,1435;Database=OrderDb;User Id=sa;Password=Sqlserver123**;TrustServerCertificate=true
```

## Run Local

- React app takes .env.local with  local URL's

```
cd frontend
npm run local
```

- Change in appsetting.Development the Connection String for each servcice and run as multiple startup

> Basket Redis `Services\Ecommerce.Basket\Ecommerce.Basket.Api\appsettings.Development.json`
```
"Redis": {
	"ConnectionString": "localhost:6379"
}
```

> Catalog SQL Database `Services\Ecommerce.Catalog\Ecommerce.Catalog.Api\appsettings.Development.json`
```
"ConnectionStrings": {
    "CatalogDatabase": "Server=localhost,1434;Database=CatalogDb;User Id=sa;Password=Sqlserver123**;TrustServerCertificate=true;"
}
```

> Orders SQL Database `Services\Ecommerce.Orders\Ecommerce.Orders.Api\appsettings.Development.json`
```
"ConnectionStrings": {
	"OrdersDatabase": "Server=localhost,1435;Database=OrderDb;User Id=sa;Password=Sqlserver123**;TrustServerCertificate=true;"
}
```

## References

Azure File Share Storage implementation using .NET Core 6 Web API
https://medium.com/@jaydeepvpatil225/azure-file-share-storage-implementation-using-net-core-6-web-api-f02cf8157f1d

A Serilog sink that writes logs directly to Elasticsearch 
https://www.elastic.co/guide/en/ecs-logging/dotnet/current/serilog-data-shipper.html

Pagination
https://henriquesd.medium.com/pagination-in-a-net-web-api-with-ef-core-2e6cb032afb7

Graphql
https://medium.com/@hilalyazbek/creating-a-graphql-server-using-net-core-cqrs-and-hotchocolate-part-1-decf59b2f983
https://medium.com/@hilalyazbek/creating-a-graphql-server-using-net-03c58a866a74
https://www.c-sharpcorner.com/article/graphql-in-net-pagination-part-3/

Clean Architecture
https://rahulsahay19.medium.com/how-clean-architecture-can-be-used-to-build-more-testable-maintainable-and-evolvable-applications-2f9f15236e3b
https://www.linkedin.com/pulse/demo-celan-architecture-cqrs-repository-pattern-net-web-edin-%C5%A1ahbaz/

# Azure API
## Authentication
Authentication api in Azure Service via GithubAction [Authentication](https://eshoes-auth.azurewebsites.net/swagger/index.html)

