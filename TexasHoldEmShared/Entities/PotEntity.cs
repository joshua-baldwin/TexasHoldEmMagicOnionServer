using System;
using System.Collections.Generic;
using MessagePack;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class PotEntity
    {
        [Key(0)]
        public Guid GoesToPlayer { get; set; }
        
        [Key(1)]
        public int PotAmount { get; set; }
        
        [Key(2)]
        public int AllInAmount { get; set; }
        
        [Key(3)]
        public List<PlayerEntity> EligiblePlayers { get; set; }

        public PotEntity(Guid goesToPlayer, int potAmount, int allInAmount, List<PlayerEntity> eligiblePlayers)
        {
            GoesToPlayer = goesToPlayer;
            PotAmount = potAmount;
            AllInAmount = allInAmount;
            EligiblePlayers = eligiblePlayers;
        }
    }
}