using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class UserCampaignRole
{
    public int UserId { get; set; }

    public int CampaignId { get; set; }

    public int RoleId { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual ArchiveUser User { get; set; } = null!;
}
