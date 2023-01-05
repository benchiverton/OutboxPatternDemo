using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using OutboxPatternDemo.Bookings.Contract.Commands;
using OutboxPatternDemo.Bookings.Contract.Events;
using OutboxPatternDemo.Bookings.FollowUpAppointments;
using OutboxPatternDemo.MedicalRecords.Contract.Events;
using OutboxPatternDemo.MedicalRecords.Contract.Models;
using Xunit;

namespace OutboxPatternDemo.Bookings.Tests.FollowUpAppointments;

public class AppointmentBookingSagaShould
{
    [Fact]
    public async Task CompleteWhenFollowUpNotRequired()
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
    public async Task NotCompleteWhenFollowUpRequired()
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
        Assert.Contains(message.Details.Id, followUpAppointmentSaga.Data.AppointmentsToFollowUpOn);
    }

    [Fact]
    public async Task CompleteWhenFollowUpBooked()
    {
        var loggerMock = new Mock<ILogger<FollowUpAppointmentSaga>>();
        var followUpAppointmentSaga = new FollowUpAppointmentSaga(loggerMock.Object)
        {
            Data = new FollowUpAppointmentSagaData()
        };
        var context = new TestableMessageHandlerContext();
        var message = new BookFollowUpAppointment(
            "Ben",
            new DateTime(1997, 07, 23)
            );

        await followUpAppointmentSaga.Handle(message, context);

        Assert.True(followUpAppointmentSaga.Completed);
    }

    [Fact]
    public async Task PublishEventWhenFollowUpBooked()
    {
        var loggerMock = new Mock<ILogger<FollowUpAppointmentSaga>>();
        var followUpAppointmentSaga = new FollowUpAppointmentSaga(loggerMock.Object)
        {
            Data = new FollowUpAppointmentSagaData
            {
                AppointmentsToFollowUpOn = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            }
        };
        var context = new TestableMessageHandlerContext();
        var message = new BookFollowUpAppointment(
            "Ben",
            new DateTime(1997, 07, 23)
            );

        await followUpAppointmentSaga.Handle(message, context);

        var publishedEvent = Assert.Single(context.PublishedMessages);
        var publishedFollowUpAppointmentBooked = Assert.IsType<FollowUpAppointmentBooked>(publishedEvent.Message);
        Assert.Equal(publishedFollowUpAppointmentBooked.AppointmentsToFollowUpOn, followUpAppointmentSaga.Data.AppointmentsToFollowUpOn);
    }
}
