using System;

namespace CBFrame.App.Wpf.Services;

/// <summary>
/// Shows file dialogs to pick project files (.cbf).
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Show an "Open Project" dialog.
    /// Returns the selected file path, or null if the user cancels.
    /// </summary>
    string? ShowOpenProjectDialog();

    /// <summary>
    /// Show a "Save Project As" dialog.
    /// Returns the selected file path, or null if the user cancels.
    /// </summary>
    /// <param name="suggestedFileName">
    /// Optional file name suggestion (e.g. current project name).
    /// </param>
    string? ShowSaveProjectDialog(string? suggestedFileName = null);
}
