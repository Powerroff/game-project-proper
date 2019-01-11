using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public List<GameObject> entities;
    public Vector3Int anchor;
    public Chunktype type;
    public bool isInitialized;
    public bool isActive;
    public bool isGemChunk;




    public static readonly int chunkSize = 20;
    public enum Chunktype { Normal, Lake, Item, Base, Rift};

    public Chunk(Vector3Int anchor) {
        entities = new List<GameObject>();
        this.anchor = anchor;
        this.isGemChunk = false;
        type = 0;
        isActive = false;
        isInitialized = false;
    }

    public Chunk(Vector3Int anchor, Chunktype type) {
        entities = new List<GameObject>();
        this.anchor = anchor;
        this.type = type;
        this.isGemChunk = false;
        isActive = false;
        isInitialized = false;
    }

    public void SetActive(bool active) {
        if (active == isActive)
            return;
        else
            isActive = active;

        //Debug.Log("Setting activity of chunk at " + anchor.ToString() + " to " + active);

        if (active && !isInitialized)
            initChunk();
        
        foreach (GameObject obj in entities) {
            if (obj != null)
                //obj.SetActive(active);
                GameManager.gameManager.bulkLoader.Activate(obj, active);
        }
    }

    public static Vector3Int FindAnchor(Vector3 pos) {
        int x = Mathf.FloorToInt(pos.x / chunkSize);
        int y = Mathf.FloorToInt(pos.y / chunkSize);
        return new Vector3Int(x * chunkSize, y * chunkSize, 0);
    }

    public void initChunk() {
        isInitialized = true;
        switch (type) {
            case Chunktype.Normal:
                initNormalChunk();
                break;
            case Chunktype.Base:
                initBaseChunk();
                break;
            case Chunktype.Item: 
                initItemChunk();
                break;
            case Chunktype.Lake:
                initLakeChunk();
                break;
            case Chunktype.Rift:
                initRiftChunk();
                break;

        }
    }

    public GameObject initObjectImmediate(GameObject obj, Vector3 loc) {
        GameObject instance = Object.Instantiate(obj, loc, Quaternion.identity, GameManager.gameManager.transform);
        entities.Add(instance);
        return instance;
    }

    void initObject(GameObject obj, Vector3 loc) {
        GameManager.gameManager.bulkLoader.Instantiate(obj, loc, GameManager.gameManager.transform, this);
    }

    void initObject(GameObject obj, Vector3 loc, bool noChunk) {
        if (noChunk)
            GameManager.gameManager.bulkLoader.Instantiate(obj, loc, GameManager.gameManager.transform, null);
    }

    void initNormalChunk() {
        for (int xPos = (int)anchor.x; xPos < (int)anchor.x + chunkSize; xPos++) {
            for (int yPos = (int)anchor.y; yPos < (int)anchor.y + chunkSize; yPos++) {
                Vector3 loc = new Vector3(xPos, yPos);
                initObject(GameManager.gameManager.boardManager.fog, loc);
                initObject(GameManager.gameManager.boardManager.underbrush, loc);
                if (Random.value > .99)
                    initObject(GameManager.gameManager.boardManager.enemy, loc);
            }
        }
    }

    void initLakeChunk() {
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        for (int xPos = (int)anchor.x; xPos < (int)anchor.x + chunkSize; xPos++) {
            for (int yPos = (int)anchor.y; yPos < (int)anchor.y + chunkSize; yPos++) {
                Vector3 loc = new Vector3(xPos, yPos);
                initObject(GameManager.gameManager.boardManager.fog, loc);
                if ((loc - center).sqrMagnitude <= 25)
                    initObject(GameManager.gameManager.boardManager.lake, loc);
                else if (Random.value > .98) {
                    initObject(GameManager.gameManager.boardManager.lake, loc);
                } else {
                    initObject(GameManager.gameManager.boardManager.underbrush, loc);
                    if (Random.value > .99)
                        initObject(GameManager.gameManager.boardManager.enemy, loc);
                }
 
            }
        }
    }
    
    void initItemChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(GameManager.gameManager.boardManager.equipment[Random.Range(0, GameManager.gameManager.boardManager.equipment.Length)], center);
    }

    void initPOIChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(GameManager.gameManager.boardManager.POI, center);
    }

    void initRiftChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(GameManager.gameManager.boardManager.rift, center);
    }

    void initBaseChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(GameManager.gameManager.boardManager.partyBase, center);
        initObject(GameManager.gameManager.boardManager.party, center, true);
    }

    void initClearingChunk() {
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        for (int xPos = (int)anchor.x; xPos < (int)anchor.x + chunkSize; xPos++) {
            for (int yPos = (int)anchor.y; yPos < (int)anchor.y + chunkSize; yPos++) {
                Vector3 loc = new Vector3(xPos, yPos);
                initObject(GameManager.gameManager.boardManager.fog, loc);
                if ((loc - center).sqrMagnitude > 25) {
                    if (Random.value < .98) {
                        if (isGemChunk && Random.value < .01) {
                            initObject(GameManager.gameManager.boardManager.gemUnderbrush, loc);
                        } else
                            initObject(GameManager.gameManager.boardManager.underbrush, loc);
                    }
                    if (Random.value > .99)
                        initObject(GameManager.gameManager.boardManager.enemy, loc);
                }

            }
        }
    }

}
