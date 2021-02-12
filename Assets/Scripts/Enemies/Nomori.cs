using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomori : Enemy
{
    public override void Start()
    {
        base.Start();
        attacks = 2;
        attackNames = new string[] {"Bite", "Fireball", "", ""};
    }

    public override void ExecuteAttack(int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                Bite(target);
                break;
            case 2:
                Fireball(target);
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

    public void Fireball(BaseCharacter target)
    {
        int damage = (int)(5 + (currentAttack * 1.3f));
        target.TakeDamage(damage);
        battleSystem.sfx.PlaySound("FireBlast");
    }
}
