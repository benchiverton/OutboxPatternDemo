# Outbox Pattern

## The problem

Often when the state of a business entity changes, you want to publish an event stating that this has happened so that other microservices are in sync. To achieve this, you would do the following:

1. Receive message
2. Update business entity
3. Publish 'EntityUpdated' event

However, if step (2) is not idempotent and only step (3) fails, we find ourselves in a scenario where the business entity has been updated an other microservices are not aware of this change.

## One (of many) solutions

The Outbox Pattern solves this problem by moving the event publisher into an outbox processor. The command service then updates the entity and adds a record to the outbox messages in the same transaction. The outbox processor will poll this outbox messages for any unprocessed messages, and publish them as they arrive. This is illustrated in the diagram below:

![Diagram](docs/OutboxPatternDiagram.png)

## This project

This project demo's the Outbox Pattern using NServiceBus (In Memory) and EntityFramework (SQLite). It contains 3 apps:

| App                          | Purpose                                                      |
| ---------------------------- | ------------------------------------------------------------ |
| [OutboxPatternDemo.MedicalRecords](/src/OutboxPatternDemo.MedicalRecords/) | ASP.NET Web API that allows you to update medical records, and uses the Outbox Pattern to guarantee that update events are published at least once |
| [OutboxPatternDemo.Bookings](/src/OutboxPatternDemo.Bookings/)  | Console App that subscribes to and de-duplicates messages, and uses a saga to book follow up appointments |
| [OutboxPatternDemo.Monitoring]((/src/OutboxPatternDemo.Monitoring/)) | Runs the Particular Service Platform, allowing you to monitor events within the system |

### Outbox Pattern Implementations

#### [NServiceBus](/src/OutboxPatternDemo.MedicalRecords/Outboxes/NServiceBus/)
* Implemented using NSB's built-in outbox pattern feature,
* NSB configuration in [Program.cs](/src/OutboxPatternDemo.MedicalRecords/Program.cs),
* **Drawback:** requires NSB license.

#### [Custom](/src/OutboxPatternDemo.MedicalRecords/Outboxes/Custom/)
* `CustomOutboxContext` - the DB context for messages to be published,
* `CustomOutboxMessageBus` - the message bus writing to `CustomOutboxContext`,
* `CustomOutboxProcessor` - the background process reading from `CustomOutboxContext` and publishing messages,
* **Drawback:** this is intended for educational purposes only, I'd recommend using commercial software (e.g. NSB, Mass Transit) for anything other than a hobby project.

### Duplicate Checker Implementations

#### [Circular Buffer](/src/OutboxPatternDemo.Bookings/DuplicateCheckers/CircularBuffer/)
* Adds duplicates to a cirular buffer (effectively caching the last X messages which it checks against),
* **Pros:** efficient as in-memory,
* **Cons:** ineffective in high throughput / 'spikey' systems, does not scale horizontally.

#### [Distributed Cache](/src/OutboxPatternDemo.Bookings/DuplicateCheckers/DistributedCache/)
* Stores duplicate keys in a distributed cache (e.g. Redis)
* **Pros:** scales horizontally, configurable sliding expiration,
* **Cons:** requires additional infrastructure, does not scale well for high throughput / large sliding expiration scenarios.

#### [SQL](/src/OutboxPatternDemo.Bookings/DuplicateCheckers/Sql/)
* Records message ID's in a table which it checks against,
* **Pros:** scales horizontally, scales well with high throughput / large sliding window scenarios,
* **Cons:** requires additional infrastructure, SQL DB requires management.

### Getting Started

Please read the docs [here](/docs/getting-started.md) to set up this project.
