using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class Capybara
{
    [Key]
    [Column("CapybaraID")]
    public Guid CapybaraId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(10)]
    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? ArrivalDate { get; set; }

    [Column("EnclosureID")]
    public Guid? EnclosureId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public bool? IsAdopted { get; set; }

    [Column("AdopterID")]
    public Guid? AdopterId { get; set; }

    [ForeignKey("AdopterId")]
    [InverseProperty("Capybaras")]
    public virtual Adopter? Adopter { get; set; }

    [InverseProperty("Capybara")]
    public virtual ICollection<CapybaraEventParticipation> CapybaraEventParticipations { get; set; } = new List<CapybaraEventParticipation>();

    [ForeignKey("EnclosureId")]
    [InverseProperty("Capybaras")]
    public virtual Enclosure? Enclosure { get; set; }

    [InverseProperty("Capybara")]
    public virtual ICollection<FeedingSchedule> FeedingSchedules { get; set; } = new List<FeedingSchedule>();

    [InverseProperty("Capybara")]
    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

    [InverseProperty("Capybara")]
    public virtual ICollection<Visitor> Visitors { get; set; } = new List<Visitor>();
}
