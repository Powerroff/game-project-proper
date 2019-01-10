using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underbrush : MonoBehaviour {


    //public Sprite dmgSprite;
    //public int hp = 4;

    public Sprite basicUnderbrush;
    public Sprite gemsUnderbrush;
    public GameObject gem;

    private SpriteRenderer spriteRenderer;


    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void cutBrush (float gemProb) {
        //spriteRenderer.sprite = dmgSprite;
        if (spriteRenderer.sprite == gemsUnderbrush) {
            if (Random.value < gemProb) {
                Instantiate(gem, transform.position, Quaternion.identity, transform.parent);
            }
        }
        Destroy(gameObject);
    }
}
