## Development Setup

### Git Hooks
This repository uses Git hooks for code quality checks. To enable them:

```bash
git config core.hooksPath .git-hooks
```

The pre-commit hook will:
- Format code using dotnet format
- Run all tests