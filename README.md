# 001-MNM

A .NET 8 console app that runs as an MCP (Model Context Protocol) server and exposes monkey data tools, with a unit test project and a GitHub Actions pipeline.

## Project structure

```text
ConsoleApp/
├── ConsoleApp.sln
├── MCPDotNetSample/          # MCP server console app
│   ├── Program.cs            # Host setup and MCP server registration
│   ├── MonkeyService.cs      # Data access (monkey list, data filter API)
│   └── MonkeyTools.cs        # MCP tools exposed to clients
└── MCPDotNetSample.Tests/    # xUnit unit tests
```

## Getting started

Requirements: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
dotnet restore ConsoleApp/ConsoleApp.sln
dotnet build ConsoleApp/ConsoleApp.sln
```

## Running the tests

```bash
dotnet test ConsoleApp/ConsoleApp.sln
```

## CI/CD

The workflow in `.github/workflows/build-and-test.yaml` runs on every push and pull request to `main`. It restores, builds and tests the solution. The `main` branch is protected, so changes must go through a pull request and the `build-and-test` check must pass before merging.
