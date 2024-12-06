using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class Campaign
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdateBy { get; set; }

    public string CampaignName { get; set; } = null!;

    public int? OwnerId { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ArchiveUser? CreatedByNavigation { get; set; }

    public virtual ArchiveUser? Owner { get; set; }

    public virtual ArchiveUser? UpdateByNavigation { get; set; }

    public virtual ICollection<UserCampaignRole> UserCampaignRoles { get; set; } = new List<UserCampaignRole>();
}
