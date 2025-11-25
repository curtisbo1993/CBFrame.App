using System;
using Microsoft.Win32;

namespace CBFrame.App.Wpf.Services;

/// <summary>
/// WPF implementation of IDialogService using OpenFileDialog / SaveFileDialog.
/// </summary>
public sealed class DialogService : IDialogService
{
    private const string ProjectFilter = "cb_FRAME Project (*.cbf)|*.cbf|All Files (*.*)|*.*";
    private const string DefaultExt = ".cbf";

    public string? ShowOpenProjectDialog()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open cb_FRAME Project",
            Filter = ProjectFilter,
            DefaultExt = DefaultExt,
            CheckFileExists = true,
            CheckPathExists = true
        };

        bool? result = dialog.ShowDialog();
        return result == true ? dialog.FileName : null;
    }

    public string? ShowSaveProjectDialog(string? suggestedFileName = null)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Save cb_FRAME Project As",
            Filter = ProjectFilter,
            DefaultExt = DefaultExt,
            AddExtension = true,
            OverwritePrompt = true,
            FileName = string.IsNullOrWhiteSpace(suggestedFileName)
                ? "Untitled.cbf"
                : suggestedFileName.EndsWith(DefaultExt, StringComparison.OrdinalIgnoreCase)
                    ? suggestedFileName
                    : suggestedFileName + DefaultExt
        };

        bool? result = dialog.ShowDialog();
        return result == true ? dialog.FileName : null;
    }
}
