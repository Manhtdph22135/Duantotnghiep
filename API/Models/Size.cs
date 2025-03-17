using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Index("SizeName", Name = "UQ__Sizes__619EFC3E38E96F8C", IsUnique = true)]
public partial class Size
{
    [Key]
    [Column("SizeID")]
    public int SizeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SizeName { get; set; }

    [InverseProperty("Size")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
