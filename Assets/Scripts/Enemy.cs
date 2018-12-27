using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {


	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (!moving) {
            int horiz = Random.Range(-1, 1);
            int vert = Random.Range(-1, 1);
            AttemptMove(horiz, vert);
        }
    }

    protected override bool Move(int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        } else return false;


    }


    protected override void onCantMove(Transform T) {
        Debug.Log(T.name);

        if (T.name == "Underbrush(Clone)") {

            //Underbrush hitUnderbrush = T.GetComponent<Underbrush>() as Underbrush;
            //hitUnderbrush.cutBrush();

            //updateStamina(stamina - 1);
            //animator.setTrigger("playerChop");

        }


    }
}
