namespace DungeonMastersArchive.Models.User
{
    public class EditUser
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public Role Role { get; set; }
        public DungeonMasterArchiveData.Models.AspNetUser AspNetUser { get; set; }
    }
}
