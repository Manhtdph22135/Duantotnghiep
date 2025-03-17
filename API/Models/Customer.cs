using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }

    [StringLength(100)]
    public string? FullName { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    public int? Phone { get; set; }

    public int? Gender { get; set; }

    [Column("DOB")]
    public DateOnly? Dob { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    public DateOnly? CreateAt { get; set; }

    public DateOnly? UdateAt { get; set; }

    [StringLength(50)]
    public string? RankMember { get; set; }

    public int? Point { get; set; }
}
