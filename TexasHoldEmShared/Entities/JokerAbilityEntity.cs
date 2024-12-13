using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class JokerAbilityEntity
    {
        [Key(0)]
        public int Id { get; set; }
        
        [Key(1)]
        public string Description { get; set; }
        
        [Key(2)]
        public List<int> AbilityEffectIds { get; set; }

        private List<AbilityEffectEntity> abilityEffects;

        public JokerAbilityEntity(int id, string description, List<int> abilityEffectIds)
        {
            Id = id;
            Description = description;
            AbilityEffectIds = abilityEffectIds;
        }

        public List<AbilityEffectEntity> GetAbilityEffects(List<AbilityEffectEntity> allAbilityEffectEntities)
        {
            if (abilityEffects != null)
                return abilityEffects;
            
            var effects = AbilityEffectIds.Select(id => allAbilityEffectEntities.First(x => x.Id == id)).ToList();
            abilityEffects = effects;
            return abilityEffects;
        }
    }
}