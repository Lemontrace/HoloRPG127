using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize
{
    static void InitializeGame()
    {   
        var itemInfoList = ItemInfo.ItemInfoList = new List<ItemInfo>();

        itemInfoList.Add(new ItemInfo(0, null, (user, _) => //Ű�ƶ� ����
        {
            var generic = user.GetComponent<Generic>();
            generic.Heal(generic.MaxHitPoint);
        }));

        itemInfoList.Add(new ItemInfo(1, null, (user, _) => //ȭ����
        {
            var damageAmount = 10f;
            var effectDuration = 8f;
            var effectSourceId = "item:2";
            var effectHandler = user.GetComponent<EffectHandler>();
            System.Predicate<Effect> get = (effect) => {
                return (effect is Effect.DamageBuff) && (((Effect.DamageBuff)effect).Source == effectSourceId);
            };
            if (effectHandler.AllEffects.Exists(get)) effectHandler.AllEffects.Find(get).Duration += effectDuration;
            else effectHandler.AddEffect(new Effect.DamageBuff(effectDuration, damageAmount, effectSourceId));
        }));
        
    }
}
