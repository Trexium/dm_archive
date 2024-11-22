using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class ArticleType
{
    public int Id { get; set; }

    public string DisplayText { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
