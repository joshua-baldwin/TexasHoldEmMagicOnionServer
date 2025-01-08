using THE.Entities;

namespace THE.GameLogic
{
    public static class WeightedCardSelector
    {
        private static readonly Random Rand = new();

        public static CardEntity GetCardEntity(List<CardEntity> cardEntityList, int totalWeight)
        {
            // totalWeight is the sum of all cards' weights
            var randomNumber = Rand.Next(0, totalWeight);

            CardEntity selectedCardEntity = null;
            foreach (var cardEntity in cardEntityList)
            {
                if (randomNumber < cardEntity.Weight)
                {
                    selectedCardEntity = cardEntity;
                    break;
                }

                randomNumber -= cardEntity.Weight;
            }

            return selectedCardEntity;
        }


        // static void Test(string[] args)
        // {
        //     List<CardEntity> CardEntitys = new GameLogicManager().CreateDeck();
        //     
        //
        //     // total the weigth
        //     int totalWeight = 0;
        //     foreach (CardEntity CardEntity in CardEntitys)
        //     {
        //         totalWeight += CardEntity.Weight;
        //     }
        //
        //     Dictionary<(Enums.CardSuitEnum, Enums.CardRankEnum), int> result = new Dictionary<(Enums.CardSuitEnum, Enums.CardRankEnum), int>();
        //
        //     CardEntity selectedCardEntity = null;
        //
        //     for (int i = 0; i < 1000; i++)
        //     {
        //         selectedCardEntity = WeightedCardSelector.GetCardEntity(CardEntitys, totalWeight);
        //         if (selectedCardEntity != null)
        //         {
        //             if (result.ContainsKey((selectedCardEntity.Suit, selectedCardEntity.Rank)))
        //             {
        //                 result[(selectedCardEntity.Suit, selectedCardEntity.Rank)] += 1;
        //             }
        //             else
        //             {
        //                 result.Add((selectedCardEntity.Suit, selectedCardEntity.Rank), 1);
        //             }
        //         }
        //     }
        //
        //     foreach (KeyValuePair<(Enums.CardSuitEnum, Enums.CardRankEnum), int> CardEntity in result)
        //     {
        //         Console.WriteLine($"{CardEntity.Key}: {CardEntity.Value}");
        //     }
        //
        //     result.Clear();
        //     Console.WriteLine();
        //     Console.ReadLine();
        //
        // }
    }
}