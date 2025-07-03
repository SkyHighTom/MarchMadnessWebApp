using System;
using System.Collections.Generic;

namespace MarchMadnessWebApp.Models;

public partial class LargestUpset
{
    public string Username { get; set; } = null!;

    public int Diff { get; set; }

    public virtual User UsernameNavigation { get; set; } = null!;
}
