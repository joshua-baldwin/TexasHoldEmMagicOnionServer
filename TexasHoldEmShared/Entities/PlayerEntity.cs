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
        public int OrderInQueue { get; set; }
        
        [Key(4)]
        public Guid RoomId { get; set; }
        
        [Key(5)]
        public bool IsDealer { get; set; }
        
        [Key(6)]
        public List<CardEntity> HoleCards { get; set; }
        
        [Key(7)]
        public bool IsReady { get; set; }
        
        [Key(8)]
        public int Chips { get; set; }
        
        [Key(9)]
        public int CurrentBet { get; set; }
        
        [Key(10)]
        public int CurrentBetIndex { get; set; }
        
        [Key(11)]
        public bool HasTakenAction { get; set; }
        
        [Key(12)]
        public bool HasFolded { get; set; }
        
        [Key(13)]
        public Enums.CommandTypeEnum LastCommand { get; set; }
        
        [Key(14)]
        public bool IsAllIn { get; set; }
        
        [Key(15)]
        public BestHandEntity? BestHand { get; set; }
        
        [Key(16)]
        public int RaiseAmount { get; set; }
        
        //currently held cards; use to show if player is bluffing with a raise or all in
        [Key(17)]
        public BestHandEntity CurrentBestHand { get; set; }
        
        [Key(18)]
        public int AllInAmount { get; set; }
        
        public PlayerEntity(string name, Guid id, Enums.PlayerRoleEnum role)
        {
            Name = name;
            Id = id;
            PlayerRole = role;
        }

        public void InitializeForNextRound()
        {
            HoleCards.Clear();
            CurrentBet = 0;
            CurrentBetIndex = 0;
            HasTakenAction = false;
            HasFolded = false;
            LastCommand = 0;
            IsAllIn = false;
            BestHand = null;
            RaiseAmount = 0;
            CurrentBestHand = null;
            AllInAmount = 0;
        }
    }
}