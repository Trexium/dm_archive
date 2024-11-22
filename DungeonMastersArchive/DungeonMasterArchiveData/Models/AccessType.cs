using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class AccessType
{
    public int Id { get; set; }

    public string? DisplayText { get; set; }

    public virtual ICollection<ArticleUserAccess> ArticleUserAccesses { get; set; } = new List<ArticleUserAccess>();
}
