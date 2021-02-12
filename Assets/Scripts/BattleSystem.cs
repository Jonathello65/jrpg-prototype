using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    public GameObject[] attackButtons = new GameObject[4];
    public Text[] attackButtonsText = new Text[4];
    public GameObject attacksButton;
    public GameObject blockButton;
    public TextMeshProUGUI battleText;
    public GameObject gameOverText;
    public GameObject winText;
    public Transform battleArrow;
    public List<Transform> enemySpawns = new List<Transform>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    protected BaseCharacter selectedCharacter;
    protected List<BaseCharacter> characters = new List<BaseCharacter>();
    protected List<BaseCharacter> players = new List<BaseCharacter>();
    public List<BaseCharacter> enemies = new List<BaseCharacter>();
    public List<BaseCharacter> deadCharacters = new List<BaseCharacter>();
    public SoundEffects sfx;
    protected int characterListPointer = 0;
    public int selectedAttack;
    public bool isAttacking = false;
    public bool isTargetingAlly = false;
    public bool isEnemyTurn = false;
    protected bool coroutineIsStart = false;
    public bool isTargetingMultiple = false;
    protected bool isRemovingCharacters = false;
    public bool learningNewAttack = false;
    protected int totalExp = 0;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (SceneManager.GetActiveScene().name != "BattleBoss")
            SpawnEnemies();
        yield return new WaitForSeconds(1);
        DisplayControls();
        characters.Clear();
        players.Clear();
        enemies.Clear();
        characters = GameObject.FindObjectsOfType<BaseCharacter>().ToList();
        characters = characters.OrderByDescending(x => x.speed).ToList();
        foreach (BaseCharacter character in characters)
        {
            if (character is Enemy)
                enemies.Add(character);
            else
                players.Add(character);
        }
        selectedCharacter = characters[0];
        DisplayTurnText();
        sfx = GameObject.Find("SFX").GetComponent<SoundEffects>();
        battleArrow = GameObject.Find("BattleArrow").transform;
        MoveArrow();
        sfx.PlaySound("BattleStart");
        if (selectedCharacter is Enemy)
        {
            StartCoroutine(EnemyTurn());
            HideControls();
        }
    }

    public void GetAttacks()
    {
        if (coroutineIsStart)
            return;
        
        if (attackButtons[0].activeSelf == true)
        {
            HideAttacks();
            DisplayTurnText();
            ResetAttackStatus();
        }
        else
        {
            DisplayAttacks();
            DisplayMoveText();
        }
    }

    public void DisplayAttacks()
    {
        for (int i = 0; i < 4; i++)
        {
            attackButtons[i].SetActive(true);
            attackButtonsText[i].text = selectedCharacter.getAttackName(i);
            if (attackButtonsText[i].text == "") { attackButtons[i].SetActive(false); }
        }
    }

    public void HideAttacks()
    {
        for (int i = 0; i < 4; i++)
        {
            attackButtons[i].SetActive(false);
        }
    }

    public void DisplayControls()
    {
        attacksButton.SetActive(true);
        blockButton.SetActive(true);
    }

    public void HideControls()
    {
        attacksButton.SetActive(false);
        blockButton.SetActive(false);
    }

    public void SelectAttack(int attack)
    {
        if (isEnemyTurn && attack == 0)
            return;

        if (isEnemyTurn)
            coroutineIsStart = false;
        
        if (coroutineIsStart)
            return;
        
        selectedAttack = attack;
        string targetReq = selectedCharacter.DetermineTargets(attack);

        switch (targetReq)
        {
            case "AllAllies":
                isTargetingAlly = true;
                isTargetingMultiple = true;
                StartCoroutine(TargetAllAllies());
                break;
            case "OneAlly":
                isTargetingAlly = true;
                if (!isEnemyTurn)
                    DisplayTargetText();
                if (isEnemyTurn)
                {
                    BaseCharacter target = players[Random.Range(0, players.Count)];
                    StartCoroutine(TargetAlly(target));
                }
                break;
            case "Self":
                isTargetingAlly = true;
                StartCoroutine(TargetAlly(selectedCharacter));
                break;
            case "OneEnemy":
                isAttacking = true;
                if (!isEnemyTurn)
                    DisplayTargetText();
                if (isEnemyTurn)
                {
                    BaseCharacter target = enemies[Random.Range(0, enemies.Count)];
                    StartCoroutine(AttackEnemy(target));
                }
                break;
            case "AllEnemies":
                isAttacking = true;
                isTargetingMultiple = true;
                StartCoroutine(AttackAllEnemies());
                break;
        }
    }

    public IEnumerator TargetAlly(BaseCharacter ally)
    {
        if (coroutineIsStart)
            yield break;

        if (!coroutineIsStart && isTargetingAlly)
            coroutineIsStart = true;

        if (isTargetingAlly)
        {
            HideAttacks();
            HideControls();
            DisplayActionText();

            yield return new WaitForSeconds(2);
            selectedCharacter.ExecuteAttack(selectedAttack, ally);
            yield return new WaitForSeconds(2);

            EndTurn();
        }
    }

    public IEnumerator TargetAllAllies()
    {
        if (coroutineIsStart)
            yield break;

        if (!coroutineIsStart)
            coroutineIsStart = true;
        
        HideAttacks();
        HideControls();
        DisplayActionText();

        yield return new WaitForSeconds(2);
        foreach (BaseCharacter player in players)
        {
            selectedCharacter.ExecuteAttack(selectedAttack, player);
        }
        yield return new WaitForSeconds(2);

        EndTurn();
    }

    public IEnumerator AttackEnemy(BaseCharacter enemy)
    {
        if (coroutineIsStart)
            yield break;
            
        if (!coroutineIsStart && isAttacking)
            coroutineIsStart = true;

        if (isAttacking)
        {
            HideAttacks();
            HideControls();
            DisplayActionText();

            yield return new WaitForSeconds(2);
            selectedCharacter.ExecuteAttack(selectedAttack, enemy);
            yield return new WaitForSeconds(2);

            EndTurn();
        }
    }

    public IEnumerator AttackAllEnemies()
    {
        if (coroutineIsStart)
            yield break;

        if (!coroutineIsStart)
            coroutineIsStart = true;

        HideAttacks();
        HideControls();
        DisplayActionText();

        yield return new WaitForSeconds(2);
        foreach (BaseCharacter enemy in enemies)
        {
            selectedCharacter.ExecuteAttack(selectedAttack, enemy);
        }
        yield return new WaitForSeconds(2);
        
        EndTurn();
    }

    public void ResetAttackStatus()
    {
        selectedAttack = 0;
        isAttacking = false;
        isTargetingAlly = false;
        isTargetingMultiple = false;
        isEnemyTurn = false;
    }

    public void EndTurn()
    {
        StartCoroutine(KillCharacters());
        StartCoroutine(ResetTurn());
    }

    public IEnumerator KillCharacters()
    {
        isRemovingCharacters = true;

        foreach (BaseCharacter character in characters)
        {
            if (character.health <= 0)
                deadCharacters.Add(character);
        }

        foreach (BaseCharacter deadCharacter in deadCharacters)
        {
            if (deadCharacter is PlayerCharacter)
            {
                RemoveAlly(deadCharacter);
                deadCharacter.GetComponent<PlayerCharacter>().isAlive = false;
                deadCharacter.characterSprite.enabled = false;
                deadCharacter.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                RemoveEnemy(deadCharacter);
                Destroy(deadCharacter.gameObject);
                totalExp += deadCharacter.exp;
            }

            if (characterListPointer >= characters.Count || (characterListPointer != 0 && characters[characterListPointer] != selectedCharacter))
                characterListPointer -= 1;

            battleText.text = deadCharacter.gameObject.name + " has been defeated!";
            yield return new WaitForSeconds(2);
        }

        deadCharacters.Clear();
        isRemovingCharacters = false;
    }

    public IEnumerator ResetTurn()
    {
        while(isRemovingCharacters)
            yield return new WaitForSeconds(0.1f);
        
        ResetAttackStatus();
        characterListPointer += 1;
        HideAttacks();
        if (players.Count == 0)
            StartCoroutine(GameOver());
        else if (enemies.Count == 0)
        {
            if (SceneManager.GetActiveScene().name == "BattleBoss")
                StartCoroutine(GameComplete());
            else
                StartCoroutine(EndBattle());
        }
        else 
            NextTurn();
    }

    public IEnumerator GameOver()
    {
        gameOverText.SetActive(true);
        battleText.text = "";
        sfx.PlaySound("GameOver");
        yield return new WaitForSeconds(5);
        ResetGame();
    }

    public IEnumerator GameComplete()
    {
        winText.SetActive(true);
        battleText.text = "";
        sfx.PlaySound("LevelUp");
        yield return new WaitForSeconds(5);
        ResetGame();
    }

    public void ResetGame()
    {
        List<PlayerCharacter> allPlayers = GameObject.FindObjectsOfType<PlayerCharacter>().ToList();
        foreach (PlayerCharacter player in allPlayers)
        {
            Destroy(player.gameObject);
        }
        CharacterStats.PlayerPosition = Vector3.zero;
        SceneManager.LoadScene("OpenWorld");
    }

    public IEnumerator EndBattle()
    {
        battleText.text = "All enemies defeated!";
        yield return new WaitForSeconds(2);

        foreach (PlayerCharacter player in players)
        {
            player.AddExp(totalExp);
        }
        battleText.text = "All allies gain " + totalExp + " experience!";
        yield return new WaitForSeconds(2);

        players.Clear();
        players = GameObject.FindObjectsOfType<BaseCharacter>().ToList();

        foreach (PlayerCharacter player in players)
        {
            int level = player.level, maxHealth = player.maxHealth, attack = player.attack, defense = player.defense, speed = player.speed;
            player.CheckLevelUp();
            if (level < player.level)
            {
                battleText.text = player.gameObject.name + " leveled up from " + level + " to " + player.level + "!";
                sfx.PlaySound("LevelUp");
                yield return new WaitForSeconds(2);
                battleText.text = "Attack increased by " + (player.attack - attack) + ", defense increased by " + (player.defense - defense) + ", and speed increased by " + (player.speed - speed) + "!";
                yield return new WaitForSeconds(2);
            }
            StartCoroutine(player.CheckNewAttack());
            while (learningNewAttack)
                yield return new WaitForSeconds(0.1f);
            
            player.SaveCharacterStats();
        }

        foreach (PlayerCharacter player in players)
        {
            player.characterSprite.enabled = false;
            player.transform.GetChild(0).gameObject.SetActive(false);
        }

        SceneManager.LoadScene("OpenWorld");
    }

    public IEnumerator EnemyTurn()
    {
        if (coroutineIsStart)
            yield break;
            
        if (!coroutineIsStart)
            coroutineIsStart = true;

        isEnemyTurn = true;
        yield return new WaitForSeconds(2);
        int attack = Random.Range(1, (selectedCharacter.attacks + 1));
        selectedAttack = attack;
        SelectAttack(attack);
    }

    public void NextTurn()
    {
        coroutineIsStart = false;
        if (characterListPointer >= characters.Count)
            characterListPointer = 0;
        selectedCharacter = characters[characterListPointer];
        MoveArrow();
        selectedCharacter.ProgressStatChange();
        DisplayTurnText();
        if (selectedCharacter is Enemy)
        {
            StartCoroutine(EnemyTurn());
            HideControls();
        }
        else
        {
            DisplayControls();
        }
    }

    public void RemoveEnemy(BaseCharacter enemy)
    {
        characters.Remove(enemy);
        enemies.Remove(enemy);
    }

    public void RemoveAlly(BaseCharacter ally)
    {
        characters.Remove(ally);
        players.Remove(ally);
    }

    public void DisplayTurnText()
    {
        battleText.text = selectedCharacter.gameObject.name + "'s turn.";
    }

    public void DisplayMoveText()
    {
        battleText.text = "Select a move to use.";
    }

    public void DisplayTargetText()
    {
        if (isAttacking)
        {
            battleText.text = "Select an enemy to use " + selectedCharacter.getAttackName(selectedAttack - 1) + " on.";
        }
        else
        {
            battleText.text = "Select an ally to use " + selectedCharacter.getAttackName(selectedAttack - 1) + " on.";
        }
    }

    public void DisplayActionText()
    {
        if (selectedAttack == 0)
            battleText.text = selectedCharacter.gameObject.name + " is blocking this turn!";
        else
            battleText.text = selectedCharacter.gameObject.name + " used " + selectedCharacter.getAttackName(selectedAttack - 1) + "!";
    }

    public void MoveArrow()
    {
        battleArrow.position = selectedCharacter.arrowPosition.position;
    }

    public void SpawnEnemies()
    {
        int numOfEnemies = Random.Range(2, 5);
        for (int i = 0; i < numOfEnemies; i++)
        {
            int enemy = Random.Range(0, enemyPrefabs.Count);
            SpawnEnemy(enemyPrefabs[enemy], enemySpawns[i]);
        }
    }

    public void SpawnEnemy(GameObject enemy, Transform spawn)
    {
        GameObject newEnemy = Instantiate(enemy, spawn.position, transform.rotation);
        newEnemy.transform.position -= (newEnemy.transform.GetChild(2).position - newEnemy.transform.position);
        newEnemy.GetComponent<BaseCharacter>().originalPos = newEnemy.transform.position;
        newEnemy.name = enemy.name;
        characters.Add(newEnemy.GetComponent<BaseCharacter>());
        enemies.Add(newEnemy.GetComponent<BaseCharacter>());

    }
}
