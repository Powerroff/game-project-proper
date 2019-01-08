using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rift : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxEnemies;
    public int spawnPreventionSq;
    public float lastUpdate;
    public GameObject riftEnemy;
    public GameObject party;
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
        if (party == null)
            party = GameObject.FindGameObjectWithTag("Player");
        if (Time.time - lastUpdate > 20) {
            lastUpdate = Time.time;
            bool playerNearby = ((transform.position - party.transform.position).sqrMagnitude <= spawnPreventionSq);
            int riftEnemiesNearby = transform.childCount;
            if (!playerNearby)
                SpawnEnemies(maxEnemies - riftEnemiesNearby);
        }
    }
}
