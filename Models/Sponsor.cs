using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto_sophos2.Models;

[Table("sponsor")]
public partial class Sponsor
{
    [Key]
    [Column("trans_ID")]
    public int TransId { get; set; }

    [Column("sponsor_ID")]
    public int? SponsorId { get; set; }

    [Column("amount", TypeName = "money")]
    public decimal? Amount { get; set; }

    [Column("hero_ID")]
    public int? HeroId { get; set; }

    [Column("sourceOM")]
    [StringLength(100)]
    [Unicode(false)]
    public string? SourceOm { get; set; }

    [ForeignKey("HeroId")]
    [InverseProperty("Sponsors")]
    public virtual Hero? Hero { get; set; }
}
