using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class Visitor
{
    [Key]
    [Column("VisitorID")]
    public Guid VisitorId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public DateOnly? VisitDate { get; set; }

    [Column("CapybaraID")]
    public Guid? CapybaraId { get; set; }

    [ForeignKey("CapybaraId")]
    [InverseProperty("Visitors")]
    public virtual Capybara? Capybara { get; set; }
}
