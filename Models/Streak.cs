using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class Streak
{
    public string Username { get; set; } = null!;

    public string TeamName { get; set; } = null!;

    public int Streak1 { get; set; }

    public virtual Team TeamNameNavigation { get; set; } = null!;

    public virtual User UsernameNavigation { get; set; } = null!;
}
