using System.Collections.Generic;
using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class BestHandEntity
    {
        [Key(0)]
        public List<CardEntity> Cards { get; set; } =  new List<CardEntity>();
        
        [Key(1)]
        public Enums.HandRankingType HandRanking { get; set; }
    }
}