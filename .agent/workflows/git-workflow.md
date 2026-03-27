---
description: Enterprise Git workflow for feature development with Conventional Commits
---

# Git Enterprise Workflow

## Branch Strategy
```bash
# Always start from updated main
git checkout main
git pull origin main

# Create feature branch
git checkout -b feat/feature-name
```

## Commit Convention (Conventional Commits)
```
feat(scope): add new feature
fix(scope): fix specific bug
refactor(scope): restructure without behavior change
docs(scope): update documentation
chore(scope): maintenance tasks
```

## Push & PR Flow
```bash
# Stage and commit
git add .
git commit -m "feat(blog): add dynamic SEO meta tags"

# Push feature branch
git push -u origin feat/feature-name

# After PR is merged on GitHub:
git checkout main
git pull origin main
git branch -d feat/feature-name
```

## Rules
1. **Never push directly to main** — always use PRs
2. **One feature per branch** — keeps PRs reviewable
3. **Squash commits** if branch has many small commits
4. **Delete branch after merge** — keeps repo clean
