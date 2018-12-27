using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    //jank
    public bool moving;

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
	
    protected IEnumerator SmoothMovement (Vector3 end) {
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

    protected virtual bool Move (int xDir, int yDir, out RaycastHit2D hit) {
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

    protected virtual void AttemptMove(int xDir, int yDir) { 
        
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        
        if (!canMove && hit.transform != null) {
            onCantMove(hit.transform);
        }
        
    }


    protected abstract void onCantMove(Transform T);
    


	// Update is called once per frame
	void Update () {
		
	}
}
