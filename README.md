# PaymentGateway
- A payment gateway
- Authentication using secret key done by setting a custom authentication scheme - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
- Autofac Keyed Services used to select appropriate processor. 
- BankSimulator API mocks an acquirer and sends a successful or failed response based on the amount being passed.
- Payment details are stored in the database and can be queried.
- Created resource link available in create payment response.

# Technologies used
- ASP.NET Core 2.2
- PostgreSQL
- Dapper
- Evolve
- Docker
- AutoMapper
- Autofac

# To-do
- Time request-response
- Mask/Encrypt card
- Benchmarking and performance tuning
- Document about separation of processors
- FluentValidation
- Swagger

# Assumptions
- For the sake of this test, the routing process has been kept very straight-forward. Card scheme and currency are not taken into consideration when routing and all requests are sent to the simulator.  