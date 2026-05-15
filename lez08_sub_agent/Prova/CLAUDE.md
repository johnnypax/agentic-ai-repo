# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
npm run dev       # Start dev server with HMR at http://localhost:5173
npm run build     # Production build (outputs to dist/)
npm run preview   # Preview production build locally
npm run lint      # Run ESLint on all JS/JSX files
```

## Architecture

This is a minimal React 19 + Vite 8 single-page app scaffold.

- `src/main.jsx` — entry point, mounts `<App>` into `#root`
- `src/App.jsx` — single root component (all app logic lives here for now)
- `src/App.css` / `src/index.css` — component and global styles
- `public/` — static assets served as-is (SVG icon sprite at `public/icons.svg`)
- `src/assets/` — assets imported directly in JSX (processed by Vite)

Icons are loaded via an SVG sprite (`<use href="/icons.svg#icon-name">`), not as individual imports.

ESLint is configured with `eslint-plugin-react-hooks` (enforces hooks rules) and `eslint-plugin-react-refresh` (prevents non-component default exports that break HMR).
