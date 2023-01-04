using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Subscriber.Sagas;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.Sagas;

public class AppointmentBookingSagaShould
{
    [Fact]
    public async Task CompleteIfFollowUpNotRequired()
    {
        var loggerMock = new Mock<ILogger<FollowUpAppointmentSaga>>();
        var businessEntitySaga = new FollowUpAppointmentSaga(loggerMock.Object)
        {
            Data = new FollowUpAppointmentSagaData()
        };
        var context = new TestableMessageHandlerContext();
        var message = new AppointmentNotesAddedToMedicalRecord(
            "Ben",
            new AppointmentNotes(Guid.NewGuid(), AppointmentType.Routine, "Patient was well", false, DateTime.UtcNow)
            );

        await businessEntitySaga.Handle(message, context);

        Assert.True(businessEntitySaga.Completed);
    }

    [Fact]
    public async Task NotCompleteIfFollowUpRequired()
    {
        var loggerMock = new Mock<ILogger<FollowUpAppointmentSaga>>();
        var businessEntitySaga = new FollowUpAppointmentSaga(loggerMock.Object)
        {
            Data = new FollowUpAppointmentSagaData()
        };
        var context = new TestableMessageHandlerContext();
        var message = new AppointmentNotesAddedToMedicalRecord(
            "Ben",
            new AppointmentNotes(Guid.NewGuid(), AppointmentType.Routine, "Patient was not well, check in 2 months", true, DateTime.UtcNow)
            );

        await businessEntitySaga.Handle(message, context);

        Assert.False(businessEntitySaga.Completed);
        Assert.Contains(message.Details.Id, businessEntitySaga.Data.AppointmentsRequiringFollowUps.Keys);
    }
}
