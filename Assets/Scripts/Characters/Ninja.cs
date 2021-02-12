using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : PlayerCharacter
{
    public override void Start()
    {
        attackNames = new string[] {"Slash", "Raikiri", "", ""};
        savedCharacter = CharacterStats.Ninja;
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
                Raikiri (enemy);
                break;
            case 3:
                Thunderstorm (enemy);
                break;
            case 4:
                HealingRain (enemy);
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
                return "OneEnemy";
            case 3:
                return "AllEnemies";
            case 4:
                return "AllAllies";
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

    protected void Raikiri (BaseCharacter enemy)
    {
        int damage = (int)(8 + (currentAttack));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("Lightning1");
    }

    protected void Thunderstorm (BaseCharacter enemy)
    {
        int damage = (int)(4 + (currentAttack) * 1.1f);
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("Lightning2");
    }

    protected void HealingRain (BaseCharacter target)
    {
        target.HealHealth(5 + attack);
        battleSystem.sfx.PlaySound("Heal2");
    }

    public override void SaveCharacterStats()
    {
        CharacterStats.Ninja = this;
    }

    public override IEnumerator CheckNewAttack()
    {
        battleSystem.learningNewAttack = true;
        if (level >= 3 && attackNames[2] == "")
        {
            attackNames[2] = "Thunderstorm";
            battleSystem.battleText.text = "Ninja learned Thunderstorm!";
            yield return new WaitForSeconds(2);
        }

        if (level >= 5 && attackNames[3] == "")
        {
            attackNames[3] = "Healing Rain";
            battleSystem.battleText.text = "Ninja learned Healing Rain!";
            yield return new WaitForSeconds(2);
        }
        battleSystem.learningNewAttack = false;
    }
}
