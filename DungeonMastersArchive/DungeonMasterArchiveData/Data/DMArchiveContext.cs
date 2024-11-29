using System;
using System.Collections.Generic;
using DungeonMasterArchiveData.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterArchiveData.Data;

public partial class DMArchiveContext : DbContext
{
    public DMArchiveContext()
    {
    }

    public DMArchiveContext(DbContextOptions<DMArchiveContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessType> AccessTypes { get; set; }

    public virtual DbSet<ArchiveUser> ArchiveUsers { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleImage> ArticleImages { get; set; }

    public virtual DbSet<ArticleLink> ArticleLinks { get; set; }

    public virtual DbSet<ArticleTag> ArticleTags { get; set; }

    public virtual DbSet<ArticleType> ArticleTypes { get; set; }

    public virtual DbSet<ArticleUserAccess> ArticleUserAccesses { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<GenericValueStore> GenericValueStores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserCampaignRole> UserCampaignRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccessTy__3214EC0721B3140A");

            entity.ToTable("AccessType");

            entity.Property(e => e.DisplayText).HasMaxLength(255);
        });

        modelBuilder.Entity<ArchiveUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArchiveU__3214EC07E37B414B");

            entity.ToTable("ArchiveUser");

            entity.Property(e => e.AspNetUserId).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.ArchiveUsers)
                .HasForeignKey(d => d.AspNetUserId)
                .HasConstraintName("FK__ArchiveUs__AspNe__1E6F845E");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC07B9ACCB41");

            entity.ToTable("Article");

            entity.Property(e => e.ArticleName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ArticleType).WithMany(p => p.Articles)
                .HasForeignKey(d => d.ArticleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Article__6F7F8B4B");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Campaig__6E8B6712");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ArticleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Created__6ABAD62E");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.ArticleUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Article__UpdateB__6BAEFA67");
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleI__3214EC07DE330E50");

            entity.ToTable("ArticleImage");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(512);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__ArticleIm__Artic__7FB5F314");

            entity.HasOne(d => d.Campaign).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__ArticleIm__Campa__00AA174D");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleIm__Creat__7EC1CEDB");
        });

        modelBuilder.Entity<ArticleLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleL__3214EC07B9FA5161");

            entity.ToTable("ArticleLink");

            entity.HasOne(d => d.ChildArticle).WithMany(p => p.ArticleLinkChildArticles)
                .HasForeignKey(d => d.ChildArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleLi__Child__73501C2F");

            entity.HasOne(d => d.ParentArticle).WithMany(p => p.ArticleLinkParentArticles)
                .HasForeignKey(d => d.ParentArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleLi__Paren__725BF7F6");
        });

        modelBuilder.Entity<ArticleTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleT__3214EC07BC90BACD");

            entity.ToTable("ArticleTag");

            entity.Property(e => e.Tag).HasMaxLength(255);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleTa__Artic__762C88DA");
        });

        modelBuilder.Entity<ArticleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleT__3214EC07B13646EF");

            entity.ToTable("ArticleType");

            entity.Property(e => e.DisplayText).HasMaxLength(255);
        });

        modelBuilder.Entity<ArticleUserAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleU__3214EC07BE143581");

            entity.ToTable("ArticleUserAccess");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccessType).WithMany(p => p.ArticleUserAccesses)
                .HasForeignKey(d => d.AccessTypeId)
                .HasConstraintName("FK__ArticleUs__Acces__382F5661");

            entity.HasOne(d => d.User).WithMany(p => p.ArticleUserAccesses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ArticleUs__UserI__373B3228");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campaign__3214EC0708C536C9");

            entity.ToTable("Campaign");

            entity.Property(e => e.CampaignName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CampaignCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Campaign__Create__29E1370A");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.CampaignUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Campaign__Update__2AD55B43");
        });

        modelBuilder.Entity<GenericValueStore>(entity =>
        {
            entity.HasKey(e => new { e.Group, e.Key }).HasName("PK_Group_Key");

            entity.ToTable("GenericValueStore");

            entity.Property(e => e.Group).HasMaxLength(255);
            entity.Property(e => e.Key).HasMaxLength(25);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC070623C5F5");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<UserCampaignRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CampaignId, e.RoleId }).HasName("PK__UserCamp__8FF7DE2B9867006F");

            entity.ToTable("UserCampaignRole");

            entity.HasOne(d => d.Campaign).WithMany(p => p.UserCampaignRoles)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCampa__Campa__3BFFE745");

            entity.HasOne(d => d.Role).WithMany(p => p.UserCampaignRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCampa__RoleI__3CF40B7E");

            entity.HasOne(d => d.User).WithMany(p => p.UserCampaignRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCampa__UserI__3B0BC30C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
