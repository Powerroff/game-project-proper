﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    //jank
    protected bool moving;

    public float moveTime; // Move time in seconds
    public float movePauseTime;
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
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > 0.001f) { //was Float.Epsilon, this caused problems. This still may be a bug in the future!
            if (Time.timeScale > .75f) {
                Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
                rb2D.MovePosition(newPosition);
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            }
            yield return null;
            
        }
        rb2D.MovePosition(end);
        yield return EndMovement();
    }

    protected virtual IEnumerator EndMovement() {
        if (movePauseTime > 0) yield return new WaitForSeconds(movePauseTime);
        moving = false;
    }

    protected virtual bool PrepareMove (Vector2 start, Vector2 end) {
        
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, blockingLayer);

        bool can_move = true;
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.Equals(transform))
                continue;
            else
                can_move = (can_move && MoveThrough(hit.transform));
        }
        return can_move;
        
    }

    protected virtual bool AttemptMove (float xDir, float yDir) {
        if (moving) return false;
        moving = true;
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        if (PrepareMove(start, end)) {
            StartCoroutine(SmoothMovement(end));
            return true;
        } else {
            StartCoroutine(EndMovement());
            return false;
        }

    }
    

    protected abstract bool MoveThrough(Transform T);
    


	// Update is called once per frame
	void Update () {
		
	}
}
