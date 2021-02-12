using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tengu : Enemy
{
    public override void Start()
    {
        base.Start();
        attacks = 3;
        attackNames = new string[] {"Heal", "Healing Rain", "Strengthen", ""};
    }

    public override void ExecuteAttack(int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                Heal(target);
                break;
            case 2:
                HealingRain(target);
                break;
            case 3:
                Strengthen(target);
                break;
            default:
                break;
        }
    }

    public override string DetermineTargets(int attack)
    {
        switch (attack)
        {
            case 1:
                return "OneEnemy";
            case 2:
                return "AllEnemies";
            case 3:
                return "OneEnemy";
            default:
                return "Self";
        }
    }

    public void Heal(BaseCharacter target)
    {
        target.HealHealth(10 + attack);
        battleSystem.sfx.PlaySound("Heal1");
    }

    public void HealingRain(BaseCharacter target)
    {
        target.HealHealth(5 + attack);
        battleSystem.sfx.PlaySound("Heal2");
    }

    public void Strengthen(BaseCharacter target)
    {
        target.StatChange("Attack", "Buff", 2);
        battleSystem.sfx.PlaySound("AtkBuff");
    }
}
