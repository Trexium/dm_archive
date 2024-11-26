using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserCampaignRole> UserCampaignRoles { get; set; } = new List<UserCampaignRole>();
}
