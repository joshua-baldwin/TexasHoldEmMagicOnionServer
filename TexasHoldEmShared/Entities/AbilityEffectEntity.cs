using MessagePack;
using THE.Shared.Enums;

namespace THE.Entities
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
        public int TargetNumber { get; set; }
        
        [Key(4)]
        public Enums.CommandTypeEnum CommandType { get; set; }
        
        [Key(5)]
        public string Description { get; set; }

        public AbilityEffectEntity(int id, int abilityId, int effectValue, int targetNumber, Enums.CommandTypeEnum commandType, string description)
        {
            Id = id;
            AbilityId = abilityId;
            EffectValue = effectValue;
            TargetNumber = targetNumber;
            CommandType = commandType;
            Description = description;
        }
    }
}