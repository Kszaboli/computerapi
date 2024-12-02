using ComputerAPInet.Models;
using System;
using System.Collections.Generic;

namespace ComputerApiHetfo.Models;

public partial class Comp
{
    public Guid Id { get; set; }

    public string? Brand { get; set; }

    public string? Type { get; set; }

    public double? Display { get; set; }

    public int? Memory { get; set; }

    public DateTime? CreatedTime { get; set; }

    public Guid? OsId { get; set; }

    public virtual Osystem? Os { get; set; }
}