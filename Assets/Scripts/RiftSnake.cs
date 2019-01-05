using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftSnake : MovingObject {

    public int damage;
    public int health;
    private Vector2 parentLoc;
    public Vector2 playerLoc;
    private float seenPlayerTime;
    public GameObject scrap;
    public Sprite defaultSprite;
    public Sprite damageSprite;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        parentLoc = transform.parent.position;
        seenPlayerTime = float.MinValue;
    }

    // Update is called once per frame
    void Update() {
        if (!moving) {
            if (Time.time - seenPlayerTime < 3)
                MoveToward(playerLoc);
            else if (((Vector2)transform.position - parentLoc).sqrMagnitude > 20)
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

    public void seePlayer(Vector2 position) {
        seenPlayerTime = Time.time;
        playerLoc = position;
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


        switch (T.tag) {
            case "Underbrush":
                return true;

            case "Enemy":
                return false;

            case "RiftSnake":
                return true;

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
