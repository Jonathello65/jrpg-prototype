using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shinobi : PlayerCharacter
{
    public override void Start()
    {
        attackNames = new string[] {"Slice", "Fire Jutsu", "", ""};
        savedCharacter = CharacterStats.Shinobi;
        base.Start();
    }

    public override void ExecuteAttack (int attack, BaseCharacter enemy)
    {
        switch (attack)
        {
            case 1:
                Slice (enemy);
                break;
            case 2:
                FireJutsu (enemy);
                break;
            case 3:
                Heal (enemy);
                break;
            case 4:
                PiercingSlice (enemy);
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
                return "OneAlly";
            case 4:
                return "OneEnemy";
            default:
                return "Self";
        }
    }

    protected void Slice (BaseCharacter enemy)
    {
        int damage = (int)(4 + (currentAttack * 1.3f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("SwordSlash");
    }

    protected void FireJutsu (BaseCharacter enemy)
    {
        int damage = (int)(3 + (currentAttack) * 1.1f);
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("Fire");
    }

    protected void Heal (BaseCharacter target)
    {
        target.HealHealth(10 + attack);
        battleSystem.sfx.PlaySound("Heal1");
    }

    protected void PiercingSlice (BaseCharacter enemy)
    {
        int damage = (int)(6 + (currentAttack * 2.0f));
        enemy.TakeDamage(damage);
        battleSystem.sfx.PlaySound("SwordSlash");
    }

    public override void SaveCharacterStats()
    {
        CharacterStats.Shinobi = this;
    }

    public override IEnumerator CheckNewAttack()
    {
        battleSystem.learningNewAttack = true;
        if (level >= 3 && attackNames[2] == "")
        {
            attackNames[2] = "Heal";
            battleSystem.battleText.text = "Shinobi learned Heal!";
            yield return new WaitForSeconds(2);
        }

        if (level >= 5 && attackNames[3] == "")
        {
            attackNames[3] = "Piercing Slice";
            battleSystem.battleText.text = "Shinobi learned Piercing Slice!";
            yield return new WaitForSeconds(2);
        }
        battleSystem.learningNewAttack = false;
    }
}
