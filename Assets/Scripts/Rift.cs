using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rift : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxEnemies;
    public float lastUpdate;
    public GameObject riftEnemy;
    void Start()
    {
        SpawnEnemies(maxEnemies);
        lastUpdate = Time.time;
    }

    void SpawnEnemies(int numEnemies) {
        while (numEnemies > 0) {
            int i = Random.Range(-5, 5);
            int j = Random.Range(-5, 5);
            Vector2 pos = new Vector2(transform.position.x + i, transform.position.y + j);
            GameObject ins = Instantiate(riftEnemy, pos, Quaternion.identity);
            ins.transform.SetParent(transform);
            numEnemies--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastUpdate > 5) {
            lastUpdate = Time.time;
            bool playerNearby = false;
            int riftEnemiesNearby = 0;
            Collider2D[] nearby = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 10);
            foreach (Collider2D collider in nearby) {
                if (collider.tag == "Player") {
                    playerNearby = true;
                }
                if (collider.tag == "RiftEnemy") {
                    riftEnemiesNearby++;
                    }
            }
            if (!playerNearby)
                SpawnEnemies(maxEnemies - riftEnemiesNearby);
        }
    }
}
