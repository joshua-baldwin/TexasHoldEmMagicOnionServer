using System.Text;
using THE.Entities;
using THE.Shared.Enums;
using THE.Shared.Utilities;

namespace THE.GameLogic
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

        public Enums.UseJokerResponseTypeEnum UseJoker(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, List<CardEntity> cardEntities, out bool isError, out bool showHand, out string actionMessage)
        {
            if (!CanUseJoker(jokerUser, targets, jokerEntity, out var message))
            {
                actionMessage = message;
                showHand = false;
                isError = true;
                return Enums.UseJokerResponseTypeEnum.Failed;
            }
            showHand = false;
            switch (jokerEntity.JokerType)
            {
                case Enums.JokerTypeEnum.Hand:
                    HandleHandInfluence(gameLogicManager, jokerUser, targets, jokerEntity, cardEntities, out isError, out actionMessage);
                    break;
                case Enums.JokerTypeEnum.Action:
                    var actualTargets = jokerEntity.TargetType == Enums.TargetTypeEnum.All ? gameLogicManager.GetAllPlayers() : targets;
                    HandleActionInfluence(gameLogicManager, jokerUser, actualTargets, jokerEntity, out isError, out actionMessage);
                    break;
                case Enums.JokerTypeEnum.Info:
                    HandleInfoInfluence(jokerUser, targets, jokerEntity, out isError, out showHand, out actionMessage);
                    break;
                case Enums.JokerTypeEnum.Board:
                    HandleBoardInfluence(gameLogicManager, jokerUser, jokerEntity, cardEntities, out isError, out actionMessage);
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

        private void HandleHandInfluence(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, List<CardEntity> cardEntities, out bool isError, out string message)
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
                    if (cardEntities.Count > effect.TargetNumber)
                    {
                        message = "Too many cards selected.\n選択されたカードが多すぎる。";
                        isError = true;
                        return;
                    }

                    switch (jokerEntity.HandInfluenceType)
                    {
                        case Enums.HandInfluenceTypeEnum.DrawThenDiscard:
                        case Enums.HandInfluenceTypeEnum.DiscardThenDraw:
                            sbEng.Append($"Player {target.Name} drew {effect.TargetNumber} new card(s).");
                            sbJap.Append($"プレイヤー{target.Name}が{effect.TargetNumber}カードを引いた。");
                            if (jokerEntity.HandInfluenceType == Enums.HandInfluenceTypeEnum.DiscardThenDraw)
                            {
                                gameLogicManager.DiscardToCardPool(target, cardEntities);
                                gameLogicManager.DrawFromCardPool(target, effect.TargetNumber, false);
                            }
                            else
                            {
                                gameLogicManager.DrawFromCardPool(target, effect.TargetNumber, true);
                                //discard is a different api
                            }
                            break;
                        case Enums.HandInfluenceTypeEnum.DrawCard:
                            sbEng.Append($"Player {target.Name} drew {effect.TargetNumber} card(s) from the pool.");
                            sbJap.Append($"プレイヤー{target.Name}が{effect.TargetNumber}枚のカードをプールから引いた。");
                            gameLogicManager.DrawFromCardPool(target, cardEntities, true);
                            break;
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
                            var jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.TargetNumber, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            sbEng.Append($"Player {target.Name} must {effect.CommandType} on their next turn.");
                            sbJp.Append($"プレイヤー{target.Name}は次のターンに{effect.CommandType}をしないといけない。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.Prevent:
                            jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.TargetNumber, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            sbEng.Append($"Player {target.Name} cannot {effect.CommandType} on their next turn.");
                            sbJp.Append($"プレイヤー{target.Name}は次のターンに{effect.CommandType}ができなくなりました。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.ChangePosition:
                            jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.TargetNumber, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            gameLogicManager.UpdateQueue(target);
                            sbEng.Append($"Player {jokerUser.Name} is acting last for this turn.");
                            sbJp.Append($"このターンにプレイヤー{jokerUser.Name}が最後にアクションを行う。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.ChangeStack:
                            break;
                        case Enums.ActionInfluenceTypeEnum.IncreaseBettingRounds:
                            jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.TargetNumber, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            gameLogicManager.IncreaseNumberOfBettingRounds();
                            sbEng.Append("The number of betting turns has increased by one.");
                            sbJp.Append("ベットラウンドの数が１つに増えた。");
                            break;
                        case Enums.ActionInfluenceTypeEnum.PreventJoker:
                            jokerEffect = new ActiveJokerEffectEntity(jokerEntity.JokerId, jokerEntity.JokerType, jokerEntity.HandInfluenceType, jokerEntity.ActionInfluenceType, jokerEntity.InfoInfluenceType, jokerEntity.BoardInfluenceType, effect.Id, effect.EffectValue, effect.TargetNumber, effect.CommandType);
                            target.ActiveEffects.Add(jokerEffect);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            if (jokerEntity.ActionInfluenceType == Enums.ActionInfluenceTypeEnum.PreventJoker)
            {
                sbEng.Append("Players cannot use Jokers during this turn.");
                sbJp.Append("このターンジョーカーを使えなくなった。");
            }

            var full = new StringBuilder(sbEng.ToString());
            full.AppendLine();
            full.Append(sbJp);
            message = full.ToString();
            isError = false;
        }
        
        private void HandleInfoInfluence(PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity jokerEntity, out bool isError, out bool showHand, out string message)
        {
            var sbEng = new StringBuilder();
            var sbJp = new StringBuilder();
            sbEng.Append($"Player {jokerUser.Name} used an info influence joker. {jokerEntity.UseCost} chips were added to the pot.");
            sbJp.Append($"プレイヤー{jokerUser.Name}がinfo influenceジョーカーを使いました。{jokerEntity.UseCost}チップがポットに追加された。");
            foreach (var target in targets)
            {
                foreach (var effect in jokerEntity.JokerAbilityEntity.AbilityEffects)
                {
                    switch (jokerEntity.InfoInfluenceType)
                    {
                        case Enums.InfoInfluenceTypeEnum.CheckHand:
                            //don't add joker effect
                            //let player use as many times as wanted in case the target changes cards
                            showHand = true;
                            sbEng.Append($"Player {target.Name} showed their hand to player {jokerUser.Name}.");
                            sbJp.Append($"プレイヤー{target.Name}が{jokerUser.Name}にハンドを見せた。");
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
            showHand = false;
        }
        
        private void HandleBoardInfluence(IGameLogicManager gameLogicManager, PlayerEntity jokerUser, JokerEntity jokerEntity, List<CardEntity> cardEntities, out bool isError, out string message)
        {
            var sbEng = new StringBuilder();
            var sbJp = new StringBuilder();
            sbEng.Append($"Player {jokerUser.Name} used a board influence joker. {jokerEntity.UseCost} chips were added to the pot.");
            sbJp.Append($"プレイヤー{jokerUser.Name}がboard influenceジョーカーを使いました。{jokerEntity.UseCost}チップがポットに追加された。");
            foreach (var effect in jokerEntity.JokerAbilityEntity.AbilityEffects)
            {
                switch (jokerEntity.BoardInfluenceType)
                {
                    case Enums.BoardInfluenceTypeEnum.IncreaseCardWeight:
                        gameLogicManager.UpdateCardWeight(cardEntities, effect.EffectValue, true);
                        sbEng.Append($"Player {jokerUser.Name} updated the card weights.");
                        sbJp.Append($"プレイヤー{jokerUser.Name}がカードの重みを更新した。");
                        break;
                    case Enums.BoardInfluenceTypeEnum.DecreaseCardWeight:
                        gameLogicManager.UpdateCardWeight(cardEntities, effect.EffectValue, false);
                        sbEng.Append($"Player {jokerUser.Name} updated the card weights.");
                        sbJp.Append($"プレイヤー{jokerUser.Name}がカードの重みを更新した。");
                        break;
                    case Enums.BoardInfluenceTypeEnum.PreventCommunityCard:
                        gameLogicManager.LockCommunityCards(cardEntities);
                        sbEng.Append("A community card has been locked.");
                        sbJp.Append("コミュニティカードがロックされた。");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var full = new StringBuilder(sbEng.ToString());
            full.AppendLine();
            full.Append(sbJp);
            message = full.ToString();
            isError = false;
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
            
            if (jokerUser.ActiveEffects.Any(activeEffect => activeEffect.EffectId == Constants.CantUseJokerAbilityEffectId))
            {
                message = "You can't use Jokers on this turn.\nこのターンジョーカー使えない。";
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
                new JokerEntity(Guid.NewGuid(), 101, 2, 3, 3, 0, jokerAbilityEntities[0], true, Enums.JokerTypeEnum.Hand, Enums.HandInfluenceTypeEnum.DiscardThenDraw, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                new JokerEntity(Guid.NewGuid(), 102, 2, 3, 3, 0, jokerAbilityEntities[1], true, Enums.JokerTypeEnum.Hand, Enums.HandInfluenceTypeEnum.DrawThenDiscard, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                new JokerEntity(Guid.NewGuid(), 103, 2, 6, 3, 0, jokerAbilityEntities[2], true, Enums.JokerTypeEnum.Hand, Enums.HandInfluenceTypeEnum.DrawCard, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                
                new JokerEntity(Guid.NewGuid(), 104, 2, 4, 3, 0, jokerAbilityEntities[3], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.Force, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 105, 2, 4, 3, 0, jokerAbilityEntities[4], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.Prevent, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 106, 2, 3, 3, 0, jokerAbilityEntities[5], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.ChangePosition, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.Self),
                //new JokerEntity(Guid.NewGuid(), 107, 2, 2, 3, 0, jokerAbilityEntities[6], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.ChangeStack, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                new JokerEntity(Guid.NewGuid(), 108, 2, 2, 3, 0, jokerAbilityEntities[7], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.IncreaseBettingRounds, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.None),
                new JokerEntity(Guid.NewGuid(), 109, 2, 5, 3, 0, jokerAbilityEntities[8], true, Enums.JokerTypeEnum.Action, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.PreventJoker, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.All),
                
                new JokerEntity(Guid.NewGuid(), 110, 2, 4, 3, 0, jokerAbilityEntities[9], true, Enums.JokerTypeEnum.Info, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.CheckHand, Enums.BoardInfluenceTypeEnum.None, Enums.TargetTypeEnum.SinglePlayer),
                
                new JokerEntity(Guid.NewGuid(), 111, 2, 3, 3, 0, jokerAbilityEntities[10], true, Enums.JokerTypeEnum.Board, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.IncreaseCardWeight, Enums.TargetTypeEnum.None),
                new JokerEntity(Guid.NewGuid(), 112, 2, 3, 3, 0, jokerAbilityEntities[11], true, Enums.JokerTypeEnum.Board, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.DecreaseCardWeight, Enums.TargetTypeEnum.None),
                new JokerEntity(Guid.NewGuid(), 113, 2, 5, 3, 0, jokerAbilityEntities[12], true, Enums.JokerTypeEnum.Board, Enums.HandInfluenceTypeEnum.None, Enums.ActionInfluenceTypeEnum.None, Enums.InfoInfluenceTypeEnum.None, Enums.BoardInfluenceTypeEnum.PreventCommunityCard, Enums.TargetTypeEnum.All),
            ];
        }

        private void CreateAbilities()
        {
            jokerAbilityEntities =
            [
                //hand influencers
                new JokerAbilityEntity(1, "Discard {targetNumber} hole card(s) and draw {targetNumber} more. ハンドを{targetNumber}枚捨てて、{targetNumber}枚弾き直す。", [jokerAbilityEffectEntities[0]]),
                new JokerAbilityEntity(2, "Draw {targetNumber} card(s) and add to your hand, discard {targetNumber} card(s). 追加で{targetNumber}枚引いて、{targetNumber}枚捨てる。", [jokerAbilityEffectEntities[2]]),
                new JokerAbilityEntity(3, "Draw the designated cards from the pool.\n指定したカードをプールから引く。", [jokerAbilityEffectEntities[4]]),

                //action influencers
                new JokerAbilityEntity(4, "Force the target to {commandType}.ターゲットに{commandType}をさせる", [jokerAbilityEffectEntities[5]]),
                new JokerAbilityEntity(5, "Prevent the target from performing a(n) {commandType}. ターゲットの{commandType}アクションを消す。", [jokerAbilityEffectEntities[6]]),
                new JokerAbilityEntity(6, "Act last during this turn.\nこのターンに最後にアクションを行う", [jokerAbilityEffectEntities[7]]),
                new JokerAbilityEntity(7, "Change stack.", [jokerAbilityEffectEntities[8]]),
                new JokerAbilityEntity(8, "Increase the number of betting rounds.\nベットラウンドの数を増える", [jokerAbilityEffectEntities[9]]),
                new JokerAbilityEntity(9, "Prevent the use of Jokers on the next turn. 次のターンでジョーカーを利用できなくさせる。", [jokerAbilityEffectEntities[10]]),

                //info influencers
                new JokerAbilityEntity(10, "Check the hand of {targetNumber} target(s).\n{targetNumber}人のターゲットのハンドを確認する", [jokerAbilityEffectEntities[11]]),

                //board influencers
                new JokerAbilityEntity(11, "Increase the weight of {targetNumber} card(s) by {effectValue} times.\n{targetNumber}枚のカードの重みを{effectValue}倍で上げる。", [jokerAbilityEffectEntities[12]]),
                new JokerAbilityEntity(12, "Decrease the weight of {targetNumber} card(s) by {effectValue} times.\n{targetNumber}枚のカードの重みを{effectValue}倍で下げる。", [jokerAbilityEffectEntities[13]]),
                new JokerAbilityEntity(13, "Prevent the use of {targetNumber} community card(s) during showdown. ショーダウンの時に{targetNumber}枚のコミュニティカードを利用させなくなる。", [jokerAbilityEffectEntities[14]]),
                
                // new(4, "Check even if there was a call or raise. コールやレイズがあったとしてもチェックできるようにする。", [5]),
                // new(5, "Add another community card. コミュニティカードを1枚追加する。", [6]),
                
                // new(7, "Increase the rank of {0} of your hole cards. {0}枚のホールカードのランクを1つ上げる。", [9]),
                // new(8, "Decrease the rank of {0} of the target's hole cards. ターゲットの{0}枚のホールカードのランクを1つ下げる。", [11]),
            ];
        }
        
        private void CreateAbilityEffects()
        {
            jokerAbilityEffectEntities =
            [
                new AbilityEffectEntity(1, 1, 0, 1, Enums.CommandTypeEnum.None, "Discard then draw"),
                new AbilityEffectEntity(2, 1, 0, 2, Enums.CommandTypeEnum.None, "Discard then draw"),
                new AbilityEffectEntity(3, 2, 0, 1, Enums.CommandTypeEnum.None, "Draw then discard"),
                new AbilityEffectEntity(4, 2, 0, 2, Enums.CommandTypeEnum.None, "Draw then discard"),
                new AbilityEffectEntity(5, 3, 0, 1, Enums.CommandTypeEnum.None, "Draw the designated cards from the pool"),

                new AbilityEffectEntity(6, 4, 0, 1, Enums.CommandTypeEnum.Raise, "Make the target raise"),
                new AbilityEffectEntity(7, 5, 0, 1, Enums.CommandTypeEnum.Check, "Prevent the target from checking"),
                new AbilityEffectEntity(8, 6, 0, 0, Enums.CommandTypeEnum.None, "Change position"),
                new AbilityEffectEntity(9, 7, 0, 0, Enums.CommandTypeEnum.None, "Change stack"),
                new AbilityEffectEntity(10, 8, 0, 0, Enums.CommandTypeEnum.None, "Increase betting rounds"),
                new AbilityEffectEntity(11, 9, 0, 0, Enums.CommandTypeEnum.None, "Prevent the use of Jokers"),

                new AbilityEffectEntity(12, 10, 0, 1, Enums.CommandTypeEnum.None, "Check the target's hand"),

                new AbilityEffectEntity(13, 11, 2, 1, Enums.CommandTypeEnum.None, "Increase card weight"),
                new AbilityEffectEntity(14, 12, 2, 1, Enums.CommandTypeEnum.None, "Decrease card weight"),
                new AbilityEffectEntity(15, 13, 0, 1, Enums.CommandTypeEnum.None, "Prevent the use of community cards"),

                // new(5, 1, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Check even if there was a call or raise"),
                // new(6, 1, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Add another community card"),
                
                // new(9, 1, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Increase the rank of 1 of your hole cards"),
                // new(10, 1, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Increase the rank of 2 of your hole cards"),
                // new(11, 1, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Decrease the rank of 1 hole card"),
                // new(12, 1, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Decrease the rank of 2 hole cards"),
            ];
        }
    }
}