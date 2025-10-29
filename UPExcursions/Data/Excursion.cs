using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class Excursion
{
    public int ExcursionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal DurationHours { get; set; }

    public decimal BasePrice { get; set; }

    public int CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ExcursionSession> ExcursionSessions { get; set; } = new List<ExcursionSession>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
