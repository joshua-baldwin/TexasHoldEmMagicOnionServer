using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.Utilities;

namespace TexasHoldEmServer.GameLogic
{
    public class GameLogicManager
    {
        private Queue<PlayerEntity> playerQueue = new Queue<PlayerEntity>();
        private PlayerEntity currentPlayer;
        public PlayerEntity GetCurrentPlayer() => currentPlayer;
        public void SetupGame(List<PlayerEntity> players)
        {
            SetRoles(players);
            SetCards(players);
            SetChips(players);
            CreateQueue(players);
        }
        
        private void SetRoles(List<PlayerEntity> players)
        {
            var first = players.GetRandomElement();
            first.IsDealer = true;
            first.CardPool = CreateDeck();

            var shuffled = players.Shuffle();
            shuffled[0].PlayerRole = Enums.PlayerRoleEnum.SmallBlind;
            shuffled[1].PlayerRole = Enums.PlayerRoleEnum.BigBlind;
        }

        private void SetCards(List<PlayerEntity> players)
        {
            var dealer = players.First(x => x.IsDealer);
            var deck = dealer.CardPool;
            var shuffled = deck.Shuffle();
            //give to dealer last
            var dealerIndex = players.IndexOf(dealer);
            var startIndex = dealerIndex + 1 >= players.Count ? 0 : dealerIndex + 1;
            var dealt = 0;
            while (dealt < players.Count)
            {
                var card1 = shuffled.GetRandomElement();
                shuffled.Remove(card1);
                var card2 = shuffled.GetRandomElement();
                shuffled.Remove(card2);
                players[startIndex].CardHand[0] = card1;
                players[startIndex].CardHand[1] = card2;
                dealt++;
                if (startIndex + 1 >= players.Count)
                    startIndex = 0;
                else
                    startIndex++;
            }

            dealer.CardPool = shuffled;
        }
        
        private void SetChips(List<PlayerEntity> players)
        {
            //start with 50 chips
            //25 + 10 + 5 + 5 + 1 + 1 + 1 + 1 + 1
            foreach (var player in players)
            {
                var chips = new List<ChipEntity>();
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Green));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Blue));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Red));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Red));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                player.Chips = chips;
            }
        }
        
        private List<CardEntity> CreateDeck()
        {
            var suits = Enum.GetValues<Enums.CardSuitEnum>();
            var ranks = Enum.GetValues<Enums.CardRankEnum>();
            var deck = new List<CardEntity>();
            foreach (var suit in suits)
            {
                deck.AddRange(ranks.Select(rank => new CardEntity(suit, rank)));
            }

            return deck;
        }
        
        private void CreateQueue(List<PlayerEntity> players)
        {
            var index = players.FindIndex(player => player.PlayerRole == Enums.PlayerRoleEnum.BigBlind);
            while (playerQueue.Count < players.Count)
            {
                playerQueue.Enqueue(players[index]);
                if (index + 1 >= players.Count)
                    index = 0;
                else
                    index++;
            }
            
            currentPlayer = playerQueue.Dequeue();
        }
    }
}