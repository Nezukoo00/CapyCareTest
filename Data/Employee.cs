using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class Employee
{
    [Key]
    [Column("EmployeeID")]
    public Guid EmployeeId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Position { get; set; }

    public DateOnly? HireDate { get; set; }

    [StringLength(100)]
    public string? Specialization { get; set; }

    [InverseProperty("ResponsibleEmployee")]
    public virtual ICollection<FeedingSchedule> FeedingSchedules { get; set; } = new List<FeedingSchedule>();

    [InverseProperty("Vet")]
    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
}
