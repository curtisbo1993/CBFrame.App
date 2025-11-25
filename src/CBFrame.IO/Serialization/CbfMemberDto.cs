namespace CBFrame.IO.Serialization;

/// <summary>
/// Serializable representation of a member/element between two 3D points.
/// For now we store pure geometry (no section/material yet).
/// </summary>
public sealed class CbfMemberDto
{
    public int Id { get; set; }

    // I-end coordinates
    public double IX { get; set; }
    public double IY { get; set; }
    public double IZ { get; set; }

    // J-end coordinates
    public double JX { get; set; }
    public double JY { get; set; }
    public double JZ { get; set; }

    // Later we can add:
    // public string? Section { get; set; }
    // public string? Material { get; set; }
}
