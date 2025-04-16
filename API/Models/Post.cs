using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("Post")]
public partial class Post
{
    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [StringLength(255)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Summary { get; set; }

    [Column(TypeName = "text")]
    public string? Content { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }
}
