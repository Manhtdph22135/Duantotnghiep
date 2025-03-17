using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class Staff
{
    [Column("StaffID")]
    public int StaffId { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }

    [StringLength(100)]
    public string? FullName { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    public int? Phone { get; set; }

    public int? Gender { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Salary { get; set; }

    public int? Position { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateOnly? CreateAt { get; set; }

    public DateOnly? UpdateAt { get; set; }
}
