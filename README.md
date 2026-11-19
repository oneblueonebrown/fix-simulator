# FIX Simulator

A simulated exchange for the CME E-mini S&P 500 (ES) futures contract. Fictional
traders connect over the FIX 4.2 protocol, submit and cancel orders, and receive
execution reports from a price-time priority matching engine.

> **Status:** early development. The solution is scaffolded and the
> infrastructure runs; domain and matching logic are in progress.

## Stack

| Component | Purpose |
|---|---|
| .NET 10 / C# | All application code |
| FIX 4.2 (hand-rolled) | Trader ↔ exchange protocol over TCP |
| RabbitMQ | Order commands and execution reports between services |
| Redis | FIX sequence numbers, order de-duplication, market data cache |
| Docker Compose | Runs RabbitMQ and Redis locally |
| xUnit | Unit tests |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Docker: Docker Desktop on Windows and macOS, or Docker Engine on Linux

## Getting started

### 1. Start the infrastructure

```bash
docker compose up -d
docker compose ps        # wait until both services report "healthy"
```

| Service | Address | Credentials |
|---|---|---|
| RabbitMQ (AMQP) | `localhost:5672` | `fixsim` / `fixsim` |
| RabbitMQ management UI | http://localhost:15672 | `fixsim` / `fixsim` |
| Redis | `localhost:6379` | none |

To stop the services while keeping their data:

```bash
docker compose stop
```

> ⚠️ `docker compose down -v` also deletes the RabbitMQ and Redis volumes,
> wiping all stored data.

### 2. Build and test

```bash
dotnet build fix-simulator.slnx
dotnet test fix-simulator.slnx
```

## Project layout

```
src/
  FixSimulator.Domain           Core types: orders, prices, quantities, trades. No dependencies.
  FixSimulator.MatchingEngine   Order book and price-time priority matching.
  FixSimulator.Application      Ports (interfaces) and command handling that drives the engine.
  FixSimulator.Fix              FIX 4.2 message encoding, decoding and session management.
  FixSimulator.Gateway          TCP server that traders connect to.
  FixSimulator.Trader           Simulated trader client.
tests/
  FixSimulator.*.Tests          Unit tests, one project per library.
```

Dependencies point inward, toward `Domain` (hexagonal architecture). Infrastructure
such as RabbitMQ and Redis is plugged in through interfaces defined in
`Application`, so the core logic never depends on it directly.

## Repository conventions

- `global.json` pins the .NET SDK to 10.0.x.
- `Directory.Build.props` holds build settings shared by every project.
  Warnings are treated as errors.
- `nuget.config` makes nuget.org the only package source.
- `.gitattributes` and `.editorconfig` enforce LF line endings on every OS.
