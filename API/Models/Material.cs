using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Index("MaterialName", Name = "UQ__Material__9C87053C237930DD", IsUnique = true)]
public partial class Material
{
    [Key]
    [Column("MaterialID")]
    public int MaterialId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? MaterialName { get; set; }

    [InverseProperty("Material")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
