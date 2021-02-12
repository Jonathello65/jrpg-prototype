using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimi : Enemy
{
    public override void Start()
    {
        base.Start();
        attacks = 3;
        attackNames = new string[] {"Bite", "Frighten", "Intimidate", ""};
    }

    public override void ExecuteAttack(int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                Bite(target);
                break;
            case 2:
                Frighten(target);
                break;
            case 3:
                Intimidate(target);
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
                return "OneAlly";
            case 2:
                return "OneAlly";
            case 3:
                return "OneAlly";
            default:
                return "Self";
        }
    }

    public void Bite(BaseCharacter target)
    {
        int damage = (int)(3 + (currentAttack * 1.3f));
        target.TakeDamage(damage);
        battleSystem.sfx.PlaySound("Bite");
    }

    public void Frighten(BaseCharacter target)
    {
        target.StatChange("Defense", "Debuff", 2);
        battleSystem.sfx.PlaySound("DefDebuff");
    }

    public void Intimidate(BaseCharacter target)
    {
        target.StatChange("Attack", "Debuff", 2);
        battleSystem.sfx.PlaySound("AtkDebuff");
    }
}
