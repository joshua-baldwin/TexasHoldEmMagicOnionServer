namespace THE.MagicOnion.Shared.Entities
{
    public enum ChipTypeEnum
    {
        White = 1,
        Red = 5,
        Blue = 10,
        Green = 25,
        Black = 100
    }
    public class ChipEntity
    {
        public ChipTypeEnum ChipType { get; private set; }

        public ChipEntity(ChipTypeEnum chipType)
        {
            ChipType = chipType;
        }
    }
}