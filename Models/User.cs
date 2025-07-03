using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public int Score { get; set; }

    public virtual FinalPrediction? FinalPrediction { get; set; }

    public virtual LargestUpset? LargestUpset { get; set; }

    public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();

    public virtual ICollection<Streak> Streaks { get; set; } = new List<Streak>();
}
