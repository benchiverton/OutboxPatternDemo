using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Subscriber.Sagas;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.Sagas;

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
        var message = new StateUpdated(
            "qwerty",
            new StateDetail(Guid.NewGuid(), "NEWSTATE", DateTime.UtcNow)
            );

        businessEntitySaga.Handle(message, context);

        // TODO verify business logic is executed
        Assert.Single(businessEntitySaga.Data.StateDetails);
    }

    [Fact]
    public void NotProcessDuplicate()
    {
        var loggerMock = new Mock<ILogger<BusinessEntitySaga>>();
        var duplicateId = Guid.NewGuid();
        var businessEntitySaga = new BusinessEntitySaga(loggerMock.Object)
        {
            Data = new BusinessEntitySagaData
            {
                StateDetails = new Dictionary<Guid, StateDetail>
                    {
                        {duplicateId, new StateDetail(duplicateId, null, DateTime.UtcNow)}
                    }
            }
        };
        var context = new TestableMessageHandlerContext();
        var message = new StateUpdated(
            "qwerty",
            new StateDetail(duplicateId, "NEWSTATE", DateTime.UtcNow)
            );

        businessEntitySaga.Handle(message, context);

        // TODO verify business logic not executed twice
        Assert.Single(businessEntitySaga.Data.StateDetails);
    }
}
