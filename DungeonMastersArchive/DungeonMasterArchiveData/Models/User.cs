using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticleUpdateByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<ArticleUserAccess> ArticleUserAccesses { get; set; } = new List<ArticleUserAccess>();

    public virtual ICollection<Campaign> CampaignCreatedByNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignUpdateByNavigations { get; set; } = new List<Campaign>();
}
