using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class Adopter
{
    [Key]
    [Column("AdopterID")]
    public Guid AdopterId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    public DateOnly? ApplicationDate { get; set; }

    public bool? Approved { get; set; }

    public string? Notes { get; set; }

    [InverseProperty("Adopter")]
    public virtual ICollection<Capybara> Capybaras { get; set; } = new List<Capybara>();
}
