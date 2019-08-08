# Payment Gateway
## Components
- This payment gateway has three main components:
	* MerchantSimulator
	* Gateway
	* BankSimulator

### MerchantSimulator
- This API mocks a merchant and has been created just to test the API client library built using IHttpClientFactory and Refit - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2
- The MerchantSimulator API sends all requests to the Gateway API.
- A CorrelationId is also auto-generated and propagated until the BankSimulator API. This uses the CorrelationId package - https://github.com/stevejgordon/CorrelationId
- The CorrelationId can also be passed by setting the CorrelationId header in the HTTP request.

### Gateway
- The Gateway API hosts all the logic necessary to process a payment.
- Its endpoints are secure and Authentication is done using a secret key - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
- Requests are then forwarded any one acquiring bank.
- Decision on routing is based on pre-defined settings in the database tables.
- Autofac Keyed Services is used to select appropriate processor.
- Payment details are stored in a PostgreSQL database and can be queried.
- Created resource link available in create payment response.
- Card numbers are encrypted using AES and masked when returned by the API.
- Validation at API and Service layers using FluentValidation.
- Credit Card validation done using CreditCardValidator library - https://github.com/gustavofrizzo/CreditCardValidator
- App metrics have been included and available at 'http://localhost:5000/metrics'.
- All logs go to Seq via Serilog.

### BankSimulator
- The BankSimulator API mocks an acquirer and sends a successful or failed response based on the amount being passed.
- CorrelationId flows from MerchantSimulator to Gateway to BankSimulator and is logged on Seq and stored in PostgreSQL.

## Database design
- The data store used is PostgreSQL and is created upon launch of the docker containers using migration scripts and Evolve - https://github.com/lecaillon/Evolve.
- Dapper is used as ORM to communicate with the DB and is setup in a Repository pattern to promote de-coupling.
- The database has the following tables:
	* Merchants - stores all merchant details along with the secret key to use in API authentication.
	* Acquirers - stores acquirer details, mainly the URL of the acquirer API.
	* MerchantAcquirers - links merchants to acquirers.
	* AcquirerResponseCodeMappings - maps response code details from the acquirer to that of the gateway.
	* Payments - stores all the payment details.

## Technologies and packages used
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

## How-to
- The Gateway has two main functionality that it provides:
1. Create a payment
2. Retrieve details of a past payment
- Swagger UI can be accessed at `http://localhost:5000/swagger/index.html`.
- Download the following Postman collection - [![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/47fc17ea36637b4502d7)

### Create a payment
1. The entire system has been containerised and runs on Docker. Bring everything up using `docker-compose up`. Wait some time until all containers are up and running.
2. Select the Create Payment request and hit the Send button.
3. A response should be back with the payment details.
```
{
    "acquirerPaymentId": "6d1ab701-96ef-4dfc-869a-0383892c68e5",
    "paymentId": "82bce548-3de0-424e-9051-135d60d9c3ae",
    "trackId": "Track ID 1",
    "amount": 1100,
    "currency": "USD",
    "card": {
        "number": "495308****3607",
        "expiryMonth": 10,
        "expiryYear": 2023,
        "name": "Test User"
    },
    "responseCode": "10000",
    "status": "Succeeded",
    "acquirerStatus": "Succeeded",
    "acquirerResponseCode": "200"
}
```

### Retrieve details of a past payment
1. Copy the paymentId GUID and select the Get Payment request.
2. Overwrite the paymentId in the URL and hit Send.
3 The same response as the previous one should be obtained.

### Notes
- The Postman collection also contains two more requests that can be used to hit the MerchantSimulator API.
- Notice the Authorization key in both request headers. This is the Merchant secret key and is found in the Merchants table in the database.
- It is the key used to identify the merchant. If an invalid key or a different key is used to obtain a merchant's payment, a 401 response will be obtained.

## Assumptions
- For the sake of this test, the routing process has been kept very straight-forward. Card scheme and currency are not taken into consideration when routing and all requests are sent to the simulator.
- A complete payment is processed with one single to the Gateway.
- Validations have been kept to a bare-minimum.
- The Database is also a very simplistic one and does not include a lot of requirements from a proper payment gateway.

## Design decisions
- The MerchantSimulator and BankSimulator APIs have been kept very simple in terms of design.
- The Gateway API is designed on a 3-layer approach - API, Service and Data layers. It has also been implemented in several projects to allow reusability.
- A common folder exists with common projects that are re-used across all components.

## Improvements
- Obviously, this is a very naive, quickly built PoC of a Payment Gateway and is far from a production system.
- More validations and risk checks.
- Better routing system with cascading to increase success rate.
- Benchmarking and performance tests to identify hot paths and improve response times.
- Caching can be considered for get payment details responses and keeping merchant and acquirer details in-memory.
- Add health checks and prepare for an orchestration system like Kubernetes to properly handle a microservices setup.
- Break the main gateway into smaller services and fits better in a microservices pattern. They can then be developed, maintained and deployed separately.
- Generate asynchronous events to keep all services in sync to changes in the eco-system - Use technologies like RabbitMQ, Kafka or EventStore for this.
- Communication between microservices can be done by asynchronous events, HTTP or gRPC.