# Getting Started

All of the code is in the `src` folder. It requires the dotnet 8 SDK to run.

## NServiceBus

Get your free development license from: https://particular.net/license/nservicebus?t=0&p=servicepulse

Then, copy your `license.xml` file to `%LocalAppData%/ParticularSoftware`. You may need to restart VS (or potentially your PC) for the license file to be picked up. This will be required NSB monitoring.

## Entity Framework

The underlying data source for outbox events uses SQL Server. Before you can run the OutboxPatternDemo.MedicalRecords/Bookings apps, you need to create a local database called `OutboxPatternDemo`.

First, verify that Entity Framework is installed:

```powershell
# install + update Entity Framework tool
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

Then, run the following commands from a PowerShell terminal in the `src/OutboxPatternDemo.MedicalRecords` directory:

```powershell
# create contexts
dotnet ef database update --context MedicalRecordContext
dotnet ef database update --context CustomOutboxContext
```

and the following commands from the `src/OutboxPatternDemo.Bookings` directory:

```powershell
# create contexts
dotnet ef database update --context DuplicateKeyContext
```

### Updating migrations

First, verify that Entity Framework is installed:

```powershell
# install + update Entity Framework tool
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

`src/OutboxPatternDemo.MedicalRecords`:

```powershell
# create contexts
dotnet ef migrations add <migrationname> --context MedicalRecordContext
dotnet ef migrations add <migrationname> --context CustomOutboxContext
```

`src/OutboxPatternDemo.Bookings`:

```powershell
# create contexts
dotnet ef migrations add <migrationname> --context DuplicateKeyContext
```
