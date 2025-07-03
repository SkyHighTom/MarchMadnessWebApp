using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class Team
{
    public string TeamName { get; set; } = null!;

    public int Seed { get; set; }

    public virtual ICollection<FinalPrediction> FinalPredictionChampionNavigations { get; set; } = new List<FinalPrediction>();

    public virtual ICollection<FinalPrediction> FinalPredictionTeam1Navigations { get; set; } = new List<FinalPrediction>();

    public virtual ICollection<FinalPrediction> FinalPredictionTeam2Navigations { get; set; } = new List<FinalPrediction>();

    public virtual ICollection<FinalPrediction> FinalPredictionTeam3Navigations { get; set; } = new List<FinalPrediction>();

    public virtual ICollection<FinalPrediction> FinalPredictionTeam4Navigations { get; set; } = new List<FinalPrediction>();

    public virtual ICollection<Match> MatchTeam1Navigations { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchTeam2Navigations { get; set; } = new List<Match>();

    public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<Streak> Streaks { get; set; } = new List<Streak>();
}
