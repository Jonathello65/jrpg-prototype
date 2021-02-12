using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerCharacter
{
    public override void Start()
    {
        attackNames = new string[] {"Arrow Shot", "Arrow Flurry", "", ""};
        savedCharacter = CharacterStats.Archer;
        base.Start();
        
    }

    public override void ExecuteAttack (int attack, BaseCharacter enemy)
    {
        switch (attack)
        {
            case 1:
                ArrowShot (enemy);
                break;
            case 2:
                ArrowFlurry (enemy);
                break;
            case 3:
                TeamRally (enemy);
                break;
            case 4:
                PiercingShot (enemy);
                break;
            default:
                Block ();
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
                return "AllAllies";
            case 4:
                return "OneEnemy";
            default:
                return "Self";
        }
    }

    protected void ArrowShot (BaseCharacter enemy)
    {
        int damage = (int)(4 + (currentAttack * 1.5f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("ArrowShot");
    }

    protected void ArrowFlurry (BaseCharacter enemy)
    {
        int damage = (int)(2 + (currentAttack * 1.5f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("ArrowBurst");
    }
    
    protected void TeamRally (BaseCharacter target)
    {
        target.StatChange("Attack", "Buff", 2);
        battleSystem.sfx.PlaySound("AtkBuff");
    }

    protected void PiercingShot (BaseCharacter enemy)
    {
        int damage = (int)(8 + (currentAttack * 2.0f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("ArrowShot");
    }

    public override void SaveCharacterStats()
    {
        CharacterStats.Archer = this;
    }

    public override IEnumerator CheckNewAttack()
    {
        battleSystem.learningNewAttack = true;
        if (level >= 3 && attackNames[2] == "")
        {
            attackNames[2] = "Team Rally";
            battleSystem.battleText.text = "Archer learned Team Rally!";
            yield return new WaitForSeconds(2);
        }

        if (level >= 5 && attackNames[3] == "")
        {
            attackNames[3] = "Piercing Shot";
            battleSystem.battleText.text = "Archer learned Piercing Shot!";
            yield return new WaitForSeconds(2);
        }
        battleSystem.learningNewAttack = false;
    }
}
