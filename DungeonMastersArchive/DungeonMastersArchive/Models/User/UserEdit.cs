﻿namespace DungeonMastersArchive.Models.User
{
    public class UserEdit
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public int? RoleId { get; set; }
        public string? AspNetUserId { get; set; }
        
    }
}