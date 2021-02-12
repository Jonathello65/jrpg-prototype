using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sarugami : Enemy
{
    public override void Start()
    {
        base.Start();
        attacks = 3;
        attackNames = new string[] {"Scratch", "Rally", "Shriek", ""};
    }

    public override void ExecuteAttack(int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                Scratch(target);
                break;
            case 2:
                Rally(target);
                break;
            case 3:
                Shriek(target);
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
                return "AllEnemies";
            case 3:
                return "OneAlly";
            default:
                return "Self";
        }
    }

    public void Scratch(BaseCharacter target)
    {
        int damage = (int)(3 + (currentAttack * 1.3f));
        target.TakeDamage(damage);
        battleSystem.sfx.PlaySound("Claw");
    }

    public void Rally(BaseCharacter target)
    {
        target.StatChange("Attack", "Buff", 2);
        battleSystem.sfx.PlaySound("AtkBuff");
    }

    public void Shriek(BaseCharacter target)
    {
        target.StatChange("Defense", "Debuff", 2);
        battleSystem.sfx.PlaySound("DefDebuff");
    }
}
