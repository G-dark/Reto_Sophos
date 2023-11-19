using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto_sophos2.Models;

[Table("history_user")]
public partial class HistoryUser
{
    [Key]
    [Column("username")]
    [StringLength(20)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("recent_searchs")]
    [StringLength(200)]
    [Unicode(false)]
    public string? RecentSearchs { get; set; }

    [ForeignKey("Username")]
    [InverseProperty("HistoryUser")]
    public virtual User UsernameNavigation { get; set; } = null!;
}
