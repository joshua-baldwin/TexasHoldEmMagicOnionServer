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
        public int OriginalOrderInQueue { get; set; }
        
        [Key(5)]
        public Guid RoomId { get; set; }
        
        [Key(6)]
        public bool IsDealer { get; set; }
        
        [Key(7)]
        public List<CardEntity> HoleCards { get; set; } = new List<CardEntity>();
        
        [Key(8)]
        public bool IsReady { get; set; }
        
        [Key(9)]
        public int Chips { get; set; }
        
        [Key(10)]
        public int CurrentBet { get; set; }
        
        [Key(11)]
        public int CurrentBetIndex { get; set; }
        
        [Key(12)]
        public bool HasTakenAction { get; set; }
        
        [Key(13)]
        public bool HasFolded { get; set; }
        
        [Key(14)]
        public Enums.CommandTypeEnum LastCommand { get; set; }
        
        [Key(15)]
        public bool IsAllIn { get; set; }
        
        [Key(16)]
        public BestHandEntity? BestHand { get; set; }
        
        [Key(17)]
        public int RaiseAmount { get; set; }
        
        //currently held cards; use to show if player is bluffing with a raise or all in
        [Key(18)]
        public BestHandEntity CurrentBestHand { get; set; }
        
        [Key(19)]
        public int AllInAmount { get; set; }
        
        [Key(20)]
        public List<JokerEntity> JokerCards { get; set; } = new List<JokerEntity>();
        
        [Key(21)]
        public List<ActiveJokerEffectEntity> ActiveEffects { get; set; } = new List<ActiveJokerEffectEntity>();
        
        [Key(22)]
        public List<CardEntity> TempHoleCards { get; set; } = new List<CardEntity>();

        [Key(23)]
        public int MaxHoleCards { get; set; }
        
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
            TempHoleCards.Clear();
            MaxHoleCards = 2;
            OrderInQueue = 0;
            OriginalOrderInQueue = 0;
        }

        public bool CardsAreValid()
        {
            if (HoleCards.Count == 0)
                return true;
            
            return TempHoleCards.Count == 0 && HoleCards.Count == MaxHoleCards;
        }
    }
}