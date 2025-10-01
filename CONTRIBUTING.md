# Contributing to HeyGPT

Thank you for your interest in contributing to HeyGPT! We welcome contributions from the community.

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:
- Clear description of the problem
- Steps to reproduce
- Expected vs actual behavior
- System information (Windows version, .NET version)
- Screenshots if applicable

### Suggesting Features

Feature requests are welcome! Please:
- Check if the feature already exists or has been requested
- Describe the feature and why it would be useful
- Provide examples of how it would work

### Code Contributions

1. **Fork the Repository**
   ```bash
   git clone https://github.com/yourusername/HeyGPT.git
   cd HeyGPT
   ```

2. **Create a Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**
   - Follow the existing code style
   - Add comments for complex logic
   - Update documentation if needed

4. **Test Your Changes**
   ```bash
   cd ChatGptVoiceAssistant
   dotnet build
   dotnet run
   ```

5. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "Add: Description of your changes"
   ```

6. **Push and Create PR**
   ```bash
   git push origin feature/your-feature-name
   ```
   Then create a Pull Request on GitHub

## Code Style Guidelines

### C# Conventions
- Use PascalCase for public members
- Use camelCase for private fields with underscore prefix (`_fieldName`)
- Use meaningful variable names
- Add XML documentation comments for public APIs

### XAML Conventions
- Use x:Name for elements that need code-behind access
- Group related properties together
- Use static resources for colors and styles

### Commit Messages
- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit first line to 72 characters
- Reference issues and PRs when applicable

## Project Structure

```
HeyGPT/
├── Assets/                 # Icons and images
├── ChatGptVoiceAssistant/
│   ├── Models/            # Data models
│   ├── Services/          # Business logic
│   ├── ViewModels/        # MVVM view models
│   ├── Views/             # WPF windows
│   └── Resources/         # Themes and styles
├── .gitignore
├── LICENSE
├── README.md
└── CONTRIBUTING.md
```

## Development Setup

### Prerequisites
- Windows 10/11
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code with C# extension
- Git

### Building
```bash
cd ChatGptVoiceAssistant
dotnet restore
dotnet build
```

### Running
```bash
dotnet run
```

## Testing

Before submitting a PR, please test:
- Wake word detection functionality
- Monitor configuration
- Button position capture
- Settings persistence
- Window animations
- System tray integration

## Areas for Contribution

### High Priority
- [ ] Unit tests for core services
- [ ] Internationalization (i18n) support
- [ ] Custom wake word training
- [ ] Performance optimizations

### Medium Priority
- [ ] Additional theme options
- [ ] Hotkey support
- [ ] Command history
- [ ] Export/import settings

### Low Priority
- [ ] Plugin system
- [ ] Voice command recognition
- [ ] Alternative AI assistants support

## Questions?

Feel free to:
- Open an issue for questions
- Start a discussion on GitHub Discussions
- Comment on existing issues/PRs

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

Thank you for contributing to HeyGPT! 🎉
