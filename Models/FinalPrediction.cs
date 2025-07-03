using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class FinalPrediction
{
    public string Team1 { get; set; } = null!;

    public string Team2 { get; set; } = null!;

    public string Team3 { get; set; } = null!;

    public string Team4 { get; set; } = null!;

    public string Champion { get; set; } = null!;

    public string Username { get; set; } = null!;

    public virtual Team ChampionNavigation { get; set; } = null!;

    public virtual Team Team1Navigation { get; set; } = null!;

    public virtual Team Team2Navigation { get; set; } = null!;

    public virtual Team Team3Navigation { get; set; } = null!;

    public virtual Team Team4Navigation { get; set; } = null!;

    public virtual User UsernameNavigation { get; set; } = null!;
}
