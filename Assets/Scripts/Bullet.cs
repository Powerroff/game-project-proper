using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovingObject
{
    // Start is called before the first frame update
    public int damage;
    public int lifespan;
    public int xDir;
    public int yDir;
    public Sprite default_bullet;
    public Sprite power_bullet;
    

    protected override bool MoveThrough(Transform T) {
        if (T == null)
            return true;
        //Debug.Log("Moving through: " + T.tag);
        switch (T.tag) {
            case "RiftEnemy":
                RiftSnake re = T.GetComponent<RiftSnake>();
                re.TakeDamage(damage);
                Destroy(gameObject);
                return false;
            case "Enemy":
                Enemy e = T.GetComponent<Enemy>();
                e.TakeDamage(damage);
                Destroy(gameObject);
                return false;

            case "Underbrush":
                return true;
            case "Impassable":
                return true;
            case "Fog":
                return true;


            default:
                return true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!moving) {
            AttemptMove(xDir, yDir);
            lifespan--;
            if (lifespan == 0)
                Destroy(gameObject);
        }
    }
}
