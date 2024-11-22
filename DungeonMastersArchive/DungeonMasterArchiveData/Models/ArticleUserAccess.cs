using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class ArticleUserAccess
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UserId { get; set; }

    public int? AccessTypeId { get; set; }

    public virtual AccessType? AccessType { get; set; }

    public virtual User? User { get; set; }
}
