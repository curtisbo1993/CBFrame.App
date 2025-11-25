namespace CBFrame.IO.Serialization;

/// <summary>
/// Serializable representation of a node in the frame model.
/// </summary>
public sealed class CbfNodeDto
{
    public int Id { get; set; }

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    /// <summary>
    /// Optional restraint descriptor (e.g., "Fixed", "Pinned", "Free", or a code you define).
    /// For now it's just a string; you can refine later.
    /// </summary>
    public string? Restraint { get; set; }
}
