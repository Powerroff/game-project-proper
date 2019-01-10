﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rift : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxEnemies;
    public int spawnPreventionSq;
    public float lastUpdate;
    public GameObject riftEnemy;
    public GameObject party;
    public Sprite drillingSprite;
    Text DrillText;
    Text DrillTimerText;
    public bool drilling;
    float drillStartTime;
    int health;
    void Start()
    {
        SpawnEnemies(maxEnemies);
        lastUpdate = Time.time;
        DrillText = GameObject.Find("DrillHealthText").GetComponent<Text>();
        DrillTimerText = GameObject.Find("DrillTimerText").GetComponent<Text>();
        drilling = false;
        health = 1000;
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
            if (!playerNearby || drilling)
                SpawnEnemies(maxEnemies - riftEnemiesNearby);
        }

        if (drilling) {
            DrillText.text = "Rift Drill Health: " + health;
            DrillTimerText.text = "Drill Time Remaining: " + (int)(60 - (Time.realtimeSinceStartup - drillStartTime));
            if (health <= 0)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardManager>().GameOver(false);
            if (Time.realtimeSinceStartup - drillStartTime > 60)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardManager>().GameOver(true);
        }
    }

    public void takeDamage(int damage) {
        health -= damage;
    }

    public void beginDrilling() {
        if (drilling) return;
        drilling = true;
        drillStartTime = Time.realtimeSinceStartup;
        maxEnemies = 7;
        GetComponent<SpriteRenderer>().sprite = drillingSprite;
    }
}
