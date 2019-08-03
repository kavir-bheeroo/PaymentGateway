# PaymentGateway
- A payment gateway
- Authentication using secret key done by setting a custom authentication scheme - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2

# Technologies used
- ASP.NET Core 2.2
- PostgreSQL
- Dapper
- Evolve
- Docker

# To-do
- Hypermedia link
- Time request-response
- Implement an acquirer factory to retrieve acquirer implementation based on configuration
- Connect to data store using Dapper
- Add AutoMapper
- Add Autofac
- Mask/Encrypt card
- Benchmarking and performance tuning
- Document about separation of processors

# Assumptions
- For the sake of this test, the routing process has been kept very straight-forward. Card scheme and currency are not taken into consideration when routing and all requests are sent to the simulator.  