using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class ArticleTag
{
    public int Id { get; set; }

    public int? ArticleId { get; set; }

    public string Tag { get; set; } = null!;

    public virtual Article? Article { get; set; }
}
