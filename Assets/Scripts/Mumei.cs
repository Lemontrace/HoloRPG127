using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mumei : PlayableCharacter
{
    public SpriteRenderer effectRenderer;

    float basicAttackReach = Util.TileSize * 0.5f;
    float basicAttackWidth = Util.TileSize * 1.5f;
    float basicAttackDamage = 50f;

    public Sprite berrySprite;
    float healAmount = 80f;
    float damageBuffDuration = 5f;
    float damageBuffAmount = 20f;

    public Sprite nightmareSprite;
    float skill2Radius = 2f;
    float skill2StunDuration = 2f;

    override protected void Start()
    {
        Skill1 = FlatStrike;
        Skill2 = SkillOne;
        Skill3 = SkillTwo;
    }

    void FlatStrike()
    {
        Vector3 facing = GetComponent<Generic>().Facing;
        Vector3 point = transform.position + facing * (basicAttackReach / 2 + 0.5f * Util.TileSize);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, facing);
        float basicAttackDamage = this.basicAttackDamage + DamageBuff;
        var colliders = Physics2D.OverlapBoxAll(
            new Vector2(point.x, point.y),
            new Vector2(basicAttackWidth, basicAttackReach),
            rotation.eulerAngles.z + 90f
        );
        foreach (var collider in colliders)
            if (collider.gameObject.CompareTag("Enemy")) collider.GetComponent<Generic>().Damage(basicAttackDamage);
    }

    void SkillOne()
    {
        GetComponent<Generic>().Heal(healAmount);
        GetComponent<EffectHandler>().AddEffect(new Effect.DamageBuff(damageBuffDuration, damageBuffAmount));
        effectRenderer.sprite = berrySprite;
        Util.DelayedExecutionManager.ScheduleAction(DisableEffect, 2f);
    }

    void SkillTwo()
    {
        var center = transform.position;
        center.y -= 0.5f * Util.TileSize;
        var colliders = Physics2D.OverlapCircleAll(new Vector2(center.x, center.y), skill2Radius);
        foreach (var collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue;
            collider.GetComponent<EffectHandler>().AddEffect(new Effect.Stun(skill2StunDuration));
        }

        effectRenderer.sprite = nightmareSprite;
        Util.DelayedExecutionManager.ScheduleAction(DisableEffect, 3f);
    }

    private void DisableEffect()
    {
        effectRenderer.sprite = null;
    }
}
