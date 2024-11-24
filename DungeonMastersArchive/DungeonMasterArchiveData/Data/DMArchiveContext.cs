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

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleImage> ArticleImages { get; set; }

    public virtual DbSet<ArticleLink> ArticleLinks { get; set; }

    public virtual DbSet<ArticleTag> ArticleTags { get; set; }

    public virtual DbSet<ArticleType> ArticleTypes { get; set; }

    public virtual DbSet<ArticleUserAccess> ArticleUserAccesses { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<GenericValueStore> GenericValueStores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccessTy__3214EC0721B3140A");

            entity.ToTable("AccessType");

            entity.Property(e => e.DisplayText).HasMaxLength(255);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC073C8FCD83");

            entity.ToTable("Article");

            entity.Property(e => e.ArticleName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ArticleType).WithMany(p => p.Articles)
                .HasForeignKey(d => d.ArticleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Article__7755B73D");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Campaig__76619304");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ArticleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Created__72910220");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.ArticleUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Article__UpdateB__73852659");
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleI__3214EC077A33054D");

            entity.ToTable("ArticleImage");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(512);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleIm__Artic__01D345B0");

            entity.HasOne(d => d.Campaign).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleIm__Campa__02C769E9");
        });

        modelBuilder.Entity<ArticleLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleL__3214EC0755B4BAC1");

            entity.ToTable("ArticleLink");

            entity.HasOne(d => d.ChildArticle).WithMany(p => p.ArticleLinkChildArticles)
                .HasForeignKey(d => d.ChildArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleLi__Child__7B264821");

            entity.HasOne(d => d.ParentArticle).WithMany(p => p.ArticleLinkParentArticles)
                .HasForeignKey(d => d.ParentArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleLi__Paren__7A3223E8");
        });

        modelBuilder.Entity<ArticleTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleT__3214EC075B118B65");

            entity.ToTable("ArticleTag");

            entity.Property(e => e.Tag).HasMaxLength(255);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleTa__Artic__7E02B4CC");
        });

        modelBuilder.Entity<ArticleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleT__3214EC07BAE19C77");

            entity.ToTable("ArticleType");

            entity.Property(e => e.DisplayText).HasMaxLength(255);
        });

        modelBuilder.Entity<ArticleUserAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleU__3214EC07BBBA8ACB");

            entity.ToTable("ArticleUserAccess");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccessType).WithMany(p => p.ArticleUserAccesses)
                .HasForeignKey(d => d.AccessTypeId)
                .HasConstraintName("FK__ArticleUs__Acces__69FBBC1F");

            entity.HasOne(d => d.User).WithMany(p => p.ArticleUserAccesses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ArticleUs__UserI__690797E6");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campaign__3214EC0714EC72FC");

            entity.ToTable("Campaign");

            entity.Property(e => e.CampaignName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CampaignCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Campaign__Create__6DCC4D03");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.CampaignUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Campaign__Update__6EC0713C");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07E6E69F22");

            entity.ToTable("User");

            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__65370702");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
