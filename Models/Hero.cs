using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto_sophos2.Models;

[Table("hero")]
[Index("HeroName", Name = "hero_name")]
[Index("Powers", Name = "powers_IDX")]
public partial class Hero
{
    [Key]
    [Column("hero_ID")]
    public int HeroId { get; set; }

    [Column("img", TypeName = "image")]
    public byte[]? Img { get; set; }

    [Column("hero_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? HeroName { get; set; }

    [Column("real_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? RealName { get; set; }

    [Column("powers")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Powers { get; set; }

    [Column("weaks")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Weaks { get; set; }

    [Column("relations")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Relations { get; set; }

    [Column("age")]
    public int? Age { get; set; }

    [Column("cellphone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Cellphone { get; set; }

    [Column("origin")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Origin { get; set; }

    [InverseProperty("Hero")]
    public virtual ICollection<Fight> Fights { get; set; } = new List<Fight>();

    [InverseProperty("Hero")]
    public virtual ICollection<Sponsor> Sponsors { get; set; } = new List<Sponsor>();

    [InverseProperty("Hero")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
