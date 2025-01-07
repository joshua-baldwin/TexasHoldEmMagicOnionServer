using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace THE.Entities
{
    [MessagePackObject]
    public class JokerAbilityEntity
    {
        [Key(0)]
        public int Id { get; set; }
        
        [Key(1)]
        public string Description { get; set; }
        
        [Key(2)]
        public List<AbilityEffectEntity> AbilityEffects { get; set; }

        public JokerAbilityEntity(int id, string description, List<AbilityEffectEntity> abilityEffects)
        {
            Id = id;
            Description = description;
            AbilityEffects = abilityEffects;
        }

        public string GetDescription()
        {
            var newString = Description;
            if (newString.Contains("{effectValue}"))
                newString = newString.Replace("{effectValue}", AbilityEffects.First().EffectValue.ToString());
            if (newString.Contains("{commandType}"))
                newString = newString.Replace("{commandType}", AbilityEffects.First().CommandType.ToString());

            return newString;
        }
    }
}