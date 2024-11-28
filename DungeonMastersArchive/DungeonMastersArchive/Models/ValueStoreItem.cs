using Microsoft.IdentityModel.Tokens;

namespace DungeonMastersArchive.Models
{
    public class ValueStoreItem<KeyType, ValueType>
    {
        public ValueStoreItem() { }
        public ValueStoreItem(KeyType key, ValueType value)
        {
            Key = key;
            Value = value;
        }

        public KeyType Key { get; set; }
        public ValueType Value { get; set; }
    }
}
