using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class Article
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdateBy { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsPublished { get; set; }

    public string ArticleName { get; set; } = null!;

    public int CampaignId { get; set; }

    public int ArticleTypeId { get; set; }

    public string? ArticleText { get; set; }

    public int? ArticleYear { get; set; }

    public int? ArticleMonth { get; set; }

    public int? ArticleDay { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual ICollection<ArticleLink> ArticleLinkChildArticles { get; set; } = new List<ArticleLink>();

    public virtual ICollection<ArticleLink> ArticleLinkParentArticles { get; set; } = new List<ArticleLink>();

    public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();

    public virtual ArticleType ArticleType { get; set; } = null!;

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? UpdateByNavigation { get; set; }
}
