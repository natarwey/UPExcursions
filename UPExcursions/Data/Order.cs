using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int SessionId { get; set; }

    public int ParticipantsCount { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ExcursionSession Session { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
