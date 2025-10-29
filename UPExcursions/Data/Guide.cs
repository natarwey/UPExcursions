using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class Guide
{
    public int GuideId { get; set; }

    public int UserId { get; set; }

    public string? Bio { get; set; }

    public decimal? RatingAvg { get; set; }

    public virtual ICollection<ExcursionSession> ExcursionSessions { get; set; } = new List<ExcursionSession>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
