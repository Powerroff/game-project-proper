using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    //jank
    protected bool moving;

    public float moveTime; // Move time in seconds
    public LayerMask blockingLayer;

    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;
    protected float inverseMoveTime;


	// Use this for initialization
	protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        moving = false;
	}
	
    protected virtual IEnumerator SmoothMovement (Vector3 end) {
        moving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
            
        }
        moving = false;
    }

    protected virtual bool PrepareMove (Vector2 start, Vector2 end) {

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (MoveThrough(hit.transform)) {
            if (hit.transform != null) {
                hit.transform.GetComponent<BoxCollider2D>().enabled = false;
                bool can_move_temp = PrepareMove(start, end); //disable the box collider we can move through and retry
                hit.transform.GetComponent<BoxCollider2D>().enabled = true;
                return can_move_temp;
            } else {
                //StartCoroutine(SmoothMovement(end));
                return true;
            }
        } else return false;
    }

    protected virtual bool AttemptMove (int xDir, int yDir) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        if (PrepareMove(start, end)) {
            StartCoroutine(SmoothMovement(end));
            return true;
        } else return false;

    }
    

    protected abstract bool MoveThrough(Transform T);
    


	// Update is called once per frame
	void Update () {
		
	}
}
