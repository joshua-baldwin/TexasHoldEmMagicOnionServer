using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Utilities
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
        
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            var n = array.Length;
            while (n > 1) 
            {
                var k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
        
        // public static void AddChips(this List<ChipEntity> chips, List<ChipEntity> newChips)
        // {
        //     foreach (var chip in newChips)
        //     {
        //         if (chips.FirstOrDefault(x => x.ChipType == chip.ChipType) == null)
        //             chips.Add(new ChipEntity(chip.ChipType, chip.ChipCount));
        //         else
        //             chips.First(x => x.ChipType == chip.ChipType).ChipCount += chip.ChipCount;
        //     }
        // }
        //
        // public static void AddChips(this List<ChipEntity> chips, int newChips)
        // {
        //     var remainingChips = newChips;
        //     var chipsToAdd = new List<ChipEntity>();
        //     while (remainingChips > 0)
        //     {
        //         Enums.ChipTypeEnum chipToAdd;
        //         if (remainingChips - (int)Enums.ChipTypeEnum.Black >= 0)
        //             chipToAdd = Enums.ChipTypeEnum.Black;
        //         else if (remainingChips - (int)Enums.ChipTypeEnum.Green >= 0)
        //             chipToAdd = Enums.ChipTypeEnum.Green;
        //         else if (remainingChips - (int)Enums.ChipTypeEnum.Blue >= 0)
        //             chipToAdd = Enums.ChipTypeEnum.Blue;
        //         else if (remainingChips - (int)Enums.ChipTypeEnum.Red >= 0)
        //             chipToAdd = Enums.ChipTypeEnum.Red;
        //         else
        //             chipToAdd = Enums.ChipTypeEnum.White;
        //         
        //         AddChip(ref chipsToAdd, chipToAdd);
        //         remainingChips -= (int)chipToAdd;
        //     }
        //     foreach (var chip in chipsToAdd)
        //     {
        //         if (chips.FirstOrDefault(x => x.ChipType == chip.ChipType) == null)
        //             chips.Add(new ChipEntity(chip.ChipType, chip.ChipCount));
        //         else
        //             chips.First(x => x.ChipType == chip.ChipType).ChipCount += chip.ChipCount;
        //     }
        // }
        //
        // private static void AddChip(ref List<ChipEntity> chipsToAdd, Enums.ChipTypeEnum chipTypeToAdd)
        // {
        //     if (chipsToAdd.FirstOrDefault(x => x.ChipType == chipTypeToAdd) == null)
        //         chipsToAdd.Add(new ChipEntity(chipTypeToAdd, 1));
        //     else
        //         chipsToAdd.First(x => x.ChipType == chipTypeToAdd).ChipCount++;
        // }
        //
        // public static void RemoveChips(this List<ChipEntity> chips, int newChips)
        // {
        //     var remainingChips = newChips;
        //     var chipsToRemove = new List<ChipEntity>();
        //     while (remainingChips > 0)
        //     {
        //         Enums.ChipTypeEnum chipToRemove;
        //         if (remainingChips - (int)Enums.ChipTypeEnum.Black >= 0)
        //             chipToRemove = Enums.ChipTypeEnum.Black;
        //         else if (remainingChips - (int)Enums.ChipTypeEnum.Green >= 0)
        //             chipToRemove = Enums.ChipTypeEnum.Green;
        //         else if (remainingChips - (int)Enums.ChipTypeEnum.Blue >= 0)
        //             chipToRemove = Enums.ChipTypeEnum.Blue;
        //         else if (remainingChips - (int)Enums.ChipTypeEnum.Red >= 0)
        //             chipToRemove = Enums.ChipTypeEnum.Red;
        //         else
        //             chipToRemove = Enums.ChipTypeEnum.White;
        //         
        //         AddChip(ref chipsToRemove, chipToRemove);
        //         remainingChips -= (int)chipToRemove;
        //     }
        //
        //     var canBreak = false;
        //     foreach (var chip in chipsToRemove)
        //     {
        //         if (canBreak)
        //             break;
        //         
        //         CheckChipAndRemove(chips, chip.ChipType, chip.ChipCount, (int)chip.ChipType * chip.ChipCount, out canBreak);
        //     }
        // }
        //
        // private static void CheckChipAndRemove(List<ChipEntity> chips, Enums.ChipTypeEnum chipTypeToCheck, int chipCount, int originalBetSum, out bool done)
        // {
        //     done = false;
        //     var myChip = chips.FirstOrDefault(x => x.ChipType == chipTypeToCheck);
        //     if (myChip != null)
        //     {
        //         if (chipCount <= myChip.ChipCount)
        //         {
        //             //if we have the chip just subtract it
        //             myChip.ChipCount -= chipCount;
        //         }
        //         else
        //         {
        //             //if we dont have the chip, take the next biggest denomination and give change
        //             var newChipToCheck = UtilityMethods.GetNextChipType(chipTypeToCheck);
        //             CheckChipAndRemove(chips, newChipToCheck, 1, originalBetSum, out done);
        //             GiveBackChange(chips, chipTypeToCheck, (int)newChipToCheck - originalBetSum);
        //             done = true;
        //         }
        //     }
        // }
        //
        // private static void GiveBackChange(List<ChipEntity> chips, Enums.ChipTypeEnum chipTypeToCheck, int changeToGiveBack)
        // {
        //     if (changeToGiveBack == (int)chipTypeToCheck)
        //     {
        //         chips.First(x => x.ChipType == chipTypeToCheck).ChipCount++;
        //     }
        //     else if (changeToGiveBack > (int)chipTypeToCheck)
        //     {
        //         var rem = changeToGiveBack % (int)chipTypeToCheck;
        //         var toAdd = rem == 0 ? changeToGiveBack / (int)chipTypeToCheck : 1;
        //         chips.First(x => x.ChipType == chipTypeToCheck).ChipCount += toAdd;
        //         var previousChipType = UtilityMethods.GetPreviousChipType(chipTypeToCheck);
        //         if (previousChipType != 0)
        //             GiveBackChange(chips, previousChipType, rem);
        //     }
        //     else
        //     {
        //         var previousChipType = UtilityMethods.GetPreviousChipType(chipTypeToCheck);
        //         if (previousChipType != 0)
        //             GiveBackChange(chips, previousChipType, changeToGiveBack);
        //     }
        // }
        //
        // public static int GetTotalChipValue(this List<ChipEntity> chips)
        // {
        //     return chips.Sum(chip => (int)chip.ChipType * chip.ChipCount);
        // }
    }
}