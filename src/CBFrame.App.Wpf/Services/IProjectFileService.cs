using CBFrame.App.Wpf.Model;

namespace CBFrame.App.Wpf.Services;

/// <summary>
/// Handles saving and loading the current frame model
/// to and from the .cbf file format.
/// </summary>
public interface IProjectFileService
{
    /// <summary>
    /// Create a new, empty document.
    /// </summary>
    FrameDocument CreateNewDocument();

    /// <summary>
    /// Save the given document to the specified file path.
    /// </summary>
    void SaveToFile(string filePath, FrameDocument document);

    /// <summary>
    /// Load a document from the specified file path.
    /// </summary>
    FrameDocument LoadFromFile(string filePath);
}
