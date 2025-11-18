using System;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Manages a stack of undoable commands and supports undo/redo.
    /// </summary>
    public interface IUndoRedoService
    {
        /// <summary>
        /// True if there is at least one command that can be undone.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// True if there is at least one command that can be redone.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Raised whenever the internal undo/redo stacks change,
        /// so the UI can update CanUndo/CanRedo bindings.
        /// </summary>
        event EventHandler? StateChanged;

        /// <summary>
        /// Executes the command and pushes it onto the undo stack.
        /// Clears the redo stack.
        /// </summary>
        void Push(IUndoableCommand command);

        /// <summary>
        /// Undo the last command, if any.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the last undone command, if any.
        /// </summary>
        void Redo();

        /// <summary>
        /// Clears all undo and redo history.
        /// </summary>
        void Clear();
    }
}
