using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class Enclosure
{
    [Key]
    [Column("EnclosureID")]
    public Guid EnclosureId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Size { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    public int? Capacity { get; set; }

    [InverseProperty("Enclosure")]
    public virtual ICollection<Capybara> Capybaras { get; set; } = new List<Capybara>();
}
