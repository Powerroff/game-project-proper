using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MovingObject {

    public Text staminaText;
    private int stamina;
    // private Animator animator;

    private int health;
    private int strength; //damage?
    //other stats probably



    protected override void onCantMove(Transform T) {
        if (stamina <= 0)
            return;
        Debug.Log(T.name);

        if (T.name == "Underbrush(Clone)") {

            Underbrush hitUnderbrush = T.GetComponent<Underbrush>() as Underbrush;
            hitUnderbrush.cutBrush();

            updateStamina(stamina - 1);
            //animator.setTrigger("playerChop");

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




    protected override void AttemptMove(int xDir, int yDir) {
        

        base.AttemptMove(xDir, yDir);

        RaycastHit2D hit;
    }
    // Use this for initialization
    protected override void Start () {
        //animator = GetComponent<Animator>();
        staminaText = GameObject.Find("StaminaText").GetComponent<Text>();
        updateStamina(100);
        base.Start();
        clearFog();
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Base") {
            updateStamina(100);
        }
    }

    private void updateStamina(int newStamina) {
        stamina = newStamina;
        staminaText.text = "Stamina: " + stamina;
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
            if (!moving)
                //clearFog();
                AttemptMove(horizontal, vertical);
	}
}
