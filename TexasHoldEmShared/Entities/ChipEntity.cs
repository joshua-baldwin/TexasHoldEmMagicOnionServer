using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class ChipEntity
    {
        [Key(0)]
        public Enums.ChipTypeEnum ChipType { get; private set; }

        public ChipEntity(Enums.ChipTypeEnum chipType)
        {
            ChipType = chipType;
        }
    }
}