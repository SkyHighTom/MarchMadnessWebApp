using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class Match
{
    public int MatchId { get; set; }

    public string Team1 { get; set; } = null!;

    public string Team2 { get; set; } = null!;

    public int Round { get; set; }

    public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();

    public virtual Result? Result { get; set; }

    public virtual Team Team1Navigation { get; set; } = null!;

    public virtual Team Team2Navigation { get; set; } = null!;
}
