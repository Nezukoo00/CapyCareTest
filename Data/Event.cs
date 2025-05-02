using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class Event
{
    [Key]
    [Column("EventID")]
    public Guid EventId { get; set; }

    [StringLength(100)]
    public string EventName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? Date { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<CapybaraEventParticipation> CapybaraEventParticipations { get; set; } = new List<CapybaraEventParticipation>();
}
