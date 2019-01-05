using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MovingObject {

    
    public Text staminaText;
    public Text healthText;
    public Text strengthText;
    public GameObject bullet;
    public int[] inventory;
    private int stamina;
    private bool shooting;
    // private Animator animator;

    private int health;
    private int strength; //damage?
    //other stats probably
    private enum inv_items { Scrap };


    protected override bool MoveThrough(Transform T) {
        
        //Debug.Log(T.name);
        if (T == null) {
            return true;
        }  else if (stamina <= 0) {
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
                strength += 10;
                Destroy(collision.gameObject);
                strengthText.text = "Strength: " + strength;
                return;
            case "Scrap":
                inventory[(int)inv_items.Scrap]++;
                Destroy(collision.gameObject);
                return;
            default:
                return;

        }
    }

    void clearFog() {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 4);
        foreach (Collider2D collider in nearby) {
            GameObject gameObject = collider.gameObject;
            if (gameObject.tag == "Fog") {
                Destroy(gameObject);
            }
            if (gameObject.tag == "RiftSnake") {
                RiftSnake rs = gameObject.GetComponent<RiftSnake>();
                rs.seePlayer((Vector2)transform.position);
            }
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        healthText.text = "Health: " + health;
    }

    // Use this for initialization
    protected override void Start () {
        //animator = GetComponent<Animator>();
        staminaText = GameObject.Find("StaminaText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        strengthText = GameObject.Find("StrengthText").GetComponent<Text>();
        updateStamina(100);
        updateHealth(100);
        strength = 10;
        shooting = false;
        strengthText.text = "Strength: " + strength;
        inventory = new int[10];
        base.Start();
        clearFog();
	}


    private void updateStamina(int newStamina) {
        stamina = newStamina;
        staminaText.text = "Stamina: " + stamina;
    }

    private void updateHealth(int newHealth) {
        health = newHealth;
        healthText.text = "Health: " + health;
    }

    private IEnumerator Reload(float seconds) {
        yield return new WaitForSeconds(seconds);
        shooting = false;
    }

    // Update is called once per frame
    void Update () {
        if (Time.timeScale < 0.75f)
            return;
        int horizontal = 0;
        int vertical = 0;
        int shootingXDir = 0;
        int shootingYDir = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        shootingXDir = (int)Input.GetAxisRaw("Fire Horizontal");
        shootingYDir = (int)Input.GetAxisRaw("Fire Vertical");
        if (shootingXDir != 0 || shootingYDir != 0) {
            if (!shooting) {
                shooting = true;
                Bullet instance = Instantiate(bullet, transform.position, Quaternion.FromToRotation(new Vector3(1,0),new Vector3(shootingXDir, shootingYDir))).GetComponent<Bullet>();
                instance.damage = strength;
                instance.xDir = shootingXDir;
                instance.yDir = shootingYDir;
                instance.lifespan = 7;
                StartCoroutine(Reload(.25f));
            }
        }

        //Debug.Log(stamina);

        //if (horizontal != 0) vertical = 0;
        if (horizontal != 0 || vertical != 0)
            if (!moving) {
                clearFog();
                //Debug.Log("Cleared Fog");
                AttemptMove(horizontal, vertical);
            }
	}
}
