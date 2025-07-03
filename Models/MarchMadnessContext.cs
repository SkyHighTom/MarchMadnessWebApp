using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MarchMadnessWebApp.Models;

public partial class MarchMadnessContext : DbContext
{
    public MarchMadnessContext()
    {
    }

    public MarchMadnessContext(DbContextOptions<MarchMadnessContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FinalPrediction> FinalPredictions { get; set; }

    public virtual DbSet<LargestUpset> LargestUpsets { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Prediction> Predictions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Streak> Streaks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-6G94KNL\\SQLEXPRESS;Database=MarchMadness;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinalPrediction>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__FinalPre__536C85E5E57B3901");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Champion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("champion");
            entity.Property(e => e.Team1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("team1");
            entity.Property(e => e.Team2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("team2");
            entity.Property(e => e.Team3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("team3");
            entity.Property(e => e.Team4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("team4");

            entity.HasOne(d => d.ChampionNavigation).WithMany(p => p.FinalPredictionChampionNavigations)
                .HasForeignKey(d => d.Champion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FinalPred__champ__73BA3083");

            entity.HasOne(d => d.Team1Navigation).WithMany(p => p.FinalPredictionTeam1Navigations)
                .HasForeignKey(d => d.Team1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FinalPred__team1__6FE99F9F");

            entity.HasOne(d => d.Team2Navigation).WithMany(p => p.FinalPredictionTeam2Navigations)
                .HasForeignKey(d => d.Team2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FinalPred__team2__70DDC3D8");

            entity.HasOne(d => d.Team3Navigation).WithMany(p => p.FinalPredictionTeam3Navigations)
                .HasForeignKey(d => d.Team3)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FinalPred__team3__71D1E811");

            entity.HasOne(d => d.Team4Navigation).WithMany(p => p.FinalPredictionTeam4Navigations)
                .HasForeignKey(d => d.Team4)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FinalPred__team4__72C60C4A");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.FinalPrediction)
                .HasForeignKey<FinalPrediction>(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FinalPred__Usern__74AE54BC");
        });

        modelBuilder.Entity<LargestUpset>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__LargestU__536C85E51E26CF08");

            entity.ToTable("LargestUpset");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.LargestUpset)
                .HasForeignKey<LargestUpset>(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LargestUp__Usern__778AC167");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PK__Matches__4218C837D5B1F3A4");

            entity.HasIndex(e => new { e.Team1, e.Team2, e.Round }, "UC_TeamCombination").IsUnique();

            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Team1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Team2)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Team1Navigation).WithMany(p => p.MatchTeam1Navigations)
                .HasForeignKey(d => d.Team1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Matches__Team1__5441852A");

            entity.HasOne(d => d.Team2Navigation).WithMany(p => p.MatchTeam2Navigations)
                .HasForeignKey(d => d.Team2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Matches__Team2__5535A963");
        });

        modelBuilder.Entity<Prediction>(entity =>
        {
            entity.HasKey(e => new { e.Username, e.MatchId }).HasName("PK__Predicti__374D0966D8B8E92B");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Winner)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Match).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Predictio__Match__6754599E");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Predictio__Usern__656C112C");

            entity.HasOne(d => d.WinnerNavigation).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.Winner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Predictio__Winne__66603565");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PK__Results__4218C837A765C3CC");

            entity.Property(e => e.MatchId)
                .ValueGeneratedNever()
                .HasColumnName("MatchID");
            entity.Property(e => e.Winner)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Match).WithOne(p => p.Result)
                .HasForeignKey<Result>(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Results__MatchID__5EBF139D");

            entity.HasOne(d => d.WinnerNavigation).WithMany(p => p.Results)
                .HasForeignKey(d => d.Winner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Results__Winner__5FB337D6");
        });

        modelBuilder.Entity<Streak>(entity =>
        {
            entity.HasKey(e => new { e.Username, e.TeamName }).HasName("PK__Streaks__978E994F9FB006F0");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Streak1).HasColumnName("Streak");

            entity.HasOne(d => d.TeamNameNavigation).WithMany(p => p.Streaks)
                .HasForeignKey(d => d.TeamName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Streaks__TeamNam__6B24EA82");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Streaks)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Streaks__Usernam__6A30C649");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamName).HasName("PK__Teams__4E21CAADE8062BC8");

            entity.Property(e => e.TeamName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__Users__536C85E5A69F5E1B");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
