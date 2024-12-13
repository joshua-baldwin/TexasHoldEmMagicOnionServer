using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class JokerEntity
    {
        [Key(0)]
        public Guid Id { get; set; }
        
        [Key(1)]
        public int BuyCost { get; set; }
        
        [Key(2)]
        public int UseCost { get; set; }
        
        [Key(3)]
        public int MaxUses { get; set; }
        
        [Key(4)]
        public int CurrentUses { get; set; }
        
        [Key(5)]
        public List<int> AbilityIds { get; set; }
        
        [Key(6)]
        public bool CanUse { get; set; }
        
        [Key(7)]
        public Enums.JokerTypeEnum JokerType { get; set; }

        private List<JokerAbilityEntity> abilityEntities;

        public JokerEntity(Guid id, int buyCost, int useCost, int maxUses, int currentUses, List<int> abilityIds, bool canUse, Enums.JokerTypeEnum jokerType)
        {
            Id = id;
            BuyCost = buyCost;
            UseCost = useCost;
            MaxUses = maxUses;
            CurrentUses = currentUses;
            AbilityIds = abilityIds;
            CanUse = canUse;
            JokerType = jokerType;
        }
        
        public List<JokerAbilityEntity> GetAbilityEntities(List<JokerAbilityEntity> allAbilityEntities)
        {
            if (abilityEntities != null)
                return abilityEntities;
            
            var abilities = AbilityIds.Select(id => allAbilityEntities.First(x => x.Id == id)).ToList();
            abilityEntities = abilities;
            return abilityEntities;
        }
    }
}