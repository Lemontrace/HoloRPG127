using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Initialize
{
    static void InitializeGame()
    {
        var itemCount = 10;
        var itemInfoList = ItemInfo.ItemInfoList = new List<ItemInfo>();
        for (int i = 0; i < itemCount; i++)
        {
            itemInfoList.Add(new ItemInfo(i, null));
        }

        itemInfoList[0].useFunction = (user, _) => //Å°¾Æ¶ó ±êÅÐ
        {
            var generic = user.GetComponent<Generic>();
            generic.Heal(generic.MaxHitPoint);
        };

        itemInfoList[1].useFunction = (user, _) => //È­°úÀÚ
        {
            var damageAmount = 10f;
            var effectDuration = 8f;
            var effectSourceId = "item:1";
            var effectHandler = user.GetComponent<EffectHandler>();
            System.Predicate<Effect> get = (effect) =>
            {
                return (effect is Effect.DamageBuff) && (((Effect.DamageBuff)effect).Source == effectSourceId);
            };
            if (effectHandler.AllEffects.Exists(get)) effectHandler.AllEffects.Find(get).Duration += effectDuration;
            else effectHandler.AddEffect(new Effect.DamageBuff(effectDuration, damageAmount, effectSourceId));
        };

    }
}
