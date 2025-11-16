# cb_FRAME – Development Phases

This file documents the phased development plan for the **cb_FRAME** application.

Each phase is implemented in a dedicated ChatGPT conversation.

---

## Phase 0 – Environment & Tooling Setup

**Goal:** Ensure that the development machine, tools, SDKs, and folder structure are correct before creating any projects.

**Key items:**
- Install Visual Studio 2022 (with .NET desktop development workload)
- Install .NET 8 SDK (x64)
- Install Git and create a GitHub repo
- Confirm directory layout:
  - `D:\Projects\cb_frame\`
    - `src\`
    - `docs\`
    - `data\`
    - `samples\`
    - `tools\`
- Confirm external tools:
  - DB Browser for SQLite
  - (Optional) Figma or Adobe XD for UI mockups

---

## Phase 1 – Solution Skeleton (Projects Only)

**Goal:** Create the Visual Studio solution and all projects (no real code yet).

**Key steps:**
- Create `CBFrame.sln`
- Create ENGINE projects:
  - `CBFrame.Core`
  - `CBFrame.Analysis`
  - `CBFrame.Design.Steel`
  - `CBFrame.Design.Concrete`
  - `CBFrame.Design.Wood`
  - `CBFrame.Design.Masonry`
  - `CBFrame.Design.Foundation`
  - `CBFrame.Loads`
  - `CBFrame.SectionsDb`
  - `CBFrame.MaterialsDb`
  - `CBFrame.Codes`
  - `CBFrame.IO`
- Create APP project:
  - `CBFrame.App.Wpf`
- Create TEST projects:
  - `CBFrame.Tests.Unit`
  - `CBFrame.Tests.Integration`
- Set project references appropriately.

---

## Phase 2 – Core Domain Model

**Goal:** Define the core structural model in `CBFrame.Core` (no analysis yet).

**Key parts:**
- `FrameModel` root
- Geometry:
  - `Node`, `ElementBase`, `LineElement`, `PlateElement`, `WallElement`, `SolidElement`, `SpringElement`, `LinkElement`
- Loads:
  - `LoadCase`, `LoadCombination`
  - `NodalLoad`, `MemberPointLoad`, `MemberUniformLoad`, `PlatePressureLoad`, etc.
- Materials:
  - `MaterialBase`, `SteelMaterial`, `ConcreteMaterial`, `WoodMaterial`, `MasonryMaterial`, `AluminumMaterial`
- Sections:
  - `SectionBase`, `SteelSection`, `ConcreteSection`, etc.
- Results:
  - `NodeResult`, `ElementForceResult`, `ReactionResult`
- Enums:
  - `ElementType`, `DofType`, `AnalysisType`, `UnitSystem`, etc.
- Simple services:
  - `UnitConversionService`, `ModelValidationService`

---

## Phase 3 – UI Shell & Layout

**Goal:** Create the cb_FRAME WPF shell so the UI visually resembles the target layout (ribbon + panels + 3D view + status bar).

**Key parts:**
- `MainWindow.xaml` with:
  - Fluent.Ribbon at top
  - Left Properties panel
  - Center 3D viewport container
  - Right Explorer/Data Entry panels
  - Bottom status bar
- Views:
  - `PropertiesPanel`, `ExplorerPanel`, `DataEntryPanel`
  - `Model3DView`
- ViewModels:
  - `MainViewModel`, `ExplorerViewModel`, `PropertiesViewModel`, `DataEntryViewModel`, `Model3DViewModel`
- Basic dark theme resources (no full styling yet)

---

## Phase 4 – Modeling Tools (Nodes & Members)

**Goal:** Implement basic modeling tools for nodes and members.

**Key parts:**
- Tool framework:
  - `ITool`, `ToolController`
  - `SelectTool`, `DrawNodeTool`, `DrawMemberTool`
- Selection system:
  - `SelectionService`
- Undo/Redo:
  - `IUndoableAction`, `UndoRedoService`
- 3D scene:
  - `SceneManager`
  - `NodeVisual3D`, `MemberVisual3D`
- Wire Ribbon buttons to activate tools
- Allow user to:
  - draw nodes and members in the viewport
  - select elements
  - see selection reflected in Explorer + Properties

---

## Phase 5 – Save/Load (.cbf File Format)

**Goal:** Enable saving and loading project files.

**Key parts:**
- `CBFrame.IO`:
  - `CbfProjectSerializer`, `CbfProjectDeserializer`
- Define `.cbf` file structure using JSON
- `App.Wpf`:
  - `CurrentProjectService`
  - `DialogService` for Open/Save dialogs
  - `ModelCommands` for New/Open/Save/Save As
- Test workflows:
  - Create model → Save → Close → Open → Confirm restored

---

## Phase 6 – Linear Static Analysis

**Goal:** Implement a basic static analysis pipeline.

**Key parts:**
- `CBFrame.Analysis`:
  - DOF mapping, stiffness matrices, global assembly
  - Load vector, application of supports
  - Linear solver
  - `LinearStaticAnalysis` class
- `App.Wpf`:
  - `AnalysisService`
  - `AnalysisCommands` (Run Analysis)
  - Display deflected shape in 3D

---

## Phase 7 – Loads, Load Cases & Combinations

**Goal:** Complete load handling and UI for loads.

**Key parts:**
- Enrich load types in `Core`
- Support load cases and combos in the analysis pipeline
- Add panels:
  - `LoadCasesPanel`, `LoadCombinationsPanel`
- Commands to manage loads, cases, and combos from the UI

---

## Phase 8 – Sections & Materials Database

**Goal:** Integrate sections and materials database.

**Key parts:**
- JSON/SQLite data files in `data/sections` & `data/materials`
- `CBFrame.SectionsDb` & `CBFrame.MaterialsDb`:
  - providers and lookup services
- `App.Wpf`:
  - Section/material selection dialogs
  - Bind to member properties

---

## Phase 9 – Design Modules (Steel & Concrete Basics)

**Goal:** Add initial design checks and summary output.

**Key parts:**
- Steel design (AISC):
  - Basic flexure and axial checks
- Concrete design (ACI):
  - Basic beam flexure & shear
- `DesignService` to orchestrate
- `DesignSummaryPanel` to display results
- Wire Design tab buttons (Run Steel Design, Run Concrete Design)

---

## Phase 10 – Results, Reporting & Polish

**Goal:** Improve results visualization and reporting.

**Key parts:**
- 3D results:
  - Diagrams, contours (if applicable)
- Tabular results:
  - Displacements, forces, reactions
- Basic reporting:
  - Export CSV
  - Simple printable/HTML report
- UI polish:
  - Themes, icons, tooltips, quality of life improvements

---

## Phase 11 – Testing, Samples & Installer

**Goal:** Prepare cb_FRAME for distribution.

**Key parts:**
- Expand unit & integration test coverage
- Add sample `.cbf` models
- Create Windows installer (VS Installer, Inno Setup, or WiX)
- Versioning and first tagged release (e.g., `v0.1.0`)
