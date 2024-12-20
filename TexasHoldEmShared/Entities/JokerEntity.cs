using System;
using System.Collections.Generic;
using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class JokerEntity
    {
        [Key(0)]
        public Guid UniqueId { get; set; }
        
        [Key(1)]
        public int JokerId { get; set; }
        
        [Key(2)]
        public int BuyCost { get; set; }
        
        [Key(3)]
        public int UseCost { get; set; }
        
        [Key(4)]
        public int MaxUses { get; set; }
        
        [Key(5)]
        public int CurrentUses { get; set; }
        
        [Key(6)]
        public List<JokerAbilityEntity> JokerAbilityEntities { get; set; }
        
        [Key(7)]
        public bool CanUse { get; set; }
        
        [Key(8)]
        public Enums.JokerTypeEnum JokerType { get; set; }
        
        [Key(9)]
        public Enums.TargetTypeEnum TargetType { get; set; }
        
        public JokerEntity(Guid uniqueId, int jokerId, int buyCost, int useCost, int maxUses, int currentUses, List<JokerAbilityEntity> abilities, bool canUse, Enums.JokerTypeEnum jokerType, Enums.TargetTypeEnum targetType)
        {
            UniqueId = uniqueId;
            JokerId = jokerId;
            BuyCost = buyCost;
            UseCost = useCost;
            MaxUses = maxUses;
            CurrentUses = currentUses;
            JokerAbilityEntities = abilities;
            CanUse = canUse;
            JokerType = jokerType;
            TargetType = targetType;
        }

        public JokerEntity(JokerEntity jokerEntity)
        {
            UniqueId = Guid.NewGuid();
            JokerId = jokerEntity.JokerId;
            BuyCost = jokerEntity.BuyCost;
            UseCost = jokerEntity.UseCost;
            MaxUses = jokerEntity.MaxUses;
            CurrentUses = jokerEntity.CurrentUses;
            JokerAbilityEntities = jokerEntity.JokerAbilityEntities;
            CanUse = jokerEntity.CanUse;
            JokerType = jokerEntity.JokerType;
            TargetType = jokerEntity.TargetType;
        }
    }
}