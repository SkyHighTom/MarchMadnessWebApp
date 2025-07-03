using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class Prediction
{
    public string Username { get; set; } = null!;

    public int MatchId { get; set; }

    public string Winner { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;

    public virtual User UsernameNavigation { get; set; } = null!;

    public virtual Team WinnerNavigation { get; set; } = null!;
}
