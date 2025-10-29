using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class ExcursionSession
{
    public int SessionId { get; set; }

    public int ExcursionId { get; set; }

    public int? GuideId { get; set; }

    public DateOnly SessionDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public int MaxParticipants { get; set; }

    public int AvailableSpots { get; set; }

    public virtual Excursion Excursion { get; set; } = null!;

    public virtual Guide? Guide { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
