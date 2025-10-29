using System;
using System.Collections.Generic;

namespace UPExcursions.Data;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Excursion> Excursions { get; set; } = new List<Excursion>();
}
