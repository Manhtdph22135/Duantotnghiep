using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class Bill
{
    [Key]
    [Column("BillID")]
    public int BillId { get; set; }

    [Column("OrderID")]
    public int? OrderId { get; set; }

    [Column("StaffID")]
    public int? StaffId { get; set; }

    [Column("CustomerID")]
    public int? CustomerId { get; set; }

    [Column("TransportID")]
    public int? TransportId { get; set; }

    public DateOnly? CreateAt { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalAmount { get; set; }

    [InverseProperty("Bill")]
    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    [ForeignKey("CustomerId")]
    [InverseProperty("Bills")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Bills")]
    public virtual Order? Order { get; set; }

    [ForeignKey("StaffId")]
    [InverseProperty("Bills")]
    public virtual Staff? Staff { get; set; }

    [ForeignKey("TransportId")]
    [InverseProperty("Bills")]
    public virtual Transport? Transport { get; set; }
}
