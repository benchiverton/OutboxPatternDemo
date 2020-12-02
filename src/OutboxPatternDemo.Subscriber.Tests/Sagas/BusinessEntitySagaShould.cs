using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Subscriber.Sagas;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.Sagas
{
    public class BusinessEntitySagaShould
    {

        [Fact]
        public void ProcessNewMessage()
        {
            var loggerMock = new Mock<ILogger<BusinessEntitySaga>>();
            var businessEntitySaga = new BusinessEntitySaga(loggerMock.Object)
            {
                Data = new BusinessEntitySagaData()
            };
            var context = new TestableMessageHandlerContext();
            var message = new StateUpdated
            {
                BusinessEntityId = "qwerty",
                Details = new StateDetail
                {
                    Id = 123,
                    State = "NEWSTATE",
                    TimeStampUtc = DateTime.UtcNow
                }
            };

            businessEntitySaga.Handle(message, context);

            // TODO verify business logic is executed
            Assert.Single(businessEntitySaga.Data.StateDetails);
        }

        [Fact]
        public void NotProcessDuplicate()
        {
            var loggerMock = new Mock<ILogger<BusinessEntitySaga>>();
            var businessEntitySaga = new BusinessEntitySaga(loggerMock.Object)
            {
                Data = new BusinessEntitySagaData
                {
                    StateDetails = new Dictionary<int, StateDetail>
                    {
                        {123, new StateDetail()}
                    }
                }
            };
            var context = new TestableMessageHandlerContext();
            var message = new StateUpdated
            {
                BusinessEntityId = "qwerty",
                Details = new StateDetail
                {
                    Id = 123,
                    State = "NEWSTATE",
                    TimeStampUtc = DateTime.UtcNow
                }
            };

            businessEntitySaga.Handle(message, context);

            // TODO verify business logic not executed twice
            Assert.Single(businessEntitySaga.Data.StateDetails);
        }
    }
}
