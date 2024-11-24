using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class ArticleLink
{
    public int Id { get; set; }

    public int ParentArticleId { get; set; }

    public int ChildArticleId { get; set; }

    public virtual Article ChildArticle { get; set; } = null!;

    public virtual Article ParentArticle { get; set; } = null!;
}
