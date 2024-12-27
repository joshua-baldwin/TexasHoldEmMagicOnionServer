using System.Text;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.GameLogic
{
    public class JokerManager : IJokerManager
    {
        private List<JokerAbilityEntity> jokerAbilityEntities;
        private List<AbilityEffectEntity> jokerAbilityEffectEntities;
        private List<JokerEntity> jokerEntities;

        public List<JokerAbilityEntity> GetJokerAbilityEntities() => jokerAbilityEntities;
        public List<AbilityEffectEntity> GetJokerAbilityEffectEntities() => jokerAbilityEffectEntities;
        public List<JokerEntity> GetJokerEntities() => jokerEntities;

        public JokerManager()
        {
            CreateAbilityEffects();
            CreateAbilities();
            CreateJokers();
        }

        public bool CanPurchaseJoker(JokerEntity joker, PlayerEntity player, out Enums.BuyJokerResponseTypeEnum response)
        {
            if (player.Chips <= joker.BuyCost)
            {
                response = Enums.BuyJokerResponseTypeEnum.NotEnoughChips;
                return false;
            }

            response = Enums.BuyJokerResponseTypeEnum.Success;
            return true;
        }

        public Enums.BuyJokerResponseTypeEnum PurchaseJoker(int jokerId, PlayerEntity player, out bool isError, out JokerEntity addedJoker)
        {
            var jokerEntity = GetJokerEntities().First(x => x.JokerId == jokerId);
            if (!CanPurchaseJoker(jokerEntity, player, out var response))
            {
                addedJoker = null;
                isError = true;
                return response;
            }
            
            var jokerToAdd = new JokerEntity(jokerEntity);
            player.Chips -= jokerToAdd.BuyCost;
            player.JokerCards.Add(jokerToAdd);
            addedJoker = jokerToAdd;
            isError = false;
            return response;
        }

        public Enums.UseJokerResponseTypeEnum UseJoker(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, List<CardEntity> holeCardsToDiscard, out bool isError, out string actionMessage)
        {
            if (!CanUseJoker(jokerUser, targets, jokerEntity, out var message))
            {
                actionMessage = message;
                isError = true;
                return Enums.UseJokerResponseTypeEnum.Failed;
            }
            
            switch (jokerEntity.JokerType)
            {
                case Enums.JokerTypeEnum.Hand:
                    HandleHandInfluence(gameLogicManager, jokerUser, targets, jokerEntity, holeCardsToDiscard, out isError, out actionMessage);
                    break;
                case Enums.JokerTypeEnum.Action:
                    HandleActionInfluence(gameLogicManager, jokerUser, targets, jokerEntity, out isError, out actionMessage);
                    break;
                case Enums.JokerTypeEnum.Info:
                    HandleInfoInfluence(jokerUser, targets, jokerEntity, out isError, out actionMessage);
                    break;
                case Enums.JokerTypeEnum.Board:
                    HandleBoardInfluence(jokerUser, targets, jokerEntity, out isError, out actionMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (isError)
                return Enums.UseJokerResponseTypeEnum.Failed;
            
            gameLogicManager.AddJokerCostToPot(jokerEntity.UseCost);
            jokerUser.Chips -= jokerEntity.UseCost;
            if (jokerEntity.HandInfluenceType != Enums.HandInfluenceTypeEnum.DrawThenDiscard)
            {
                jokerEntity.CurrentUses++;
                if (jokerEntity.CurrentUses >= jokerEntity.MaxUses)
                    jokerUser.JokerCards.RemoveAll(x => x.UniqueId == jokerEntity.UniqueId);
            }

            isError = false;
            return Enums.UseJokerResponseTypeEnum.Success;
        }

        private void HandleHandInfluence(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, List<CardEntity> holeCardsToDiscard, out bool isError, out string message)
        {
            var sbEng = new StringBuilder();
            var sbJap = new StringBuilder();
            sbEng.Append($"Player {jokerUser.Name} used a hand influence joker. {jokerEntity.UseCost} chips were added to the pot.");
            sbJap.Append($"プレイヤー{jokerUser.Name}がhand influenceジョーカーを使いました。{jokerEntity.UseCost}チップがポットに追加された。");
            //currently assuming one ability and one effect
            foreach (var target in targets)
            {
                foreach (var effect in jokerEntity.JokerAbilityEntity.AbilityEffects)
                {
                    if (holeCardsToDiscard.Count > effect.EffectValue)
                    {
                        message = "Too many cards selected.\n選択されたカードが多すぎる。";
                        isError = true;
                        return;
                    }

                    sbEng.Append($"Player {target.Name} drew {effect.EffectValue} new card(s).");
                    sbJap.Append($"プレイヤー{target.Name}が{effect.EffectValue}カードを引いた。");
                    if (jokerEntity.HandInfluenceType == Enums.HandInfluenceTypeEnum.DiscardThenDraw)
                    {
                        gameLogicManager.DiscardToCardPool(target, holeCardsToDiscard);
                        target.HoleCards.AddRange(gameLogicManager.DrawFromCardPool(effect.EffectValue));
                    }
                    else
                    {
                        target.TempHoleCards.AddRange(gameLogicManager.DrawFromCardPool(effect.EffectValue));
                        //discard is a different api
                    }

                }
            }
            var full = new StringBuilder(sbEng.ToString());
            full.AppendLine();
            full.Append(sbJap);
            message = full.ToString();
            isError = false;
        }
        
        private void HandleActionInfluence(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, out bool isError, out string message)
        {
            var sbEng = new StringBuilder();
            var sbJp = new StringBuilder();
            sbEng.Append($"Player {jokerUser.Name} used an action influence joker. {jokerEntity.UseCost} chips were added to the pot.");
            sbJp.Append($"プレイヤー{jokerUser.Name}がaction influenceジョーカーを使いました。{jokerEntity.UseCost}チップがポットに追加された。");
            foreach (var target in targets)
            {
                foreach (var effect in jokerEntity.JokerAbilityEntity.AbilityEffects)
                {
                    if (target.ActiveEffects.Any(x => x.EffectId == effect.Id))
                    {
                        message = "This effect is already active.\nこの効果はもう既に付与されている。";
                        isError = true;
                        return;
                    }

                    switch (jokerEntity.ActionInfluenceType)
                    {
                        case Enums.ActionInfluenceTypeEnum.Force:
                            var jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            sbEng.Append($"Player {target.Name} must {effect.CommandType} on their next turn.");
                            sbJp.Append($"プレイヤー{target.Name}は次のターンに{effect.CommandType}をしないといけない。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.Prevent:
                            jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            sbEng.Append($"Player {target.Name} cannot {effect.CommandType} on their next turn.");
                            sbJp.Append($"プレイヤー{target.Name}は次のターンに{effect.CommandType}ができなくなりました。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.ChangePosition:
                            jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            gameLogicManager.UpdateQueue(target);
                            sbEng.Append($"Player {jokerUser.Name} is acting last for this turn.");
                            sbJp.Append($"このターンにプレイヤー{jokerUser.Name}が最後にアクションを行う。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.ChangeStack:
                            break;
                        case Enums.ActionInfluenceTypeEnum.IncreaseBettingRounds:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            var full = new StringBuilder(sbEng.ToString());
            full.AppendLine();
            full.Append(sbJp);
            message = full.ToString();
            isError = false;
        }
        
        private void HandleInfoInfluence(PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, out bool isError, out string message)
        {
            throw new NotImplementedException();
        }
        
        private void HandleBoardInfluence(PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, out bool isError, out string message)
        {
            throw new NotImplementedException();
        }

        private bool CanUseJoker(PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity joker, out string message)
        {
            if (joker.CurrentUses >= joker.MaxUses)
            {
                message = "You've reached the max use count for this Joker .\nこのジョーカーの利用上限数を超えた。";
                return false;
            }

            if (jokerUser.Chips <= joker.UseCost)
            {
                message = "You don't have enough chips to use this Joker.\nこのジョーカーを使うには必要なチップが足りない。";
                return false;
            }

            foreach (var target in targets)
            {
                foreach (var effect in joker.JokerAbilityEntity.AbilityEffects)
                {
                    if (target.ActiveEffects.Select(activeEffect => activeEffect.EffectId).Contains(effect.Id))
                    {
                        message = "This effect is already active.\nこの効果はもう既に発揮されている。";
                        return false;
                    }
                }
            }

            message = "";
            return true;
        }
        
        private void CreateJokers()
        {
            jokerEntities =
            [
                new JokerEntity(Guid.NewGuid(), 101, 2, 2, 3, 0, jokerAbilityEntities[0], true, Enums.JokerTypeEnum.Hand, Enums.HandInfluenceTypeEnum.DiscardThenDraw, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                new JokerEntity(Guid.NewGuid(), 102, 2, 2, 3, 0, jokerAbilityEntities[1], true, Enums.JokerTypeEnum.Hand, Enums.HandInfluenceTypeEnum.DrawThenDiscard, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                new JokerEntity(Guid.NewGuid(), 103, 2, 2, 3, 0, jokerAbilityEntities[2], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.Force, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 104, 2, 2, 3, 0, jokerAbilityEntities[3], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.Prevent, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 105, 2, 2, 3, 0, jokerAbilityEntities[4], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.ChangePosition, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                new JokerEntity(Guid.NewGuid(), 106, 2, 2, 3, 0, jokerAbilityEntities[5], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.ChangeStack, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 107, 2, 2, 3, 0, jokerAbilityEntities[6], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.IncreaseBettingRounds, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.None),
                new JokerEntity(Guid.NewGuid(), 108, 2, 2, 3, 0, jokerAbilityEntities[7], true, Enums.JokerTypeEnum.Info, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.CheckHand, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 109, 2, 2, 3, 0, jokerAbilityEntities[8], true, Enums.JokerTypeEnum.Board, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.IncreaseEquity, Enums.TargetTypeEnum.Self),
                new JokerEntity(Guid.NewGuid(), 110, 2, 2, 3, 0, jokerAbilityEntities[9], true, Enums.JokerTypeEnum.Board, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.DecreaseEquity, Enums.TargetTypeEnum.SinglePlayer),
            ];
        }

        private void CreateAbilities()
        {
            jokerAbilityEntities =
            [
                //hand influencers
                new JokerAbilityEntity(1, "Discard {effectValue} hole card(s) and draw {effectValue} more. ハンドを{effectValue}枚捨てて、{effectValue}枚弾き直す。", [jokerAbilityEffectEntities[0]]),
                new JokerAbilityEntity(2, "Draw {effectValue} card(s) and add to your hand, discard {effectValue} card(s). 追加で{effectValue}枚引いて、{effectValue}枚の中から{effectValue}枚捨てる。", [jokerAbilityEffectEntities[2]]),

                //action influencers
                new JokerAbilityEntity(3, "Force the target to {commandType}.ターゲットに{commandType}をさせる", [jokerAbilityEffectEntities[4]]),
                new JokerAbilityEntity(4, "Prevent the target from performing a(n) {commandType}. ターゲットの{commandType}アクションを消す。", [jokerAbilityEffectEntities[5]]),
                new JokerAbilityEntity(5, "Change position.", [jokerAbilityEffectEntities[6]]),
                new JokerAbilityEntity(6, "Change stack.", [jokerAbilityEffectEntities[7]]),
                new JokerAbilityEntity(7, "Increase the number of betting rounds.", [jokerAbilityEffectEntities[8]]),

                //info influencers
                new JokerAbilityEntity(8, "Check the target's hand", [jokerAbilityEffectEntities[9]]),

                //board influencers
                new JokerAbilityEntity(9, "Increase equity", [jokerAbilityEffectEntities[10]]),
                new JokerAbilityEntity(10, "Decrease the target's equity", [jokerAbilityEffectEntities[11]]),

                // new(2, "Make the target show their hand. ターゲットのハンドを見せさせる。", [3]),
                // new(3, "Make the target raise on their next turn. ターゲットを次のターンでレイズさせる。", [4]),
                // new(4, "Check even if there was a call or raise. コールやレイズがあったとしてもチェックできるようにする。", [5]),
                // new(5, "Add another community card. コミュニティカードを1枚追加する。", [6]),
                // new(6, "Prevent the use of {0} community card during showdown. ショーダウンの時に{0}枚のコミュニティカードを利用させなくなる。", [7]),
                // new(7, "Increase the rank of {0} of your hole cards. {0}枚のホールカードのランクを1つ上げる。", [9]),
                // new(8, "Decrease the rank of {0} of the target's hole cards. ターゲットの{0}枚のホールカードのランクを1つ下げる。", [11]),
                // new(9, "Prevent the use of Jokers on the next turn. 次のターンでジョーカーを利用させなくなる。", [13]),
                // new(10, "Treat this card as any standard card. 好きな通常のカードとして扱う。", [14]),
            ];
        }
        
        private void CreateAbilityEffects()
        {
            jokerAbilityEffectEntities =
            [
                new AbilityEffectEntity(1, 1, 1, Enums.CommandTypeEnum.None, "Discard then draw"),
                new AbilityEffectEntity(2, 1, 2, Enums.CommandTypeEnum.None, "Discard then draw"),
                new AbilityEffectEntity(3, 2, 1, Enums.CommandTypeEnum.None, "Draw then discard"),
                new AbilityEffectEntity(4, 2, 2, Enums.CommandTypeEnum.None, "Draw then discard"),

                new AbilityEffectEntity(5, 3, 1, Enums.CommandTypeEnum.Raise, "Make the target raise"),
                new AbilityEffectEntity(6, 4, 1, Enums.CommandTypeEnum.Check, "Prevent the target from checking"),
                new AbilityEffectEntity(7, 5, 0, Enums.CommandTypeEnum.None, "Change position"),
                new AbilityEffectEntity(8, 6, 0, Enums.CommandTypeEnum.None, "Change stack"),
                new AbilityEffectEntity(9, 7, 0, Enums.CommandTypeEnum.None, "Increase betting rounds"),

                new AbilityEffectEntity(10, 8, 0, Enums.CommandTypeEnum.None, "Check the target's hand"),

                new AbilityEffectEntity(11, 9, 0, Enums.CommandTypeEnum.None, "Increase equity"),
                new AbilityEffectEntity(12, 10, 0, Enums.CommandTypeEnum.None, "Decrease target's equity"),

                // new(5, 1, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Check even if there was a call or raise"),
                // new(6, 1, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Add another community card"),
                // new(7, 1, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Prevent the use of 1 community card"),
                // new(8, 1, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Prevent the use of 2 community cards"),
                // new(9, 1, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Increase the rank of 1 of your hole cards"),
                // new(10, 1, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Increase the rank of 2 of your hole cards"),
                // new(11, 1, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Decrease the rank of 1 hole card"),
                // new(12, 1, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Decrease the rank of 2 hole cards"),
                // new(13, 1, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Prevent the use of Jokers"),
                // new(14, 1, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Treat this card as any standard card"),
            ];
        }
    }
}