using System;
using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class AbilityEffectEntity
    {
        [Key(0)]
        public int Id { get; set; }
        
        [Key(1)]
        public int AbilityId { get; set; }
        
        [Key(2)]
        public int EffectValue { get; set; }
        
        [Key(3)]
        public Enums.CommandTypeEnum CommandType { get; set; }
        
        [Key(4)]
        public Enums.HandInfluenceTypeEnum HandInfluenceType { get; set; }
        
        [Key(5)]
        public Enums.ActionInfluenceTypeEnum ActionInfluenceType { get; set; }
        
        [Key(6)]
        public string Description { get; set; }

        public AbilityEffectEntity(int id, int abilityId, int effectValue, Enums.CommandTypeEnum commandType, Enums.HandInfluenceTypeEnum handInfluenceType, Enums.ActionInfluenceTypeEnum actionInfluenceType, string description)
        {
            Id = id;
            AbilityId = abilityId;
            EffectValue = effectValue;
            CommandType = commandType;
            HandInfluenceType = handInfluenceType;
            ActionInfluenceType = actionInfluenceType;
            Description = description;
        }
    }
}