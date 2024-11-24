namespace TOTools.Models;

/// <summary>
/// A group of phase groups of a single type.
/// </summary>
/// <param name="phaseGroupType">The type of the phase groups.</param>
/// <param name="identifiers">The identifiers of each phase group.</param>
public class BracketGroup(string phaseGroupType, ICollection<string> identifiers) : List<string>(identifiers)
{
    private bool Expanded { get; set; } = true;

    public string PhaseGroupType { get; set; } = phaseGroupType;

    public void ToggleExpanded()
    {
        if (!Expanded)
        {
            AddRange(identifiers);
        }
        else
        {
            Clear();
        }

        Expanded = !Expanded;
    }
}