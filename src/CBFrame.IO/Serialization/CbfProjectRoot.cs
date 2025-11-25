using System.Collections.Generic;

namespace CBFrame.IO.Serialization;

/// <summary>
/// Root object for a .cbf file. 
/// Keeps versioning and the actual project payload.
/// </summary>
public sealed class CbfProjectRoot
{
    /// <summary>
    /// Version of the .cbf format. Start at 1.
    /// </summary>
    public int CbfVersion { get; set; } = 1;

    /// <summary>
    /// The actual project data (nodes, members, etc.).
    /// </summary>
    public CbfProjectDto Project { get; set; } = new CbfProjectDto();
}

/// <summary>
/// Project-level data: basic metadata + collections of model entities.
/// </summary>
public sealed class CbfProjectDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public IList<CbfNodeDto> Nodes { get; set; } = new List<CbfNodeDto>();
    public IList<CbfMemberDto> Members { get; set; } = new List<CbfMemberDto>();

    // Later phases can add:
    // - Load cases
    // - Nodal loads
    // - Member loads
    // - Combinations, etc.
}
