using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public BoardManager boardManager;
    public BulkLoader bulkLoader;
    public List<GameObject> entities;
    public Vector3Int anchor;
    public Chunktype type;
    public bool isInitialized;
    public bool isActive;




    public static readonly int chunkSize = 20;
    public enum Chunktype { Normal, Lake, Item, Base, Rift};

    public Chunk(Vector3Int anchor, BoardManager boardManager) {
        entities = new List<GameObject>();
        this.anchor = anchor;
        this.boardManager = boardManager;
        this.bulkLoader = boardManager.gameObject.GetComponent<BulkLoader>();
        type = 0;
        isActive = false;
        isInitialized = false;
    }

    public Chunk(Vector3Int anchor, Chunktype type, BoardManager boardManager) {
        entities = new List<GameObject>();
        this.anchor = anchor;
        this.type = type;
        this.boardManager = boardManager;
        this.bulkLoader = boardManager.gameObject.GetComponent<BulkLoader>();
        isActive = false;
        isInitialized = false;
    }

    public void setActive(bool active) {
        if (active == isActive)
            return;
        else
            isActive = active;

        Debug.Log("Setting activity of chunk at " + anchor.ToString() + " to " + active);

        if (active && !isInitialized)
            initChunk();
        
        foreach (GameObject obj in entities) {
            if (obj != null)
                //obj.SetActive(active);
                bulkLoader.Activate(obj, active);
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

//    GameObject initObject(GameObject obj, Vector3 loc) {
//        GameObject instance = Object.Instantiate(obj, loc, Quaternion.identity, boardManager.boardHolder);
//        entities.Add(instance);
//        return instance;
//    }

    void initObject(GameObject obj, Vector3 loc) {
        bulkLoader.Instantiate(obj, loc, boardManager.boardHolder, this);
    }

    void initObject(GameObject obj, Vector3 loc, bool noChunk) {
        if (noChunk)
            bulkLoader.Instantiate(obj, loc, boardManager.boardHolder, null);
    }

    void initNormalChunk() {
        for (int xPos = (int)anchor.x; xPos < (int)anchor.x + chunkSize; xPos++) {
            for (int yPos = (int)anchor.y; yPos < (int)anchor.y + chunkSize; yPos++) {
                Vector3 loc = new Vector3(xPos, yPos);
                initObject(boardManager.fog, loc);
                initObject(boardManager.underbrush, loc);
                if (Random.value > .99)
                    initObject(boardManager.enemy, loc);
            }
        }
    }

    void initLakeChunk() {
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        for (int xPos = (int)anchor.x; xPos < (int)anchor.x + chunkSize; xPos++) {
            for (int yPos = (int)anchor.y; yPos < (int)anchor.y + chunkSize; yPos++) {
                Vector3 loc = new Vector3(xPos, yPos);
                initObject(boardManager.fog, loc);
                if ((loc - center).sqrMagnitude <= 25)
                    initObject(boardManager.lake, loc);
                else if (Random.value > .98) {
                    initObject(boardManager.lake, loc);
                } else {
                    initObject(boardManager.underbrush, loc);
                    if (Random.value > .99)
                        initObject(boardManager.enemy, loc);
                }
 
            }
        }
    }

    void initClearingChunk() {
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        for (int xPos = (int)anchor.x; xPos < (int)anchor.x + chunkSize; xPos++) {
            for (int yPos = (int)anchor.y; yPos < (int)anchor.y + chunkSize; yPos++) {
                Vector3 loc = new Vector3(xPos, yPos);
                initObject(boardManager.fog, loc);
                if ((loc - center).sqrMagnitude > 25) {
                    if (Random.value < .98)
                        initObject(boardManager.underbrush, loc);
                    if (Random.value > .99)
                        initObject(boardManager.enemy, loc);
                }

            }
        }
    }

    void initItemChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(boardManager.item, center);
    }

    void initPOIChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(boardManager.POI, center);
    }

    void initRiftChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(boardManager.rift, center);
    }

    void initBaseChunk() {
        initClearingChunk();
        Vector3 center = new Vector3(anchor.x + chunkSize / 2, anchor.y + chunkSize / 2);
        initObject(boardManager.partyBase, center);
        initObject(boardManager.party, center, true);
    }
        
}
