using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base character class to be inherited by player characters and enemies
public class BaseCharacter : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int attack;
    public int currentAttack;
    public int speed;
    public int currentSpeed;
    public int defense;
    public int currentDefense;
    public int level;
    public int exp;
    public string[] attackNames;
    public int attacks;
    protected int attackChangeTurns = 0;
    protected int defenseChangeTurns = 0;
    protected float shakeDuration = 0;
    protected float shakeAmount = 0.7f;
    public Vector3 originalPos;
    protected Transform healthBar;
    public Transform arrowPosition;
    public SpriteRenderer characterSprite;
    protected SpriteRenderer healthSprite;

    public BattleSystem battleSystem;

    public virtual void Start()
    {
        health = maxHealth;
        currentAttack = attack;
        currentDefense = defense;
        currentSpeed = speed;
        battleSystem = GameObject.FindObjectOfType<BattleSystem>();
        characterSprite = GetComponent<SpriteRenderer>();
        healthBar = transform.GetChild(0).GetChild(2);
        healthSprite = healthBar.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        arrowPosition = transform.GetChild(1);
        originalPos = transform.localPosition;
    }

    public void Update()
    {
        // Randomly shakes object while duration is above 0, based on shake amount. Shake amount decreases over time
		if (shakeDuration > 0)
		{
            //originalPos = transform.localPosition;
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
			shakeDuration -= Time.deltaTime;
            shakeAmount -= Time.deltaTime;
		}
		else
		{
			shakeDuration = 0f;
            transform.localPosition = originalPos;
		}
    }

    public virtual void ExecuteAttack (int attack, BaseCharacter target)
    {
        switch (attack)
        {
            case 1:
                // Use attack 1
                break;
            case 2:
                // Use attack 2
                break;
            case 3:
                // Use attack 3
                break;
            case 4:
                // Use attack 4
                break;
            default:
                // Block
                break;
        }
    }

    public void Block()
    {
        StatChange("Defense", "Buff", 1);
        battleSystem.sfx.PlaySound("DefBuff");
    }

    public void TakeDamage(int damage)
    {
        damage -= Mathf.CeilToInt((float)currentDefense / 2f);
        if (damage <= 0) { damage = 1; }
        health -= damage;
        SetHealthBar();
        
        if (!battleSystem.isTargetingMultiple)
            battleSystem.battleText.text = this.gameObject.name + " takes " + damage + " damage!";
        else if (!battleSystem.isEnemyTurn)
            battleSystem.battleText.text = "All enemies are hit!";
        else
            battleSystem.battleText.text = "All allies are hit!";

        ShakeObject(0.5f, 0.3f);
    }

    public void HealHealth(int heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
        SetHealthBar();

        if (!battleSystem.isTargetingMultiple)
        {
            if (health == maxHealth)
            {
                battleSystem.battleText.text = this.gameObject.name + " is fully healed!";
            }
            else
            {
                battleSystem.battleText.text = this.gameObject.name + " receives " + heal + " health!";
            }
        }
        else if (!battleSystem.isEnemyTurn)
            battleSystem.battleText.text = "All allies receive " + heal + " health!";
        else
            battleSystem.battleText.text = "All enemies receive " + heal + " health!";
    }

    public void StatChange(string stat, string buffType, int turns)
    {
        if (stat == "Attack")
        {
            if (currentAttack != attack) { currentAttack = attack; }

            if (buffType == "Buff")
            {
                currentAttack = (int)(attack * 2f);
                if (!battleSystem.isTargetingMultiple)
                    battleSystem.battleText.text = this.gameObject.name + "'s attack increased!";
                else if (!battleSystem.isEnemyTurn)
                    battleSystem.battleText.text = "All allies attack increased!";
                else
                    battleSystem.battleText.text = "All enemies attack increased!";
            }
            else
            {
                currentAttack = (int)(attack * 0.5f);
                if (!battleSystem.isTargetingMultiple)
                    battleSystem.battleText.text = this.gameObject.name + "'s attack decreased!";
                else if (!battleSystem.isEnemyTurn)
                    battleSystem.battleText.text = "All enemies attack decreased!";
                else
                    battleSystem.battleText.text = "All allies attack decreased!";
            }
            attackChangeTurns = turns;
        }
        else if (stat == "Defense")
        {
            if (currentDefense != defense) { currentDefense = defense; }

            if (buffType == "Buff")
            {
                currentDefense = (int)(defense * 2f);
                if (!battleSystem.isTargetingMultiple)
                    battleSystem.battleText.text = this.gameObject.name + "'s defense increased!";
                else if (!battleSystem.isEnemyTurn)
                    battleSystem.battleText.text = "All allies defense increased!";
                else
                    battleSystem.battleText.text = "All enemies defense increased!";
            }
            else
            {
                currentDefense = (int)(defense * 0.5f);
                if (!battleSystem.isTargetingMultiple)
                    battleSystem.battleText.text = this.gameObject.name + "'s defense decreased!";
                else if (!battleSystem.isEnemyTurn)
                    battleSystem.battleText.text = "All enemies defense decreased!";
                else
                    battleSystem.battleText.text = "All allies defense decreased!";
            }
            defenseChangeTurns = turns;
        }
        else
        {
            Debug.Log("Invalid stat");
        }
    }

    public void ProgressStatChange()
    {
        if (attackChangeTurns != 0) { attackChangeTurns -= 1; }
        if (defenseChangeTurns != 0) { defenseChangeTurns -= 1; }

        if (attackChangeTurns == 0) { currentAttack = attack; }
        if (defenseChangeTurns == 0) { currentDefense = defense; }
    }

    public virtual void KillCharacter()
    {
        // Destroy character if enemy or knockout if player
    }

    public virtual string DetermineTargets(int attack)
    {
        // Determine what targets the attack uses.
        return "Self";
    }

    public string getAttackName(int attack)
    {
        return attackNames[attack];
    }

    public void ShakeObject(float duration, float amplitude)
    {
        shakeDuration = duration;
        shakeAmount = amplitude;
    }

    public void SetHealthBar()
    {
        float size = (float)health / (float)maxHealth;
        size = Mathf.Clamp(size, 0, 1);
        healthBar.localScale = new Vector3(size, 1);
        if (size > 0.65f)
            SetHealthColor(Color.green);
        else if (size > 0.3f)
            SetHealthColor(Color.yellow);
        else
            SetHealthColor(Color.red);
    }

    public void SetHealthColor(Color newColor)
    {
        healthSprite.color = newColor;
    }
}
