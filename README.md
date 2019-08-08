# PaymentGateway
- A payment gateway
- Authentication using secret key done by setting a custom authentication scheme - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
- Autofac Keyed Services used to select appropriate processor. 
- BankSimulator API mocks an acquirer and sends a successful or failed response based on the amount being passed.
- Payment details are stored in the database and can be queried.
- Created resource link available in create payment response.
- Card numbers are encrypted using AES and masked when returned by the API.
- IHttpClientFactory and Refit used to create an API client library - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2
- CorrelationId flows from MerchantSimulator to Gateway to BankSimulator and is logged on Seq and stored in PostgreSQL.
- Credit Card validation done using CreditCardValidator library - https://github.com/gustavofrizzo/CreditCardValidator
- Validation at API and Service layers using FluentValidation.

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
- CorrelationId
- Serilog
- Seq
- IHttpClientFactory
- Refit
- FluentValidation

# To-do
- Time request-response
- Benchmarking and performance tuning
- Document about separation of processors
- Add health checks

# Assumptions
- For the sake of this test, the routing process has been kept very straight-forward. Card scheme and currency are not taken into consideration when routing and all requests are sent to the simulator.  