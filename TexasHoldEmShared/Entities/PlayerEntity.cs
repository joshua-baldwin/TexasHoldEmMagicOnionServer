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
        public List<int> CurrentBets { get; private set; }
        
        [Key(9)]
        public int CurrentBetIndex { get; set; }
        
        [Key(10)]
        public bool HasTakenAction { get; set; }
        
        [Key(11)]
        public bool HasFolded { get; set; }
        
        [Key(12)]
        public Enums.CommandTypeEnum LastCommand { get; set; }
        
        [Key(13)]
        public bool IsAllIn { get; set; }
        
        [Key(14)]
        public BestHandEntity? BestHand { get; set; }
        
        public PlayerEntity(string name, Guid id, Enums.PlayerRoleEnum role)
        {
            Name = name;
            Id = id;
            PlayerRole = role;
            CurrentBets = new List<int> { 0 };
        }

        public void AddNewCurrentBet(int bet)
        {
            CurrentBets.Add(bet);
            CurrentBetIndex++;
        }

        public void AddToCurrentBet(int bet)
        {
            CurrentBets[CurrentBetIndex] += bet;
        }

        public void SubtractFromCurrentBet(int bet)
        {
            //when subtracting after making the main pot, if the order is raise, all in, call,
            //the player who raised needs to get subtracted from the first nonzero element
            //since they bet before the all in
            var index = CurrentBetIndex;
            while (CurrentBets[index] == 0)
                index--;

            Recurse(index, bet);
        }

        private void Recurse(int index, int remainder)
        {
            var newRemainder = CurrentBets[index] - remainder;
            if (newRemainder >= 0)
            {
                CurrentBets[index] -= remainder;
                return;
            }

            CurrentBets[index] = 0;
            index--;
                
            Recurse(index, -newRemainder);
        }

        public void ResetCurrentBets()
        {
            CurrentBets = new List<int> { 0 };
            CurrentBetIndex = 0;
        }

        public int GetCurrentBet()
        {
            return CurrentBets[CurrentBetIndex];
        }

        public void InitializeForNextRound()
        {
            HoleCards.Clear();
            CurrentBets = new List<int> { 0 };
            CurrentBetIndex = 0;
            HasTakenAction = false;
            HasFolded = false;
            LastCommand = 0;
            IsAllIn = false;
            BestHand = null;
        }
    }
}