using System;
using System.Collections.Generic;

namespace DungeonMasterArchiveData.Models;

public partial class GenericValueStore
{
    public string Group { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}
