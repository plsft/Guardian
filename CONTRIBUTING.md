# Contributing to Guardian

We love your input! We want to make contributing to Guardian as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer

## We Develop with Github
We use GitHub to host code, to track issues and feature requests, as well as accept pull requests.

## We Use [Github Flow](https://guides.github.com/introduction/flow/index.html)
Pull requests are the best way to propose changes to the codebase. We actively welcome your pull requests:

1. Fork the repo and create your branch from `main`.
2. If you've added code that should be tested, add tests.
3. If you've changed APIs, update the documentation.
4. Ensure the test suite passes.
5. Make sure your code follows the existing style.
6. Issue that pull request!

## Any contributions you make will be under the MIT Software License
In short, when you submit code changes, your submissions are understood to be under the same [MIT License](http://choosealicense.com/licenses/mit/) that covers the project. Feel free to contact the maintainers if that's a concern.

## Report bugs using Github's [issues](https://github.com/yourusername/guardian/issues)
We use GitHub issues to track public bugs. Report a bug by [opening a new issue](https://github.com/yourusername/guardian/issues/new); it's that easy!

## Write bug reports with detail, background, and sample code

**Great Bug Reports** tend to have:

- A quick summary and/or background
- Steps to reproduce
  - Be specific!
  - Give sample code if you can.
- What you expected would happen
- What actually happens
- Notes (possibly including why you think this might be happening, or stuff you tried that didn't work)

## Development Process

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022, Visual Studio Code, or JetBrains Rider
- Git

### Getting Started
1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/guardian.git
   cd guardian
   ```

2. Restore dependencies
   ```bash
   dotnet restore
   ```

3. Build the solution
   ```bash
   dotnet build
   ```

4. Run tests
   ```bash
   dotnet test
   ```

### Project Structure
```
Guardian/
├── src/
│   └── Guardian/          # Main library project
├── tests/
│   └── Guardian.Tests/    # Unit tests
├── samples/
│   ├── Guardian.Samples.Console/  # Console app samples
│   └── Guardian.Samples.WebApi/   # Web API samples
└── docs/                  # Documentation
```

### Adding New Guard Clauses

When adding a new guard clause:

1. Add the method signature to the `IGuardClause` interface
2. Implement the method in the `GuardClause` class
3. Add comprehensive unit tests
4. Update the README with usage examples
5. Add XML documentation comments

Example:
```csharp
// In IGuardClause interface
T MyNewGuard<T>(T value, string? parameterName = null, string? message = null);

// In GuardClause class
public T MyNewGuard<T>(T value, string? parameterName, string? message)
{
    // Implementation
}
```

### Testing Guidelines

- Write tests for both success and failure cases
- Use descriptive test names following the pattern: `MethodName_Scenario_ExpectedResult`
- Aim for >90% code coverage
- Use FluentAssertions for assertions
- Group related tests in the same test class

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for all public APIs
- Keep methods small and focused
- Use guard clauses at the beginning of methods
- Prefer explicit types over `var` for clarity

### Commit Messages

- Use clear and meaningful commit messages
- Start with a verb in present tense (Add, Fix, Update, Remove)
- Keep the first line under 50 characters
- Add detailed description if needed after a blank line

Examples:
```
Add guard clause for email validation

Add InvalidEmail guard clause that validates email format
using a regex pattern. Includes tests and documentation.
```

## Pull Request Process

1. Update the README.md with details of changes if applicable
2. Update the XML documentation
3. Add or update tests as needed
4. Ensure all tests pass
5. Update the sample projects if relevant
6. The PR will be merged once you have the sign-off of at least one maintainer

## License
By contributing, you agree that your contributions will be licensed under its MIT License.

## References
This document was adapted from the open-source contribution guidelines template.