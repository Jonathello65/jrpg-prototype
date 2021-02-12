using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onikuma : Enemy
{
    public override void Start()
    {
        base.Start();
        attacks = 4;
        attackNames = new string[] {"Claw Slash", "Earthquake", "Monstrous Rally", "Call of the Mountain"};
    }

    public override void ExecuteAttack(int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                Slash(target);
                break;
            case 2:
                Earthquake(target);
                break;
            case 3:
                Rally(target);
                break;
            case 4:
                StartCoroutine(Call());
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
                return "AllAllies";
            case 3:
                return "AllEnemies";
            case 4:
                return "Self";
            default:
                return "Self";
        }
    }

    public void Slash(BaseCharacter target)
    {
        int damage = (int)(8 + (currentAttack * 1.5f));
        target.TakeDamage(damage);
        battleSystem.sfx.PlaySound("Claw");
    }

    public void Earthquake(BaseCharacter target)
    {
        int damage = (int)(5 + (currentAttack * 1.5f));
        target.TakeDamage(damage);
        battleSystem.sfx.PlaySound("GameOver");
    }

    public void Rally(BaseCharacter target)
    {
        target.StatChange("Attack", "Buff", 3);
        battleSystem.sfx.PlaySound("AtkBuff");
    }

    public IEnumerator Call()
    {
        battleSystem.sfx.PlaySound("Roar");
        if (battleSystem.enemies.Count == 1)
        {
            int enemy = Random.Range(0, battleSystem.enemyPrefabs.Count);
            battleSystem.SpawnEnemy(battleSystem.enemyPrefabs[enemy], battleSystem.enemySpawns[1]);
            enemy = Random.Range(0, battleSystem.enemyPrefabs.Count);
            battleSystem.SpawnEnemy(battleSystem.enemyPrefabs[enemy], battleSystem.enemySpawns[2]);

            battleSystem.battleText.text = "Onikuma summoned reinforcements!";
            yield return new WaitForSeconds(2);
        }
        else
        {
            battleSystem.battleText.text = "No one came to Onikuma's aid.";
            yield return new WaitForSeconds(2);
        }
    }
}
