using TOTools.Models.Startgg;

namespace TOTools.Models;

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