using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace CapyCareTest.Data;

public partial class HealthRecord
{
    [Key]
    [Column("HealthRecordID")]
    public Guid HealthRecordId { get; set; }

    [Column("CapybaraID")]
    public Guid? CapybaraId { get; set; }

    public DateOnly? CheckDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    [Column("VetID")]
    public Guid? VetId { get; set; }

    [ForeignKey("CapybaraId")]
    [InverseProperty("HealthRecords")]
    public virtual Capybara? Capybara { get; set; }

    [ForeignKey("VetId")]
    [InverseProperty("HealthRecords")]
    public virtual Employee? Vet { get; set; }
}
