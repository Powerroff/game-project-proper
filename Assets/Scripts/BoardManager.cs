using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;

    public GameObject underbrush;
    public GameObject gemUnderbrush;
    public GameObject party;
    public GameObject partyBase;
    public GameObject POI;
    public GameObject fog;
    public GameObject lake;
    public GameObject[] equipment;
    public GameObject enemy;
    public GameObject pauseMenu;
    public GameObject rift;
    //public GameObject riftEnemy;
    
    public Transform boardHolder;
    public List<Chunk> chunks;

    public void loadChunks(Vector3 pos) { //TODO tomorrow write a better data structure to store chunks lolol. Or just speed this up.
        for (int i = -5; i <= 5; i++)
            for (int j = -5; j <= 5; j++) {
                Chunk c = findOrCreateChunk(pos + new Vector3(Chunk.chunkSize * i, Chunk.chunkSize * j));
                if (Mathf.Abs(i) == 5 || Mathf.Abs(j) == 5) {
                    c.setActive(false);
                } else if (Mathf.Abs(i) <= 3 && Mathf.Abs(j) <= 3) {
                    c.setActive(true);
                }
            }
    }

    public Chunk findOrCreateChunk(Vector3 pos) {
        Vector3Int anchor = Chunk.FindAnchor(pos);
        foreach (Chunk chunk in chunks)
            if (chunk.anchor.Equals(anchor))
                return chunk;
        //Debug.Log("Could not find chunk at "+ anchor.ToString());
        Chunk c = new Chunk(anchor, this);
        chunks.Add(c);
        return c;
    }

    public void updateChunkOfObject(GameObject obj) {
        Vector3 pos = obj.transform.position;
        foreach(Chunk c in chunks) { //LOL
            c.entities.Remove(obj);
        }
        Chunk newChunk = findOrCreateChunk(pos);
        newChunk.entities.Add(obj);
    }

    public void boardSetup() {
        

        int chunkSize = Chunk.chunkSize;
        Chunk.Chunktype[,] metaMap = new Chunk.Chunktype[5, 5];
        chunks = new List<Chunk>();


        metaMap[2, 2] = Chunk.Chunktype.Base;
        Chunk c = new Chunk(new Vector3Int(0, 0, 0), Chunk.Chunktype.Base, this);
        chunks.Add(c);
        c.initChunk();

        int riftsToPlace = 1;
        int lakesToPlace = 3;
        int itemsToPlace = 15;
        int gemsToPlace = 4;
        while (riftsToPlace + lakesToPlace + itemsToPlace > 0) {
            int i = Random.Range(0, 5);
            int j = Random.Range(0, 5);
            if (metaMap[i, j] == Chunk.Chunktype.Normal) {
                if (riftsToPlace > 0) {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > 2) {
                        metaMap[i, j] = Chunk.Chunktype.Rift;
                        chunks.Add(new Chunk(new Vector3Int(chunkSize * (i - 2), chunkSize * (j - 2), 0), Chunk.Chunktype.Rift, this));
                        riftsToPlace--;
                    }
                } else if (lakesToPlace > 0) {
                    metaMap[i, j] = Chunk.Chunktype.Lake;
                    chunks.Add(new Chunk(new Vector3Int(chunkSize * (i - 2), chunkSize * (j - 2), 0), Chunk.Chunktype.Lake, this));
                    lakesToPlace--;
                } else if (itemsToPlace > 0) {
                    metaMap[i, j] = Chunk.Chunktype.Item;
                    chunks.Add(c = new Chunk(new Vector3Int(chunkSize * (i - 2), chunkSize * (j - 2), 0), Chunk.Chunktype.Item, this));
                    if (Random.value < (float)gemsToPlace / itemsToPlace) {
                        c.isGemChunk = true;
                        gemsToPlace--;
                    }
                    itemsToPlace--;
                }


            }
        }
    }



        


	// Use this for initialization
	void Awake () {
        boardHolder = transform;
        pauseMenu = GameObject.Find("PauseMenu");
        gameObject.GetComponent<BulkLoader>().Setup(); //TODO: Figure this out, order of Awakes, etc.
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver( bool win) {
        GetComponent<GameManager>().GameOver(win);
    }
}
