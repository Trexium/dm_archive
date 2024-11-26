namespace DungeonMastersArchive.Models
{
    public class ValueStoreItem
    {
        public ValueStoreItem() { }
        public ValueStoreItem(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; set; }
        public string Value { get; set; }
    }
}
