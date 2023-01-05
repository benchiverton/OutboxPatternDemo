using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using OutboxPatternDemo.Bookings.Sagas;
using OutboxPatternDemo.MedicalRecords.Contract.Events;
using OutboxPatternDemo.MedicalRecords.Contract.Models;
using Xunit;

namespace OutboxPatternDemo.Bookings.Tests.Sagas;

public class AppointmentBookingSagaShould
{
    [Fact]
    public async Task CompleteIfFollowUpNotRequired()
    {
        var loggerMock = new Mock<ILogger<FollowUpAppointmentSaga>>();
        var followUpAppointmentSaga = new FollowUpAppointmentSaga(loggerMock.Object)
        {
            Data = new FollowUpAppointmentSagaData()
        };
        var context = new TestableMessageHandlerContext();
        var message = new AppointmentNotesAddedToMedicalRecord(
            "Ben",
            new AppointmentNotes(Guid.NewGuid(), AppointmentType.Routine, "Patient was well", false, DateTime.UtcNow)
            );

        await followUpAppointmentSaga.Handle(message, context);

        Assert.True(followUpAppointmentSaga.Completed);
    }

    [Fact]
    public async Task NotCompleteIfFollowUpRequired()
    {
        var loggerMock = new Mock<ILogger<FollowUpAppointmentSaga>>();
        var followUpAppointmentSaga = new FollowUpAppointmentSaga(loggerMock.Object)
        {
            Data = new FollowUpAppointmentSagaData()
        };
        var context = new TestableMessageHandlerContext();
        var message = new AppointmentNotesAddedToMedicalRecord(
            "Ben",
            new AppointmentNotes(Guid.NewGuid(), AppointmentType.Routine, "Patient was not well, check in 2 months", true, DateTime.UtcNow)
            );

        await followUpAppointmentSaga.Handle(message, context);

        Assert.False(followUpAppointmentSaga.Completed);
        Assert.Contains(message.Details.Id, followUpAppointmentSaga.Data.AppointmentsRequiringFollowUps.Keys);
    }
}
