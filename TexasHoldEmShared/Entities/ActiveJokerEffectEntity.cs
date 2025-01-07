using MessagePack;
using THE.Shared.Enums;

namespace THE.Entities
{
    [MessagePackObject]
    public class ActiveJokerEffectEntity
    {
        [Key(0)]
        public int JokerId { get; set; }
        
        [Key(1)]
        public Enums.JokerTypeEnum JokerType { get; set; }
        
        [Key(2)]
        public Enums.HandInfluenceTypeEnum HandInfluenceType { get; set; }
        
        [Key(3)]
        public Enums.ActionInfluenceTypeEnum ActionInfluenceType { get; set; }
        
        [Key(4)]
        public Enums.InfoInfluenceTypeEnum InfoInfluenceType { get; set; }
        
        [Key(5)]
        public Enums.BoardInfluenceTypeEnum BoardInfluenceType { get; set; }
        
        [Key(6)]
        public int EffectId { get; set; }
        
        [Key(7)]
        public int EffectValue { get; set; }
        
        [Key(8)]
        public Enums.CommandTypeEnum CommandType { get; set; }

        public ActiveJokerEffectEntity(int jokerId, Enums.JokerTypeEnum jokerType, Enums.HandInfluenceTypeEnum handInfluenceType, Enums.ActionInfluenceTypeEnum actionInfluenceType, Enums.InfoInfluenceTypeEnum infoInfluenceType, Enums.BoardInfluenceTypeEnum boardInfluenceType, int effectId, int effectValue, Enums.CommandTypeEnum commandType)
        {
            JokerId = jokerId;
            JokerType = jokerType;
            HandInfluenceType = handInfluenceType;
            ActionInfluenceType = actionInfluenceType;
            InfoInfluenceType = infoInfluenceType;
            BoardInfluenceType = boardInfluenceType;
            EffectId = effectId;
            EffectValue = effectValue;
            CommandType = commandType;
        }
    }
}