using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MovingObject {


    public Text staminaText;
    public Text healthText;
    public GameObject bullet;
    public int[] inventory;
    public Deck[] decks;
    public int current_deck;
    private int stamina;
    private BoardManager boardManager;
    private bool begin;
    // private Animator animator;

    private int health;
    //other stats probably
    private enum inv_items { Scrap };



    //Initialization ----------------------------
    protected override void Start() {
        //animator = GetComponent<Animator>();
        InitUI();
        boardManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardManager>();
        Debug.Log(boardManager.ToString());
        inventory = new int[10];
        base.Start();
        clearFog();
        boardManager.loadChunks(transform.position);
        begin = false; //TODO do this better
        boardManager.GetComponent<BulkLoader>().performingFirstLoad = 1;
    }

    private void InitUI() {

        {
            staminaText = GameObject.Find("StaminaText").GetComponent<Text>();
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
            updateStamina(100);
            updateHealth(100);
        } //Init health, stamina

        {
            decks = new Deck[2];
            decks[0] = new Deck(GameObject.Find("LeftCard_Panel"));
            decks[1] = new Deck(GameObject.Find("RightCard_Panel"));

            decks[0].InitBasicDeck();
            decks[1].InitBasicDeck();

            current_deck = 0;
        } //Init decks
    }
    //-------------------------------------------


    //Variable update methods---------------------
    private void updateStamina(int newStamina) {
        stamina = newStamina;
        staminaText.text = "Stamina: " + stamina;
    }

    private void updateHealth(int newHealth) {
        health = newHealth;
        healthText.text = "Health: " + health;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        healthText.text = "Health: " + health;
    }
    //--------------------------------------------


    //Frame update methods -----------------------
    void Update() {
        if (!begin) {
            if (boardManager.GetComponent<BulkLoader>().performingFirstLoad < 2)
                return;
            else {
                begin = true;
                GetComponentInChildren<Camera>().enabled = true;
            }
        }
        if (Time.timeScale < 0.75f)
            return;
        EvaluateMovementInput();
        EvaluateShootingInput();
    }

    void EvaluateMovementInput() {
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0) {
            if (!moving) {
                AttemptMove(horizontal, vertical);

                clearFog();
                boardManager.loadChunks(transform.position);
            }
        }
    }

    void EvaluateShootingInput() {
        int shootingXDir = 0;
        int shootingYDir = 0;

        shootingXDir = (int)Input.GetAxisRaw("Fire Horizontal");
        shootingYDir = (int)Input.GetAxisRaw("Fire Vertical");
        Deck deck = decks[current_deck];
        if (shootingXDir != 0 || shootingYDir != 0) {
            if (!deck.IsReloading()) {
                deck.SetReloading(true);
                StartCoroutine(deck.flash());
                Deck.Card c = deck.Pop();
                deck.UpdatePanel();
                Bullet instance = Instantiate(bullet, transform.position, Quaternion.FromToRotation(new Vector3(1, 0), new Vector3(shootingXDir, shootingYDir))).GetComponent<Bullet>();
                instance.damage = c.damage;
                if (instance.damage > 15)
                    instance.GetComponent<SpriteRenderer>().sprite = instance.power_bullet;
                instance.xDir = shootingXDir;
                instance.yDir = shootingYDir;
                instance.lifespan = 7;
                StartCoroutine(deck.Reload(c.reload));
                if (Input.GetButton("Deck Cycle"))
                    StartCoroutine(CycleDecks(0.5f));

            }
        }
    }

    IEnumerator CycleDecks(float cycleTime) {
        yield return new WaitForSeconds(cycleTime);
        current_deck = (current_deck + 1) % decks.Length;
    }
    //    --Movement--     -----------------------
    void clearFog() {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 4);
        foreach (Collider2D collider in nearby) {
            GameObject gameObject = collider.gameObject;
            if (gameObject.tag == "Fog") {
                Destroy(gameObject);
            }
        }
    }

    protected override bool MoveThrough(Transform T) {

        if ((transform.position - T.position).sqrMagnitude < .25)
            return true; //Move off of any object you accidentally overlapped

        //Debug.Log(T.name);
        if (T == null) {
            return true;
        } else if (stamina <= 0) {
            return false;
        }

        //Debug.Log(T.name);
        switch (T.tag) {
            case "Underbrush":
                Underbrush hitUnderbrush = T.GetComponent<Underbrush>() as Underbrush;
                hitUnderbrush.cutBrush();

                updateStamina(stamina - 1);
                //animator.setTrigger("playerChop");

                return true;
            case "RiftEnemy":
            case "Enemy":
                //Enemy enemy = T.GetComponent<Enemy>() as Enemy;
                //enemy.TakeDamage(strength);
                return false;

            case "Impassable":
                return false;

            default:
                return true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.tag) {
            case "Base":
                updateStamina(100);
                updateHealth(100);
                return;
            case "Item":
                Destroy(collision.gameObject);
                return;
            case "Scrap":
                inventory[(int)inv_items.Scrap]++;
                Destroy(collision.gameObject);
                return;
            default:
                return;

        }
    }
    //---------------------------------------------

}
