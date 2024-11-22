using System;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Utilities
{
    public static class UtilityMethods
    {
        public static Enums.ChipTypeEnum GetNextChipType(Enums.ChipTypeEnum chipType)
        {
            switch (chipType)
            {
                case Enums.ChipTypeEnum.White:
                    return Enums.ChipTypeEnum.Red;
                case Enums.ChipTypeEnum.Red:
                    return Enums.ChipTypeEnum.Blue;
                case Enums.ChipTypeEnum.Blue:
                    return Enums.ChipTypeEnum.Green;
                case Enums.ChipTypeEnum.Green:
                    return Enums.ChipTypeEnum.Black;
                case Enums.ChipTypeEnum.Black:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chipType), chipType, null);
            }
        }

        public static Enums.ChipTypeEnum GetPreviousChipType(Enums.ChipTypeEnum chipType)
        {
            switch (chipType)
            {
                case Enums.ChipTypeEnum.White:
                    return 0;
                case Enums.ChipTypeEnum.Red:
                    return Enums.ChipTypeEnum.White;
                case Enums.ChipTypeEnum.Blue:
                    return Enums.ChipTypeEnum.Red;
                case Enums.ChipTypeEnum.Green:
                    return Enums.ChipTypeEnum.Blue;
                case Enums.ChipTypeEnum.Black:
                    return Enums.ChipTypeEnum.Green;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chipType), chipType, null);
            }
        }
    }
}