namespace CS341Project.Models;

/// <summary>
/// An expandable group of players sharing the same tier.
/// </summary>
/// <param name="tier">The tier of the player group.</param>
/// <param name="players">The players in the given tier.</param>
public class PlayerTierGroup(
    string tier,
    List<Player> players
) : List<Player>(players)
{
    public string Tier { get; } = tier;

    public bool Expanded { get; private set; } = true;

    public void ToggleExpanded()
    {
        if (!Expanded)
        {
                AddRange(players);
        }
        else
        {
            Clear();
        }
        Expanded = !Expanded;
    }
}