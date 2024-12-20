using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.GameLogic
{
    public interface IJokerManager
    {
        bool CanPurchaseJoker(JokerEntity joker, PlayerEntity player, out Enums.BuyJokerResponseTypeEnum response);
        Enums.BuyJokerResponseTypeEnum PurchaseJoker(JokerEntity joker, PlayerEntity player);
        Enums.UseJokerResponseTypeEnum UseJoker(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity joker, List<int> holeCardIndicesToDiscard, out bool isError, out string actionMessage);
        List<JokerAbilityEntity> GetJokerAbilityEntities();
        List<AbilityEffectEntity> GetJokerAbilityEffectEntities();
        List<JokerEntity> GetJokerEntities();
    }
}