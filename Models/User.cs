using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto_sophos2.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("username")]
    [StringLength(20)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("pw")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Pw { get; set; }

    [Column("img", TypeName = "image")]
    public byte[]? Img { get; set; }

    [InverseProperty("UsernameNavigation")]
    public virtual HistoryUser? HistoryUser { get; set; }

    [InverseProperty("EditedByNavigation")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
