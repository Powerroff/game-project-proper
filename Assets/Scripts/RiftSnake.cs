﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftSnake : MovingObject {

    public int damage;
    public int health;
    public int homeRadiusSq;
    public int detectionRadiusSq;
    public int territoryRadiusSq;
    private Vector2 parentLoc;
    public GameObject scrap;
    public GameObject party;
    public Sprite defaultSprite;
    public Sprite damageSprite;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        parentLoc = transform.parent.position;
    }

    // Update is called once per frame
    void Update() {
        if (party == null)
            party = GameObject.FindGameObjectWithTag("Player");
        else if (!moving) {
            if (((Vector2)transform.position - parentLoc).sqrMagnitude > territoryRadiusSq)
                MoveToward(parentLoc);
            else if ((transform.position - party.transform.position).sqrMagnitude < detectionRadiusSq)
                MoveToward(party.transform.position);
            else if (((Vector2)transform.position - parentLoc).sqrMagnitude > homeRadiusSq)
                MoveToward(parentLoc);
            else {
                int horiz = Random.Range(-1, 2);
                int vert = Random.Range(-1, 2);
                AttemptMove(horiz, vert);
            }
        }
    }

    private void MoveToward(Vector2 loc) {

        Vector2 dir = (loc - (Vector2)transform.position);
        if (dir.sqrMagnitude < float.Epsilon)
            return;
        dir.Normalize();
        AttemptMove(Mathf.Round(dir.x), Mathf.Round(dir.y));
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
        Random.InitState(Time.frameCount + (int)Time.time);
        if (Random.value < .25) {
            //Instantiate(scrap, transform.position, Quaternion.identity);
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

            case "RiftSnake":
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

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.tag) {
            case "Rift":
                Rift r = collision.GetComponent<Rift>();
                if (r.drilling)
                    r.takeDamage(20);
                break;
            default:
                return;

        }
    }
}

