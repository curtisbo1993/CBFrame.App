namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Represents a single undoable operation (e.g., add node, delete member).
    /// </summary>
    public interface IUndoableCommand
    {
        /// <summary>
        /// Short label for UI (e.g., "Add Node").
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Perform the action.
        /// </summary>
        void Execute();

        /// <summary>
        /// Undo the action.
        /// </summary>
        void Undo();
    }
}
