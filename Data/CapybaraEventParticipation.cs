using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

[Table("CapybaraEventParticipation")]
public partial class CapybaraEventParticipation
{
    [Key]
    [Column("ParticipationID")]
    public Guid ParticipationId { get; set; }

    [Column("CapybaraID")]
    public Guid? CapybaraId { get; set; }

    [Column("EventID")]
    public Guid? EventId { get; set; }

    public string? Notes { get; set; }

    [ForeignKey("CapybaraId")]
    [InverseProperty("CapybaraEventParticipations")]
    public virtual Capybara? Capybara { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("CapybaraEventParticipations")]
    public virtual Event? Event { get; set; }
}
