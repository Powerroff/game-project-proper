using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int damage;
    private int health;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        health = 10;
	}
	
	// Update is called once per frame
	void Update () {
        if (!moving) {
            int horiz = Random.Range(-1, 2);
            int vert = Random.Range(-1, 2);
            AttemptMove(horiz, vert);
        }
    }


    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }


    protected override bool MoveThrough(Transform T) {
        //Debug.Log(T.name);
        if (T == null) {
            return true;
        }


        switch (T.tag) {
            case "Underbrush":
                return true;

            case "Enemy":
                return false;

            case "Player":
                Party party = T.GetComponent<Party>() as Party;
                party.TakeDamage(damage);
                return false;

            case "Base":
                return false;

            default:
                return true;

        }


    }
}
