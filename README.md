# FIX Simulator — Mini S&P500 Futures Order Book

A simulated FIX 4.2 order book for the CME Mini S&P500 (ES) futures contract,
implementing mock messaging between fictional traders and a simulated exchange.

## Architecture

Hexagonal Architecture (Ports and Adapters) with strict inward-only dependencies:

FixSimulator.Domain          → pure domain model, no dependencies
FixSimulator.MatchingEngine  → order book + matching logic
FixSimulator.Fix             → FIX 4.2 protocol encode/decode + session
FixSimulator.Exchange        → TCP server (exchange process)
FixSimulator.Trader          → TCP client (trader process)

## Requirements

- .NET 10 SDK
- JetBrains Rider (recommended) or VS Code with C# Dev Kit

## Build

```bash
dotnet build fix-simulator.slnx
```

## Test

```bash
dotnet test fix-simulator.slnx
```

## Run

```bash
# Terminal 1 — start the exchange
dotnet run --project src/FixSimulator.Exchange

# Terminal 2 — start a trader
dotnet run --project src/FixSimulator.Trader
```
