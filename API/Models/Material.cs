using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Index("MaterialName", Name = "UQ__Material__9C87053CEF6D0D8B", IsUnique = true)]
public partial class Material
{
    [Key]
    [Column("MaterialID")]
    public int MaterialId { get; set; }

    [StringLength(100)]
    public string MaterialName { get; set; } = null!;

    [InverseProperty("Material")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
