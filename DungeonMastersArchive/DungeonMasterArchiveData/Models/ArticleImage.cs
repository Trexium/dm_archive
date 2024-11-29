using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class ArticleImage
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int? ArticleId { get; set; }

    public int? CampaignId { get; set; }

    public string? Title { get; set; }

    public string? FileName { get; set; }

    public virtual Article? Article { get; set; }

    public virtual Campaign? Campaign { get; set; }

    public virtual ArchiveUser CreatedByNavigation { get; set; } = null!;
}
