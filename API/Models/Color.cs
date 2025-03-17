using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Index("ColorName", Name = "UQ__Colors__C71A5A7B3A1CC250", IsUnique = true)]
public partial class Color
{
    [Key]
    [Column("ColorID")]
    public int ColorId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ColorName { get; set; }

    [InverseProperty("Color")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
