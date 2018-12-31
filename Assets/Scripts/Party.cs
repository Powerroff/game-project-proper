using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MovingObject {

    public Text staminaText;
    public Text healthText;
    public Text strengthText;
    private int stamina;
    // private Animator animator;

    private int health;
    private int strength; //damage?
    //other stats probably



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

            case "Enemy":
                //Debug.Log("------");
                //Debug.Log(T.name);
                Enemy enemy = T.GetComponent<Enemy>() as Enemy;
                //Debug.Log(enemy.name);
                enemy.TakeDamage(strength);
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
        strengthText.text = "Strength: " + strength;
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

    // Update is called once per frame
    void Update () {

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

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
