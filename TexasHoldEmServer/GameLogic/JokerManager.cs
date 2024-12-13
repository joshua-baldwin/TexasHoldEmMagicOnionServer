using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.GameLogic
{
    public static class JokerManager
    {
        private static List<JokerAbilityEntity> jokerAbilities;
        private static List<AbilityEffectEntity> jokerAbilityEffects;
        private static List<JokerEntity> jokerCards;

        public static void CreateJokerDeck()
        {
            jokerAbilities =
            [
                //hand influencers
                new JokerAbilityEntity(1, "Discard {0} hole card(s) and draw {0} more. ハンドを{0}枚捨てて、{0}枚弾き直す。", [1]),
                new JokerAbilityEntity(2, "Draw {0} card(s) and add to your hand, discard {0} card(s). 追加で{0}枚引いて、{0}枚の中から{0}枚捨てる。", [3]),
                
                //action influencers
                new JokerAbilityEntity(3, "Force the target to {0}", [5]),
                new JokerAbilityEntity(4, "Prevent the target from performing a(n) {0}. ターゲットの{0}アクションを消す。", [6]),
                new JokerAbilityEntity(5, "Change position.", [7]),
                new JokerAbilityEntity(6, "Change stack.", [8]),
                new JokerAbilityEntity(7, "Increase the number of betting rounds.", [9]),
                
                //info influencers
                new JokerAbilityEntity(8, "Check the target's hand", [10]),
                
                //board influencers
                new JokerAbilityEntity(9, "Increase equity", [11]),
                new JokerAbilityEntity(10, "Decrease the target's equity", [12]),
                
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
            
            jokerAbilityEffects =
            [
                new AbilityEffectEntity(1, 1, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Discard then draw"),
                new AbilityEffectEntity(2, 1, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Discard then draw"),
                new AbilityEffectEntity(3, 2, 1, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Draw then discard"),
                new AbilityEffectEntity(4, 2, 2, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Draw then discard"),
                
                new AbilityEffectEntity(5, 3, 0, Enums.EffectTargetTypeEnum.LeftEnemy, Enums.CommandTypeEnum.Raise, "Make the target raise"),
                new AbilityEffectEntity(6, 4, 0, Enums.EffectTargetTypeEnum.LeftEnemy, Enums.CommandTypeEnum.Check, "Prevent the target from checking"),
                new AbilityEffectEntity(7, 5, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Change position"),
                new AbilityEffectEntity(8, 6, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Change stack"),
                new AbilityEffectEntity(9, 7, 0, Enums.EffectTargetTypeEnum.None, Enums.CommandTypeEnum.None, "Increase betting rounds"),
                
                new AbilityEffectEntity(10, 8, 0, Enums.EffectTargetTypeEnum.LeftEnemy, Enums.CommandTypeEnum.None, "Check the target's hand"),
                
                new AbilityEffectEntity(11, 9, 0, Enums.EffectTargetTypeEnum.Self, Enums.CommandTypeEnum.None, "Increase equity"),
                new AbilityEffectEntity(12, 10, 0, Enums.EffectTargetTypeEnum.LeftEnemy, Enums.CommandTypeEnum.None, "Decrease target's equity"),
                
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

            jokerCards =
            [
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [1], true, Enums.JokerTypeEnum.Hand),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [2], true, Enums.JokerTypeEnum.Hand),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [3], true, Enums.JokerTypeEnum.Action),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [4], true, Enums.JokerTypeEnum.Action),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [5], true, Enums.JokerTypeEnum.Action),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [6], true, Enums.JokerTypeEnum.Action),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [7], true, Enums.JokerTypeEnum.Action),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [8], true, Enums.JokerTypeEnum.Info),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [9], true, Enums.JokerTypeEnum.Board),
                new JokerEntity(Guid.NewGuid(), 2, 2, 3, 0, [10], true, Enums.JokerTypeEnum.Board),
            ];
        }

        public static List<JokerAbilityEntity> GetJokerAbilities() => jokerAbilities;
        public static List<AbilityEffectEntity> GetJokerAbilityEffects() => jokerAbilityEffects;
        public static List<JokerEntity> GetJokerCards() => jokerCards;
    }
}