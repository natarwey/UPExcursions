using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int ExcursionId { get; set; }

    public int? GuideId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Excursion Excursion { get; set; } = null!;

    public virtual Guide? Guide { get; set; }

    public virtual User User { get; set; } = null!;
}
