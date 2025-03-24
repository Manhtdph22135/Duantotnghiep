using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Index("ColorName", Name = "UQ__Colors__C71A5A7BAD5758A9", IsUnique = true)]
public partial class Color
{
    [Key]
    [Column("ColorID")]
    public int ColorId { get; set; }

    [StringLength(50)]
    public string ColorName { get; set; } = null!;

    [InverseProperty("Color")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
