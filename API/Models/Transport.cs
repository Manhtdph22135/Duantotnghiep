using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("Transport")]
public partial class Transport
{
    [Key]
    [Column("TransportID")]
    public int TransportId { get; set; }

    [StringLength(50)]
    public string TransportMethod { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost { get; set; }

    [InverseProperty("Transport")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
