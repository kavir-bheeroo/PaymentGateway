using Autofac.Features.Indexed;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Gateway.Acquiring.Contracts;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Core.Services;
using Gateway.Core.Validators;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Gateway.Core.UnitTests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IMerchantAcquirerRepository> _merchantAcquirerRepositoryMock;
        private readonly Mock<IAcquirerResponseCodeMappingRepository> _acquirerResponseCodeMappingRepositoryMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IIndex<ProcessorList, IProcessor>> _indexMock;
        private readonly Mock<IProcessor> _processorMock;
        private readonly Mock<ICryptor> _cryptorMock;
        private readonly Mock<IMapper> _mapperMock;

        // Extension methods of the validators cannot be mocked so proper instances are used.
        private readonly IValidator<CardRequestModel> _cardRequestModelValidator;
        private readonly IValidator<PaymentRequestModel> _paymentRequestModelValidator;
        private readonly IValidator<MerchantModel> _merchantModelValidator;

        private readonly PaymentService _sut;

        public PaymentServiceTests()
        {
            _merchantAcquirerRepositoryMock = new Mock<IMerchantAcquirerRepository>();
            _acquirerResponseCodeMappingRepositoryMock = new Mock<IAcquirerResponseCodeMappingRepository>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _indexMock = new Mock<IIndex<ProcessorList, IProcessor>>();
            _processorMock = new Mock<IProcessor>();
            _cryptorMock = new Mock<ICryptor>();
            _mapperMock = new Mock<IMapper>();

            _cardRequestModelValidator = new CardRequestModelValidator();
            _paymentRequestModelValidator = new PaymentRequestModelValidator(_cardRequestModelValidator);
            _merchantModelValidator = new MerchantModelValidator();

            _sut = new PaymentService(
                _merchantAcquirerRepositoryMock.Object,
                _acquirerResponseCodeMappingRepositoryMock.Object,
                _paymentRepositoryMock.Object,
                _indexMock.Object,
                _cryptorMock.Object,
                _paymentRequestModelValidator,
                _merchantModelValidator,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnPaymentResponse()
        {
            // Arrange
            var paymentId = Guid.NewGuid();

            var merchant = new Faker<MerchantModel>()
                .RuleFor(x => x.Id, 1)
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.SecretKey, f => f.Random.String(10))
                .Generate();
            var payment = new Faker<PaymentEntity>()
                .RuleFor(x => x.MerchantId, 1)
                .Generate();

            var paymentResponseModel = new Faker<PaymentResponseModel>()
                .RuleFor(x => x.ResponseCode, Constants.SuccessResponseCode)
                .RuleFor(x => x.Card, new Faker<CardResponseModel>()
                    .RuleFor(x => x.Number, f => f.Random.String(10))
                    .RuleFor(x => x.ExpiryMonth, DateTime.UtcNow.Month)
                    .RuleFor(x => x.ExpiryYear, DateTime.UtcNow.AddYears(1).Year)
                    .RuleFor(x => x.Name, f => f.Person.FullName)
                    .Generate())
                .Generate();

            _paymentRepositoryMock
                .Setup(x => x.GetByIdAsync(paymentId))
                .ReturnsAsync(payment);

            _mapperMock
                .Setup(x => x.Map<PaymentResponseModel>(payment))
                .Returns(paymentResponseModel);

            _cryptorMock
                .Setup(x => x.Decrypt(paymentResponseModel.Card.Number))
                .Returns(new Faker().Finance.CreditCardNumber());

            // Act
            var response = await _sut.GetByIdAsync(paymentId, merchant);

            // Assert
            response.Should().NotBeNull();

            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(paymentId), Times.Once);
            _mapperMock.Verify(x => x.Map<PaymentResponseModel>(payment), Times.Once);
            _cryptorMock.Verify(x => x.Decrypt(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldReturnPaymentResponse()
        {
            // Arrange
            var creditCardNumber = new Faker().Finance.CreditCardNumber();
            var merchantName = new Faker().Company.CompanyName();
            var acquirerName = "BankSimulator";

            var request = new Faker<PaymentRequestModel>()
                .RuleFor(x => x.Amount, 100)
                .RuleFor(x => x.Currency, "USD")
                .RuleFor(x => x.Card, new Faker<CardRequestModel>()
                        .RuleFor(x => x.Number, creditCardNumber)
                        .RuleFor(x => x.Cvv, "323")
                        .RuleFor(x => x.ExpiryMonth, DateTime.UtcNow.Month)
                        .RuleFor(x => x.ExpiryYear, DateTime.UtcNow.AddYears(1).Year)
                        .RuleFor(x => x.Name, f => f.Person.FullName)
                        .Generate())
                .Generate();

            var merchant = new Faker<MerchantModel>()
                .RuleFor(x => x.Id, 1)
                .RuleFor(x => x.Name, merchantName)
                .RuleFor(x => x.SecretKey, f => f.Random.String(10))
                .Generate();

            var merchantAcquirer = new Faker<MerchantAcquirerEntity>()
                .RuleFor(x => x.MerchantId, 1)
                .RuleFor(x => x.MerchantName, f => merchantName)
                .RuleFor(x => x.AcquirerName, acquirerName)
                .Generate();

            var processorResponse = new Faker<ProcessorResponse>()
                .RuleFor(x => x.AcquirerResponseCode, "10000")
                .Generate();

            var acquirerResponseCodeMapping = new Faker<AcquirerResponseCodeMapping>()
                .RuleFor(x => x.GatewayResponseCode, Constants.SuccessResponseCode)
                .Generate();

            var paymentResponseModel = new Faker<PaymentResponseModel>()
                .RuleFor(x => x.ResponseCode, Constants.SuccessResponseCode)
                .RuleFor(x => x.Card, new Faker<CardResponseModel>()
                    .RuleFor(x => x.Number, creditCardNumber)
                    .RuleFor(x => x.ExpiryMonth, DateTime.UtcNow.Month)
                    .RuleFor(x => x.ExpiryYear, DateTime.UtcNow.AddYears(1).Year)
                    .RuleFor(x => x.Name, f => f.Person.FullName)
                    .Generate())
                .Generate();

            _merchantAcquirerRepositoryMock
                .Setup(x => x.GetByMerchantIdAsync(merchant.Id))
                .ReturnsAsync(merchantAcquirer);

            _indexMock.Setup(x => x[It.IsAny<ProcessorList>()]).Returns(_processorMock.Object);
            _cryptorMock.Setup(x => x.Encrypt(request.Card.Number)).Returns(It.IsAny<string>());
            _paymentRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PaymentEntity>()));

            _mapperMock
                .Setup(x => x.Map<PaymentDetails>(request))
                .Returns(It.IsAny<PaymentDetails>());

            _mapperMock
                .Setup(x => x.Map<AcquirerDetails>(merchantAcquirer))
                .Returns(It.IsAny<AcquirerDetails>());

            _processorMock
                .Setup(x => x.ProcessPaymentAsync(It.IsAny<ProcessorRequest>()))
                .ReturnsAsync(processorResponse);

            _acquirerResponseCodeMappingRepositoryMock
                .Setup(x => x.GetByAcquirerResponseCodeAsync(processorResponse.AcquirerResponseCode))
                .ReturnsAsync(acquirerResponseCodeMapping);

            _mapperMock
                .Setup(x => x.Map<PaymentResponseModel>(processorResponse))
                .Returns(paymentResponseModel);

            _mapperMock.Setup(x => x.Map(paymentResponseModel, It.IsAny<PaymentEntity>()));

            _paymentRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PaymentEntity>()));

            // Act
            var response = await _sut.ProcessAsync(request, merchant);

            // Assert
            response.Should().NotBeNull();

            _merchantAcquirerRepositoryMock.Verify(x => x.GetByMerchantIdAsync(merchant.Id), Times.Once);
            _indexMock.Verify(x => x[It.IsAny<ProcessorList>()], Times.Once);
            _cryptorMock.Verify(x => x.Encrypt(request.Card.Number), Times.Once);
            _paymentRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<PaymentEntity>()), Times.Once);
            _mapperMock.Verify(x => x.Map<PaymentDetails>(request), Times.Once);
            _mapperMock.Verify(x => x.Map<AcquirerDetails>(merchantAcquirer), Times.Once);
            _processorMock.Verify(x => x.ProcessPaymentAsync(It.IsAny<ProcessorRequest>()), Times.Once);
            _acquirerResponseCodeMappingRepositoryMock.Verify(x => x.GetByAcquirerResponseCodeAsync(processorResponse.AcquirerResponseCode), Times.Once);
            _mapperMock.Verify(x => x.Map<PaymentResponseModel>(processorResponse), Times.Once);
            _mapperMock.Verify(x => x.Map(paymentResponseModel, It.IsAny<PaymentEntity>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PaymentEntity>()), Times.Once);
        }
    }
}