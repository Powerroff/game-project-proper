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
    private bool isCyclingDecks;
    // private Animator animator;

    private int health;
    //other stats probably
    private enum inv_items { Scrap, Gems };
    public Equipment[] equipment1;
    public Equipment[] equipment2;
    equipWrapper[][] equipment;
    public Card basicCard;

    struct equipWrapper {
        public Equipment.Slot slot;
        public Card card;

        public equipWrapper(Equipment e) { //Get rid of this struct and everything that references it, eventually
            if (e == null) {
                slot = 0;
                card = null;
            } else {
                slot = e.slot;
                card = e.card;
            }
        }
    }

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
        isCyclingDecks = false;
    }
    

    private void InitUI() {

        {
            staminaText = GameObject.Find("StaminaText").GetComponent<Text>();
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
            updateStamina(100);
            updateHealth(100);
        } //Init health, stamina

        {

            equipment = new equipWrapper[2][]; //Code for testing, to be changed later.
            equipment[0] = new equipWrapper[equipment1.Length];
            equipment[1] = new equipWrapper[equipment2.Length];
            for (int i = 0; i < equipment2.Length; i++) {
                equipment[0][i] = new equipWrapper(equipment1[i]);
                equipment[1][i] = new equipWrapper(equipment2[i]);
            }

            decks = new Deck[2];
            decks[0] = new Deck(GameObject.Find("LeftCard_Panel"));
            decks[1] = new Deck(GameObject.Find("RightCard_Panel"));

            updateDecks();

            current_deck = 0;
        } //Init equipment, decks
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
        if (health <= 0) {
            boardManager.GameOver(false);
        }
    }

    void updateDecks() {
        foreach (equipWrapper e in equipment[0])
            if (e.card != null)
                Debug.Log(e.card.name);

        for (int deckNum = 0; deckNum < equipment.Length; deckNum++) {
            decks[deckNum] = new Deck(decks[deckNum].panel);
            for (int i = 0; i < Equipment.numSlots; i++) {
                if (equipment[deckNum][i].card == null) {
                    decks[deckNum].Add(basicCard);
                } else
                    decks[deckNum].Add(equipment[deckNum][i].card);
            }
            decks[deckNum].UpdatePanel();
        }
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
        if (Input.GetButtonDown("Deck Cycle"))
            isCyclingDecks = !isCyclingDecks;
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
                Card c = deck.Pop();
                deck.UpdatePanel();
                Bullet instance = Instantiate(bullet, transform.position, Quaternion.FromToRotation(new Vector3(1, 0), new Vector3(shootingXDir, shootingYDir))).GetComponent<Bullet>();
                instance.damage = c.damage;
                if (instance.damage > 15)
                    instance.GetComponent<SpriteRenderer>().sprite = instance.power_bullet;
                instance.xDir = shootingXDir;
                instance.yDir = shootingYDir;
                instance.lifespan = 7;
                StartCoroutine(deck.Reload(c.reload));
                if (isCyclingDecks)
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
                hitUnderbrush.cutBrush(1);

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
                Equipment item = collision.GetComponent<Equipment>();
                equipment[current_deck][(int)item.slot] = new equipWrapper(item);
                updateDecks(); //This resets the decks. Maybe fix this when I redo the deck data structure.
                Destroy(collision.gameObject);
                return;
            case "Scrap":
                inventory[(int)inv_items.Scrap]++;
                Destroy(collision.gameObject);
                return;
            case "Gem":
                inventory[(int)inv_items.Gems]++;
                Destroy(collision.gameObject);
                return;
            case "Rift":
                if (Input.GetButton("Interact")) {
                    if (inventory[(int)inv_items.Gems] >= 1 && inventory[(int)inv_items.Scrap] >= 1) {
                        inventory[(int)inv_items.Gems] -= 1;
                        inventory[(int)inv_items.Scrap] -= 1;
                        collision.GetComponent<Rift>().beginDrilling();
                    }
                }
                return;
            default:
                return;

        }
    }
    //---------------------------------------------

}
