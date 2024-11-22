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

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
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
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC07F77FB5E6");

            entity.ToTable("Article");

            entity.Property(e => e.ArticleName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasDefaultValue(true);
            entity.Property(e => e.Published).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ArticleTypeNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.ArticleType)
                .HasConstraintName("FK__Article__Article__73BA3083");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__Article__Campaig__72C60C4A");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ArticleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Article__Created__6EF57B66");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.ArticleUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Article__UpdateB__6FE99F9F");
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleI__3214EC07B6D0FBD8");

            entity.ToTable("ArticleImage");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(512);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__ArticleIm__Artic__7E37BEF6");

            entity.HasOne(d => d.Campaign).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("FK__ArticleIm__Campa__7F2BE32F");
        });

        modelBuilder.Entity<ArticleLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleL__3214EC07EF36E36E");

            entity.ToTable("ArticleLink");

            entity.HasOne(d => d.ChildArticle).WithMany(p => p.ArticleLinkChildArticles)
                .HasForeignKey(d => d.ChildArticleId)
                .HasConstraintName("FK__ArticleLi__Child__778AC167");

            entity.HasOne(d => d.ParentArticle).WithMany(p => p.ArticleLinkParentArticles)
                .HasForeignKey(d => d.ParentArticleId)
                .HasConstraintName("FK__ArticleLi__Paren__76969D2E");
        });

        modelBuilder.Entity<ArticleTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleT__3214EC07065EF64B");

            entity.ToTable("ArticleTag");

            entity.Property(e => e.Tag).HasMaxLength(255);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__ArticleTa__Artic__7A672E12");
        });

        modelBuilder.Entity<ArticleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleT__3214EC07BAE19C77");

            entity.ToTable("ArticleType");

            entity.Property(e => e.DisplayText).HasMaxLength(255);
        });

        modelBuilder.Entity<ArticleUserAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArticleU__3214EC07464A8EA5");

            entity.ToTable("ArticleUserAccess");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccessType).WithMany(p => p.ArticleUserAccesses)
                .HasForeignKey(d => d.AccessTypeId)
                .HasConstraintName("FK__ArticleUs__Acces__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.ArticleUserAccesses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ArticleUs__UserI__5629CD9C");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campaign__3214EC0790C9BDE4");

            entity.ToTable("Campaign");

            entity.Property(e => e.CampaignName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CampaignCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Campaign__Create__4D94879B");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.CampaignUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Campaign__Update__4E88ABD4");
        });

        modelBuilder.Entity<GenericValueStore>(entity =>
        {
            entity.HasKey(e => new { e.Group, e.Key }).HasName("PK_Group_Key");

            entity.ToTable("GenericValueStore");

            entity.Property(e => e.Group).HasMaxLength(255);
            entity.Property(e => e.Key).HasMaxLength(25);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC079BC11D00");

            entity.ToTable("User");

            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
