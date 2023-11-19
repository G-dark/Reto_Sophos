using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto_sophos2.Models;

[Table("fight")]
public partial class Fight
{
    [Key]
    [Column("fight_ID")]
    public int FightId { get; set; }

    [Column("result")]
    public int? Result { get; set; }

    [Column("hero_ID")]
    public int? HeroId { get; set; }

    [Column("villain_ID")]
    public int? VillainId { get; set; }

    [Column("comments")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Comments { get; set; }

    [ForeignKey("HeroId")]
    [InverseProperty("Fights")]
    public virtual Hero? Hero { get; set; }

    [ForeignKey("VillainId")]
    [InverseProperty("Fights")]
    public virtual Villain? Villain { get; set; }
}
