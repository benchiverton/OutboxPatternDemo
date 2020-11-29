using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;
using OutboxPatternDemo.Subscriber.Handlers;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.Handlers
{
    public class BusinessEntityEventHandlerShould
    {
        [Fact]
        public void CheckForDuplicates()
        {
            var loggerMock = new Mock<ILogger<BusinessEntityEventHandler>>();
            var duplicateCheckerMock = new Mock<IDuplicateChecker>();
            duplicateCheckerMock.Setup(dc => dc.IsDuplicate(12345)).Verifiable();
            var handler = new BusinessEntityEventHandler(loggerMock.Object, duplicateCheckerMock.Object);
            var context = new TestableMessageHandlerContext();

            handler.Handle(new StateUpdated { Details = new StateDetail { Id = 12345 } }, context);

            duplicateCheckerMock.Verify();
        }
    }
}
