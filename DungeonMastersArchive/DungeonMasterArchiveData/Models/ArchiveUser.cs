using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class ArchiveUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? AspNetUserId { get; set; }

    public int? CurrentCampaignId { get; set; }

    public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual ICollection<Article> ArticleUpdateByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<ArticleUserAccess> ArticleUserAccesses { get; set; } = new List<ArticleUserAccess>();

    public virtual AspNetUser? AspNetUser { get; set; }

    public virtual ICollection<Campaign> CampaignCreatedByNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignUpdateByNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<UserCampaignRole> UserCampaignRoles { get; set; } = new List<UserCampaignRole>();
}
