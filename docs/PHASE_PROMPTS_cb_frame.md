
---

## 4️⃣ `PHASE_PROMPTS_cb_frame.md`

```markdown
# cb_FRAME – Phase Chat Prompts (for ChatGPT)

Use these prompts when starting a **new ChatGPT conversation** for each phase of cb_FRAME.

Each chat should only focus on **one phase** from start to finish.

---

## Phase 0 – Environment & Tooling Setup

**Chat title:** `cb_FRAME – Phase 0 (Setup & Environment)`

**Prompt:**

You are now working on the cb_FRAME structural engineering desktop app.  
This chat is dedicated ONLY to **PHASE 0: Environment & Tooling Setup**.  
Do not jump ahead to later phases.

Your tasks in this chat:
1. Confirm the tools I need (Visual Studio 2022, .NET 8 SDK, Git, etc.).
2. Confirm the folder structure for the cb_FRAME project.
3. Confirm any external utilities (SQLite viewer, design tools).
4. Make sure everything is correctly set up before I start creating the solution in Phase 1.

When Phase 0 is complete, explicitly tell me:  
"Phase 0 is complete. You can now start a new chat for Phase 1."

Begin Phase 0 now.

---

## Phase 1 – Solution Skeleton

**Chat title:** `cb_FRAME – Phase 1 (Solution Skeleton)`

**Prompt:**

You are now working on the cb_FRAME structural engineering desktop app.  
This chat is dedicated ONLY to **PHASE 1: Solution Skeleton (Projects Only)**.

Your tasks in this chat:
1. Help me create the CBFrame.sln solution.
2. Create all projects listed in the solution layout for ENGINE, APP, and TESTS.
3. Set proper target frameworks (e.g., .NET 8).
4. Set project references correctly (App references Engine, Tests reference Engine).

Do NOT add real implementation code yet.  
Do NOT jump to later phases.

When Phase 1 is complete, explicitly tell me:  
"Phase 1 is complete. You can now start a new chat for Phase 2."

Begin Phase 1 now.

---

## Phase 2 – Core Domain Model

**Chat title:** `cb_FRAME – Phase 2 (Core Domain Model)`

**Prompt:**

You are now working on the cb_FRAME structural engineering desktop app.  
This chat is dedicated ONLY to **PHASE 2: Core Domain Model**.

Your tasks:
1. Define all core model classes in CBFrame.Core.
2. Create the basic structure for nodes, elements, loads, materials, sections, results, and enums.
3. Keep implementation simple (properties, basic validation, no analysis math).
4. Provide code and explain each major type so I understand its purpose.

Do NOT implement analysis or UI here.  
Do NOT jump to later phases.

When Phase 2 is complete, explicitly tell me:  
"Phase 2 is complete. You can now start a new chat for Phase 3."

Begin Phase 2 now.

---

## Phase 3 – UI Shell & Layout

(Similar pattern – you already know the rest; continue the same format for phases 3–11.)

You can expand:

- Phase 3 – UI Shell & Layout
- Phase 4 – Modeling Tools
- Phase 5 – Save/Load
- Phase 6 – Analysis
- Phase 7 – Loads & Combos
- Phase 8 – Sections & Materials DB
- Phase 9 – Design Modules
- Phase 10 – Results & Reporting
- Phase 11 – Testing & Installer

Follow the same structure:
- Explain scope
- Say “this chat is ONLY for Phase X”
- List responsibilities
- Tell the assistant not to jump ahead
- Ask it to explicitly mark the phase as complete when done

---
