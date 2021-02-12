using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    public SpriteRenderer sprite;
    public SoundEffects sfx;
    public GameObject music1;
    public GameObject music2;
    public GameObject battleStartText;
    public GameObject healText;
    private List<PlayerCharacter> characters = new List<PlayerCharacter>();
    private bool healedOnce = false;
    private bool walkingVertical = false;
    private bool stopMovement = false;
    private bool onBeach = false;
    private bool onForest1 = false;
    private bool onForest2 = false;
    private bool onMountain = false;
    private bool inSafe = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        characters = GameObject.FindObjectsOfType<PlayerCharacter>().ToList();
        if (CharacterStats.PlayerPosition != Vector3.zero)
            transform.position = CharacterStats.PlayerPosition;
        int song = Random.Range(0, 2);
        if (song == 0)
            music1.SetActive(true);
        else
            music2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMovement) 
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement = movement.normalized;
        }

        if (movement == Vector2.zero || stopMovement)
            animator.speed = 0f;
        else
            animator.speed = 1f;

        if (movement.y != 0)
            walkingVertical = true;
        else
            walkingVertical = false;

        animator.SetFloat("Horizontal", Mathf.Abs(movement.x));
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("WalkingVertical", walkingVertical);

        if (movement.x > 0)
            sprite.flipX = true;

        if (movement.x < 0)
            sprite.flipX = false;
    }

    void FixedUpdate()
    {
        if (!stopMovement)
            rb.MovePosition(rb.position + (movement * speed * Time.fixedDeltaTime));

        if (movement != Vector2.zero && !inSafe && !stopMovement)
        {
            int encounter = Random.Range(0, 1500);
            if (encounter <= 1)
            {
                StartCoroutine(StartBattle());
            }
        }
    }

    IEnumerator StartBattle()
    {
        stopMovement = true;
        CharacterStats.PlayerPosition = transform.position;
        battleStartText.SetActive(true);
        sfx.PlaySound("Encounter");
        yield return new WaitForSeconds(2);
        if (onBeach)
            SceneManager.LoadScene("BattleBeach");
        else if (onForest1)
            SceneManager.LoadScene("BattleForest1");
        else if (onForest2)
            SceneManager.LoadScene("BattleForest2");
        else if (onMountain)
            SceneManager.LoadScene("BattleMountain");
        else
            SceneManager.LoadScene("BattleForest1");
    }

    IEnumerator StartBoss()
    {
        stopMovement = true;
        sfx.PlaySound("Roar");
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("BattleBoss");
    }

    IEnumerator HealParty()
    {
        stopMovement = true;
        healedOnce = true;
        healText.SetActive(true);
        foreach (PlayerCharacter character in characters)
        {
            character.health = character.maxHealth;
            character.isAlive = true;
        }
        sfx.PlaySound("Heal2");
        yield return new WaitForSeconds(3);
        healText.SetActive(false);
        stopMovement = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Beach"))
        {
            onBeach = true;
            onForest1 = false;
            onForest2 = false;
            onMountain = false;
            inSafe = false;
        }
        else if (other.CompareTag("Forest1"))
        {
            onBeach = false;
            onForest1 = true;
            onForest2 = false;
            onMountain = false;
            inSafe = false;
        }
        else if (other.CompareTag("Forest2"))
        {
            onBeach = false;
            onForest1 = false;
            onForest2 = true;
            onMountain = false;
            inSafe = false;
        }
        else if (other.CompareTag("Mountain"))
        {
            onBeach = false;
            onForest1 = false;
            onForest2 = false;
            onMountain = true;
            inSafe = false;
        }
        else if (other.CompareTag("Safe"))
        {
            onBeach = false;
            onForest1 = false;
            onForest2 = false;
            onMountain = false;
            inSafe = true;
        }
        else
        {
            onBeach = false;
            onForest1 = false;
            onForest2 = false;
            onMountain = false;
            inSafe = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            StartCoroutine(StartBoss());
        }
        if (other.gameObject.CompareTag("Shrine") && !healedOnce)
        {
            StartCoroutine(HealParty());
        }
    }
}
