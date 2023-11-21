using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto_sophos2.Models;

[Table("task")]
public partial class Task
{
    [Key]
    [Column("task_ID")]
    public int TaskId { get; set; }

    [Column("task_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TaskName { get; set; }

    [Column("hero_ID")]
    public int HeroId { get; set; }

    [Column("sdate", TypeName = "datetime")]
    public DateTime? Sdate { get; set; }

    [Column("fdate", TypeName = "datetime")]
    public DateTime? Fdate { get; set; }

    [Column("task_status")]
    public int? TaskStatus { get; set; }

    [Column("edited_by")]
    [StringLength(20)]
    [Unicode(false)]
    public string? EditedBy { get; set; }

    [Column("edited_at", TypeName = "date")]
    public DateTime? EditedAt { get; set; }

    [ForeignKey("EditedBy")]
    [InverseProperty("Tasks")]
    public virtual User? EditedByNavigation { get; set; }

    [ForeignKey("HeroId")]
    [InverseProperty("Tasks")]
    public virtual Hero? Hero { get; set; } = null!;
}
