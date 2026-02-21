---
description: project coding guidelines — auto-loaded each session
---

# Guidelines

## Angular
- Standalone components; `OnPush` always; lazy-load all routes
- Signals: `signal()`, `computed()`, `effect()` — no `BehaviorSubject`
- Inputs/outputs: `input()` / `output()` — not `@Input`/`@Output`
- Templates: `@if`/`@for` not `*ngIf`/`*ngFor`; no function calls in templates
- HTTP in services only; typed responses; functional interceptors
- DI: use `inject()`, `providedIn: 'root'`
- Subs: `toSignal()` / `async` pipe; `takeUntilDestroyed()` for manual subs

## TypeScript
- No `any`; `strict: true`; `?.` and `??` for null safety
- Named exports only; `import type` for type imports
- `async/await`; no `!` assertions; no `eslint-disable`

## Styling
- Always separate component into 3 files: `.ts`, `.html`, `.css` (no inline styles/templates)
- **Tailwind first** — use utility classes for everything possible
- Write custom CSS only when Tailwind cannot achieve the result (e.g. complex animations, pseudo-elements)
- No inline `style=""` attributes in templates

## General
- KISS/DRY/YAGNI; guard clauses; no magic numbers
- Comments explain *why*; `feat(scope): msg` commits; never push to `main`
- Validate all external inputs; secrets in env vars only
