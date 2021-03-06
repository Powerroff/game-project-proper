﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int damage;
    public GameObject scrap;
    public Sprite defaultSprite;
    public Sprite damageSprite;

    private int health;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        health = 20;
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
            OnDestroy();
        }
        StartCoroutine(DamageAnim());
    }

    protected IEnumerator DamageAnim() {
        GetComponent<SpriteRenderer>().sprite = damageSprite;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    protected void OnDestroy() {
        Random.InitState(Time.frameCount+(int)Time.time);
        if (Random.value < .25) {
            Instantiate(scrap, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
   

    protected override bool MoveThrough(Transform T) {
        //Debug.Log(T.name);
        if (T == null) {
            return true;
        }

        if ((transform.position - T.position).sqrMagnitude < .25)
            return true; //Move off of any object you accidentally overlapped


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
