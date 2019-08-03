# PaymentGateway
- A payment gateway
- Authentication using secret key done by setting a custom authentication scheme - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
- Autofac Keyed Services used to select appropriate processor. 

# Technologies used
- ASP.NET Core 2.2
- PostgreSQL
- Dapper
- Evolve
- Docker
- AutoMapper
- Autofac

# To-do
- Hypermedia link
- Time request-response
- Mask/Encrypt card
- Benchmarking and performance tuning
- Document about separation of processors
- FluentValidation

# Assumptions
- For the sake of this test, the routing process has been kept very straight-forward. Card scheme and currency are not taken into consideration when routing and all requests are sent to the simulator.  