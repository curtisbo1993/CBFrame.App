# cb_FRAME – Solution Layout (.sln Structure)

This document describes the intended `.sln` structure, including which projects are part of the **shared engine** and which are specific to the **cb_FRAME UI**.

---

## Solution Root

- `CBFrame.sln`
- `README_cb_frame.md`
- `PHASES_cb_frame.md`
- `docs/`
- `data/`
- `samples/`
- `tools/`
- `src/`

---

## Engine Projects (Shared)

These projects will eventually become the shared CBDC Structural Engine and be reused by other apps (cb_element, cb_connection, etc.).

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

These projects **must not** contain any UI-specific logic.

---

## cb_FRAME UI Project (App-Specific)

- `CBFrame.App.Wpf`

This is the WPF desktop application that hosts:

- Ribbon
- 3D viewport
- Explorer panel
- Properties panel
- Data entry panels
- Result views
- Commands and tools for modeling

This project is **specific to cb_FRAME** and will not be reused directly by other apps.

---

## Test Projects

- `CBFrame.Tests.Unit`
  - Unit tests for the engine (Core, Analysis, Design, Loads, etc.)
- `CBFrame.Tests.Integration`
  - End-to-end tests that build small example models and run analyses/designs

---

## Example Tree Layout (Simplified)

```text
CBFrame.sln
src/
  CBFrame.App.Wpf/
  CBFrame.Core/
  CBFrame.Analysis/
  CBFrame.Design.Steel/
  CBFrame.Design.Concrete/
  CBFrame.Design.Wood/
  CBFrame.Design.Masonry/
  CBFrame.Design.Foundation/
  CBFrame.Loads/
  CBFrame.SectionsDb/
  CBFrame.MaterialsDb/
  CBFrame.Codes/
  CBFrame.IO/
  CBFrame.Tests.Unit/
  CBFrame.Tests.Integration/
data/
  sections/
  materials/
  codes/
samples/
  *.cbf
docs/
  README_cb_frame.md
  PHASES_cb_frame.md
  solution-layout_cb_frame.md
