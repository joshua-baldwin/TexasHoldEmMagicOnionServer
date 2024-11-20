using System.Collections;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.Utilities
{
    public static class ExtensionMethods
    {
        public static T GetRandomElement<T>(this List<T> source)
        {
            var rand = new Random();
            var index = rand.Next(0, source.Count);
            return source[index];
        }

        public static List<T> Shuffle<T>(this List<T> source)
        {
            var rand = new Random();
            var sourceArray = source.ToArray();
            rand.Shuffle(sourceArray);
            return sourceArray.ToList();
        }

        public static void AddChips(this List<ChipEntity> chips, List<ChipEntity> newChips)
        {
            foreach (var chip in newChips)
            {
                if (chips.FirstOrDefault(x => x.ChipType == chip.ChipType) == null)
                    chips.Add(chip);
                else
                    chips.First(x => x.ChipType == chip.ChipType).ChipCount += chip.ChipCount;
            }
        }
        
        public static void RemoveChips(this List<ChipEntity> chips, List<ChipEntity> newChips)
        {
            foreach (var chip in newChips)
            {
                var toRemove = chips.FirstOrDefault(x => x.ChipType == chip.ChipType);
                if (toRemove != null)
                    toRemove.ChipCount -= chip.ChipCount;
            }
        }

        public static int GetTotalChipValue(this List<ChipEntity> chips)
        {
            return chips.Sum(chip => (int)chip.ChipType * chip.ChipCount);
        }
    }
}