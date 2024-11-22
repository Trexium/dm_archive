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

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdateByNavigation { get; set; }
}
