using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class Article
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdateBy { get; set; }

    public bool? Deleted { get; set; }

    public bool? Published { get; set; }

    public string ArticleName { get; set; } = null!;

    public int? CampaignId { get; set; }

    public int? ArticleType { get; set; }

    public string? ArticleText { get; set; }

    public int? ArticleYear { get; set; }

    public int? ArticleMonth { get; set; }

    public int? ArticleDay { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual ICollection<ArticleLink> ArticleLinkChildArticles { get; set; } = new List<ArticleLink>();

    public virtual ICollection<ArticleLink> ArticleLinkParentArticles { get; set; } = new List<ArticleLink>();

    public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();

    public virtual ArticleType? ArticleTypeNavigation { get; set; }

    public virtual Campaign? Campaign { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdateByNavigation { get; set; }
}
