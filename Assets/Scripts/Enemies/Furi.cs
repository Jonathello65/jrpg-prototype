using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furi : Enemy
{
    public override void Start()
    {
        base.Start();
        attacks = 2;
        attackNames = new string[] {"Scratch", "Rage", "", ""};
    }

    public override void ExecuteAttack(int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                Scratch(target);
                break;
            case 2:
                Rage();
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
                return "Self";
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

    public void Rage()
    {
        this.StatChange("Attack", "Buff", 2);
        battleSystem.sfx.PlaySound("AtkBuff");
    }
}
