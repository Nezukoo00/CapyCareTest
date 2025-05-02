using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class FeedingSchedule
{
    [Key]
    [Column("FeedingScheduleID")]
    public Guid FeedingScheduleId { get; set; }

    [Column("CapybaraID")]
    public Guid? CapybaraId { get; set; }

    [StringLength(100)]
    public string? FoodType { get; set; }

    [StringLength(50)]
    public string? PortionSize { get; set; }

    public DateTime FeedingTime { get; set; }

    [Column("ResponsibleEmployeeID")]
    public Guid? ResponsibleEmployeeId { get; set; }

    [ForeignKey("CapybaraId")]
    [InverseProperty("FeedingSchedules")]
    public virtual Capybara? Capybara { get; set; }

    [ForeignKey("ResponsibleEmployeeId")]
    [InverseProperty("FeedingSchedules")]
    public virtual Employee? ResponsibleEmployee { get; set; }
}
