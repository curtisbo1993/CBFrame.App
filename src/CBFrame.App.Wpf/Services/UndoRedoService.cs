using System;
using System.Collections.Generic;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Default implementation of IUndoRedoService using two stacks.
    /// </summary>
    public class UndoRedoService : IUndoRedoService
    {
        private readonly Stack<IUndoableCommand> _undoStack = new();
        private readonly Stack<IUndoableCommand> _redoStack = new();

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public event EventHandler? StateChanged;

        public void Push(IUndoableCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();

            OnStateChanged();
        }

        public void Undo()
        {
            if (!CanUndo)
                return;

            var cmd = _undoStack.Pop();
            cmd.Undo();
            _redoStack.Push(cmd);

            OnStateChanged();
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            var cmd = _redoStack.Pop();
            cmd.Execute();
            _undoStack.Push(cmd);

            OnStateChanged();
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            OnStateChanged();
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
