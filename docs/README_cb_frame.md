# cb_FRAME – 3D Structural Analysis & Design Application

## Overview

**cb_FRAME** is the flagship 3D structural engineering application in the **CBDC Structural Design Suite**.

It is conceptually similar to tools like:
- RISA-3D
- ETABS
- RAM Elements

But the goals are:
- A modern, clean, dark, Revit-style UI
- Simple, understandable workflows
- Transparent calculations
- Extensibility (future apps: cb_element, cb_connection, cb_floor, cb_foundation, etc.)

cb_FRAME will serve both as:
1. A standalone analysis & design program, and  
2. The "engine host" that drives the shared CBDC structural libraries used by future apps.

---

## Core Capabilities (Target)

- 3D modeling of:
  - Nodes
  - Members (beams, columns, braces)
  - Plates, walls, solids (later phases)
  - Springs and links
- Loads:
  - Nodal loads, member loads, area/plate loads
  - Load cases, load combinations, and basic patterns
- Analysis:
  - Linear static analysis (initial)
  - P-Delta (later)
  - Modal analysis (later)
- Design:
  - Steel design (AISC)
  - Concrete design (ACI)
  - Wood, masonry, foundations (later modules)
- Results:
  - Deflected shape visualization
  - Member force diagrams
  - Basic tables and exportable reports

---

## Architecture Overview

cb_FRAME is built as a **multi-project .NET solution** with a clear separation between:

1. **Engine (shared)** – structural logic, math, design checks
2. **UI (cb_FRAME-specific)** – WPF desktop shell, 3D view, ribbon, panels

### ENGINE Projects (shared across future apps)

These projects contain no app-specific UI; they are reusable libraries:

- `CBFrame.Core` – domain model:
  - Nodes, elements, materials, sections, loads, results, units, etc.
- `CBFrame.Analysis` – structural analysis engine:
  - DOF mapping, stiffness matrices, solvers, analysis types
- `CBFrame.Design.Steel` – steel member design
- `CBFrame.Design.Concrete` – concrete member design
- `CBFrame.Design.Wood` – wood member design (later)
- `CBFrame.Design.Masonry` – masonry design (later)
- `CBFrame.Design.Foundation` – foundations design (later)
- `CBFrame.Loads` – wind, seismic, snow, live load helpers
- `CBFrame.SectionsDb` – sections database and lookups
- `CBFrame.MaterialsDb` – material database and lookups
- `CBFrame.Codes` – design/building code metadata
- `CBFrame.IO` – project file formats (.cbf), CSV/JSON helpers

### APP UI Project (cb_FRAME-specific)

- `CBFrame.App.Wpf` – WPF application:
  - Ribbon, docking/layout
  - Explorer, Properties, Data Entry panels
  - 3D viewport (HelixToolkit)
  - Results views and charts
  - Commands, tools, selection, undo/redo

### Test Projects

- `CBFrame.Tests.Unit` – unit tests for engine components
- `CBFrame.Tests.Integration` – small models tested end-to-end

---

## Technology Stack

- **Language:** C#
- **Runtime:** .NET 8
- **Desktop UI:** WPF
- **3D View:** HelixToolkit.Wpf.SharpDX
- **Data Storage:**
  - JSON (`.cbf` project files)
  - SQLite / JSON for shapes & materials databases
- **Patterns:**
  - MVVM (Model-View-ViewModel)
  - Clean, layered architecture (Engine vs UI)

---

## Development Flow (Phases)

Development is organized into phases:

- Phase 0: Environment & tooling setup (no code)
- Phase 1: Solution skeleton (projects only)
- Phase 2: Core domain model
- Phase 3: UI shell & layout
- Phase 4: Modeling tools – nodes & members
- Phase 5: Save/Load (.cbf)
- Phase 6: Linear static analysis
- Phase 7: Loads, load cases, combinations
- Phase 8: Sections & materials database
- Phase 9: Design modules (steel & concrete basics)
- Phase 10: Results, reporting & polish
- Phase 11: Testing, samples & installer

Each phase is handled in a **dedicated ChatGPT conversation** to keep focus and organization.

---
