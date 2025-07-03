using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class Result
{
    public int MatchId { get; set; }

    public string Winner { get; set; } = null!;

    public int Team1Score { get; set; }

    public int Team2Score { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team WinnerNavigation { get; set; } = null!;
}
