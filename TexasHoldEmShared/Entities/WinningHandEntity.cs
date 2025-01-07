using System.Collections.Generic;
using MessagePack;
using THE.Shared.Enums;

namespace THE.Entities
{
    [MessagePackObject]
    public class WinningHandEntity
    {
        [Key(0)]
        public PlayerEntity? Winner { get; set; }
        
        [Key(1)]
        public List<CardEntity> Cards { get; set; } = new List<CardEntity>();

        [Key(2)]
        public Enums.HandRankingType HandRanking { get; set; } = Enums.HandRankingType.Nothing;
        
        [Key(3)]
        public List<PlayerEntity> TiedWith { get; set; } =  new List<PlayerEntity>();
        
        [Key(4)]
        public int PotToWinner { get; set; }
        
        [Key(5)]
        public int PotToTiedWith { get; set; }
        
        [Key(6)]
        public string PotName { get; set; }
    }
}