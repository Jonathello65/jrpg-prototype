using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : PlayerCharacter
{
    public override void Start()
    {
        attackNames = new string[] {"Slash", "Charge", "", ""};
        savedCharacter = CharacterStats.Samurai;
        base.Start();
        
    }

    public override void ExecuteAttack (int attack, BaseCharacter enemy)
    {
        switch (attack)
        {
            case 1:
                Slash (enemy);
                break;
            case 2:
                Charge ();
                break;
            case 3:
                Bolster (enemy);
                break;
            case 4:
                SpinningStrike (enemy);
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
                return "Self";
            case 3:
                return "AllAllies";
            case 4:
                return "AllEnemies";
            default:
                return "Self";
        }
    }

    protected void Slash (BaseCharacter enemy)
    {
        int damage = (int)(5 + (currentAttack * 1.5f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("SwordSlash");
    }

    protected void Charge ()
    {
        StatChange("Attack", "Buff", 2);
        battleSystem.sfx.PlaySound("AtkBuff");
    }

    protected void Bolster (BaseCharacter target)
    {
        StatChange("Defense", "Buff", 2);
        battleSystem.sfx.PlaySound("DefBuff");
    }

    protected void SpinningStrike (BaseCharacter enemy)
    {
        int damage = (int)(4 + (currentAttack * 1.4f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("SwordSlash");
    }

    public override void SaveCharacterStats()
    {
        CharacterStats.Samurai = this;
    }

    public override IEnumerator CheckNewAttack()
    {
        battleSystem.learningNewAttack = true;
        if (level >= 3 && attackNames[2] == "")
        {
            attackNames[2] = "Bolster";
            battleSystem.battleText.text = "Samurai learned Bolster!";
            yield return new WaitForSeconds(2);
        }

        if (level >= 5 && attackNames[3] == "")
        {
            attackNames[3] = "Spinning Strike";
            battleSystem.battleText.text = "Samurai learned Spinning Strike!";
            yield return new WaitForSeconds(2);
        }
        battleSystem.learningNewAttack = false;
    }
}
