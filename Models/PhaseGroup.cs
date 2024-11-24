using TOTools.StartggAPI;

namespace TOTools.Models;

/// <summary>
/// A startgg PhaseGroupType wrapper.
/// </summary>
public class PhaseGroup
{
    public PhaseGroupType PhaseGroupType { get; }
    public List<SetType> Sets { get; }

    /// <summary>
    /// Creates a phase group.
    /// </summary>
    /// <param name="phaseGroupType">The type of the phase group.</param>
    /// <param name="sets">The sets in the phase group.</param>
    public PhaseGroup(PhaseGroupType phaseGroupType, List<SetType> sets)
    {
        PhaseGroupType = phaseGroupType;
        Sets = sets;
    }
}