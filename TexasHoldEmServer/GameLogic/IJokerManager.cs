using THE.Entities;
using THE.Shared.Enums;

namespace THE.GameLogic
{
    public interface IJokerManager
    {
        bool CanPurchaseJoker(JokerEntity joker, PlayerEntity player, out Enums.BuyJokerResponseTypeEnum response);
        Enums.BuyJokerResponseTypeEnum PurchaseJoker(int jokerId, PlayerEntity player, out bool isError, out JokerEntity addedJoker);
        Enums.UseJokerResponseTypeEnum UseJoker(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity joker, List<CardEntity> holeCardsToDiscard, List<CardEntity> cardsToUpdateWeight, out bool isError, out bool showHand, out string actionMessage);
        List<JokerAbilityEntity> GetJokerAbilityEntities();
        List<AbilityEffectEntity> GetJokerAbilityEffectEntities();
        List<JokerEntity> GetJokerEntities();
    }
}