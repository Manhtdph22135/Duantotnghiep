using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class ProductDetail
{
    [Key]
    [Column("ProductDetailID")]
    public int ProductDetailId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column("ColorID")]
    public int? ColorId { get; set; }

    [Column("SizeID")]
    public int? SizeId { get; set; }

    [Column("MaterialID")]
    public int? MaterialId { get; set; }

    public int? StockQuantity { get; set; }

    public string? Image { get; set; }

    [ForeignKey("ColorId")]
    [InverseProperty("ProductDetails")]
    public virtual Color? Color { get; set; }

    [ForeignKey("MaterialId")]
    [InverseProperty("ProductDetails")]
    public virtual Material? Material { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductDetails")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SizeId")]
    [InverseProperty("ProductDetails")]
    public virtual Size? Size { get; set; }
}
