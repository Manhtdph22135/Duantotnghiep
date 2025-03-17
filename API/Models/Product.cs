using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class Product
{
    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? ProductName { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdateAt { get; set; }

    public int? Statuss { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual ProductCategory? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
