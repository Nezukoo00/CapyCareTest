using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.Data;

public partial class CapyCareContext : DbContext
{
    public CapyCareContext()
    {
    }

    public CapyCareContext(DbContextOptions<CapyCareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adopter> Adopters { get; set; }

    public virtual DbSet<Capybara> Capybaras { get; set; }

    public virtual DbSet<CapybaraEventParticipation> CapybaraEventParticipations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Enclosure> Enclosures { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<FeedingSchedule> FeedingSchedules { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-CMTQU0P6\\MSSQLSERVER2;Database=CapyCare;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adopter>(entity =>
        {
            entity.HasKey(e => e.AdopterId).HasName("PK__Adopters__499FD2ED2E4EA95C");

            entity.Property(e => e.AdopterId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Approved).HasDefaultValue(false);
        });

        modelBuilder.Entity<Capybara>(entity =>
        {
            entity.HasKey(e => e.CapybaraId).HasName("PK__Capybara__A7CF9D528CE8D752");

            entity.Property(e => e.CapybaraId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsAdopted).HasDefaultValue(false);

            entity.HasOne(d => d.Adopter).WithMany(p => p.Capybaras).HasConstraintName("FK__Capybaras__Adopt__412EB0B6");

            entity.HasOne(d => d.Enclosure).WithMany(p => p.Capybaras).HasConstraintName("FK__Capybaras__Enclo__403A8C7D");
        });

        modelBuilder.Entity<CapybaraEventParticipation>(entity =>
        {
            entity.HasKey(e => e.ParticipationId).HasName("PK__Capybara__4EA27080012C1470");

            entity.Property(e => e.ParticipationId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Capybara).WithMany(p => p.CapybaraEventParticipations).HasConstraintName("FK__CapybaraE__Capyb__5441852A");

            entity.HasOne(d => d.Event).WithMany(p => p.CapybaraEventParticipations).HasConstraintName("FK__CapybaraE__Event__5535A963");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1C8658035");

            entity.Property(e => e.EmployeeId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Enclosure>(entity =>
        {
            entity.HasKey(e => e.EnclosureId).HasName("PK__Enclosur__4A63C52CC3E6EC1F");

            entity.Property(e => e.EnclosureId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C870AD3666C7");

            entity.Property(e => e.EventId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<FeedingSchedule>(entity =>
        {
            entity.HasKey(e => e.FeedingScheduleId).HasName("PK__FeedingS__2BB09A3A83ED6A8F");

            entity.Property(e => e.FeedingScheduleId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Capybara).WithMany(p => p.FeedingSchedules).HasConstraintName("FK__FeedingSc__Capyb__4CA06362");

            entity.HasOne(d => d.ResponsibleEmployee).WithMany(p => p.FeedingSchedules).HasConstraintName("FK__FeedingSc__Respo__4D94879B");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.HealthRecordId).HasName("PK__HealthRe__3BE0B89D7756C4CD");

            entity.Property(e => e.HealthRecordId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Capybara).WithMany(p => p.HealthRecords).HasConstraintName("FK__HealthRec__Capyb__47DBAE45");

            entity.HasOne(d => d.Vet).WithMany(p => p.HealthRecords).HasConstraintName("FK__HealthRec__VetID__48CFD27E");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.VisitorId).HasName("PK__Visitors__B121AFA89541D3BE");

            entity.Property(e => e.VisitorId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Capybara).WithMany(p => p.Visitors).HasConstraintName("FK__Visitors__Capyba__59063A47");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
