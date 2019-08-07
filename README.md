# PaymentGateway
- A payment gateway
- Authentication using secret key done by setting a custom authentication scheme - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
- Autofac Keyed Services used to select appropriate processor. 
- BankSimulator API mocks an acquirer and sends a successful or failed response based on the amount being passed.
- Payment details are stored in the database and can be queried.
- Created resource link available in create payment response.
- Card numbers are encrypted using AES and masked when returned by the API.

# Technologies and packages used
- ASP.NET Core 2.2
- PostgreSQL
- Dapper
- Evolve
- Docker
- AutoMapper
- Autofac
- Swagger
- App Metrics

# To-do
- Time request-response
- Benchmarking and performance tuning
- Document about separation of processors
- FluentValidation
- Add health checks

# Assumptions
- For the sake of this test, the routing process has been kept very straight-forward. Card scheme and currency are not taken into consideration when routing and all requests are sent to the simulator.  