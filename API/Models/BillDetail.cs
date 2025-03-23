using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class BillDetail
{
    [Key]
    [Column("BillDetailID")]
    public int BillDetailId { get; set; }

    [Column("BillID")]
    public int? BillId { get; set; }

    [Column("ProductDetailID")]
    public int? ProductDetailId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(29, 2)")]
    public decimal? Total { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("BillDetails")]
    public virtual Bill? Bill { get; set; }

    [ForeignKey("ProductDetailId")]
    [InverseProperty("BillDetails")]
    public virtual ProductDetail? ProductDetail { get; set; }
}
