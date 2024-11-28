using System;
using System.Collections.Generic;
using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class PlayerEntity
    {
        [Key(0)]
        public string Name { get; private set; }
        
        [Key(1)]
        public Guid Id { get; private set; }
        
        [Key(2)]
        public Enums.PlayerRoleEnum PlayerRole { get; set; }
        
        [Key(3)]
        public Guid RoomId { get; set; }
        
        [Key(4)]
        public bool IsDealer { get; set; }
        
        [Key(5)]
        public List<CardEntity> HoleCards { get; set; }
        
        [Key(6)]
        public bool IsReady { get; set; }
        
        [Key(7)]
        public int Chips { get; set; }
        
        [Key(8)]
        public int CurrentBet { get; set; }
        
        [Key(9)]
        public bool HasTakenAction { get; set; }
        
        [Key(10)]
        public bool HasFolded { get; set; }
        
        [Key(11)]
        public Enums.CommandTypeEnum LastCommand { get; set; }
        
        [Key(12)]
        public bool IsAllIn { get; set; }
        
        public PlayerEntity(string name, Guid id, Enums.PlayerRoleEnum role)
        {
            Name = name;
            Id = id;
            PlayerRole = role;
        }
    }
}