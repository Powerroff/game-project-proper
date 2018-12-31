using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underbrush : MonoBehaviour {


    public Sprite dmgSprite;
    //public int hp = 4;

    private SpriteRenderer spriteRenderer;


    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void cutBrush () {
        spriteRenderer.sprite = dmgSprite;
        Destroy(gameObject);
    }
}
