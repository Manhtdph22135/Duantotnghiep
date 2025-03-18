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

    [Column("ProductID")]
    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Total { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("BillDetails")]
    public virtual Bill? Bill { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("BillDetails")]
    public virtual Product? Product { get; set; }
}
