# .github Folder Guide

This folder contains AI collaboration assets and project-specific guidance.

## Structure

- `agents/` — specialized AI agent profiles for focused tasks.
- `instructions/` — concise repository rules used by coding agents.
- `prompts/` — reusable prompt templates for common workflows.
- `copilot/` — reserved for future Copilot-specific assets.
- `workflows/` — reserved for GitHub Actions workflows.

## Naming Conventions

- Instruction files: `*.instructions.md`
- Prompt files: `*.prompt.md`
- Agent files: `*.agent.md`

## Current Source of Truth

- General rules: `instructions/general.instructions.md`
- Backend rules: `instructions/backend.instructions.md`
- Frontend rules: `instructions/frontend.instructions.md`
- Main project prompt: `prompts/default.prompt.md`

## Maintenance Rules

- Keep rules aligned with real repository tooling and frameworks.
- Prefer small, concrete changes over generic enterprise directives.
- Avoid contradictory guidance between prompts and instructions.
- Update this folder docs when stack, test framework, or conventions change.
