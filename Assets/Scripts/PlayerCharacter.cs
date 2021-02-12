using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    public int expToNextLvl;
    public bool isAlive = true;
    protected float nextLvlModifier = 2.8f;
    protected PlayerCharacter savedCharacter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void Start()
    {
        base.Start();
        if (savedCharacter != null && savedCharacter.isAlive)
        {
            SetCharacterStats();
            battleSystem.RemoveAlly(savedCharacter);
            Destroy(savedCharacter.gameObject);
        }

        if (savedCharacter != null && !savedCharacter.isAlive)
            Destroy(gameObject);

        SetHealthBar();
    }

    public void AddExp (int experience)
    {
        exp += experience;
    }

    // Check exp if character can level up
    public void CheckLevelUp ()
    {
        if (exp >= expToNextLvl)
        {
            expToNextLvl = (int)(expToNextLvl * nextLvlModifier);
            level++;
            LevelUp ();
        }
    }

    // Level up character stats
    protected void LevelUp ()
    {
        maxHealth += Random.Range(3, 8);
        health = maxHealth;
        attack += Random.Range(1, 4);
        defense += Random.Range(1, 4);
        speed += Random.Range(1, 4);
        CheckLevelUp();
    }

    public virtual IEnumerator CheckNewAttack()
    {
        // Check if character learned new attack
        yield return new WaitForSeconds(0);
    }

    void OnMouseDown()
    {
        StartCoroutine(battleSystem.TargetAlly(this));
    }

    protected void SetCharacterStats()
    {
        level = savedCharacter.level;
        exp = savedCharacter.exp;
        expToNextLvl = savedCharacter.expToNextLvl;
        health = savedCharacter.health;
        maxHealth = savedCharacter.maxHealth;
        attack = savedCharacter.attack;
        defense = savedCharacter.defense;
        speed = savedCharacter.speed;
        attackNames = savedCharacter.attackNames;
        currentAttack = attack;
        currentDefense = defense;
        currentSpeed = speed;
    }

    public virtual void SaveCharacterStats()
    {
        // Save character stats to static script
    }
}
