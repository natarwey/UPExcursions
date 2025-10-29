using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class Favorite
{
    public int UserId { get; set; }

    public int ExcursionId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Excursion Excursion { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
