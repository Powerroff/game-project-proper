using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns;
    public int rows;
    public int chunksize = 40;
    public GameObject[,] chunks;

    public GameObject underbrush;
    public GameObject party;
    public GameObject partyBase;
    public GameObject POI;
    public GameObject fog;
    public GameObject lake;
    public GameObject item;
    public GameObject enemy;
    public GameObject chunkLoader;

    private Transform boardHolder;

    public void updateChunk(float x, float y, GameObject obj) {
        //Debug.Log("Updating at x = " + (int)x + ", y = " + (int)y);
        int X = ((int)x + columns / 2 + 5) / chunksize;
        int Y = ((int)y + rows / 2 + 5) / chunksize;
        if (chunks[X, Y] == null) {
            chunks[X,Y] = Instantiate(chunkLoader, Vector2.zero, Quaternion.identity) as GameObject;
            chunks[X,Y].transform.SetParent(boardHolder);
        }
        obj.transform.SetParent(chunks[X,Y].transform);
    }
    public void setActiveChunks(float x, float y) {
        //Debug.Log("setting actives");
        int X = ((int)x + columns / 2 + 5) / chunksize;
        int Y = ((int)y + rows / 2 + 5) / chunksize;
        for (int i = 0; i < columns/chunksize; i++)
            for (int j = 0; j < rows/chunksize; j++) {
                if ((int)Mathf.Abs(X-i) + (int)Mathf.Abs(Y-j) > 2)
                    StartCoroutine(chunks[i,j].GetComponent<ChunkLoader>().activate(false));
                else {
                    if (!chunks[i, j].GetComponent<ChunkLoader>().isActive)
                        Debug.Log("Loading in New Chunks at (" + i + "," + j + ")");
                    StartCoroutine(chunks[i, j].GetComponent<ChunkLoader>().activate(true));
                    
                }
            }
    }


    public void boardSetup() {

        chunks = new GameObject[columns/chunksize + 1, rows/chunksize + 1];


        for (int x = 0; x < columns / chunksize; x++) {
            for (int y = 0; y < rows / chunksize; y++) {
                chunks[x, y] = Instantiate(chunkLoader, Vector2.zero, Quaternion.identity) as GameObject;
                chunks[x, y].transform.SetParent(boardHolder);



                int i = chunksize * x - columns / 2;
                int j = chunksize * y - rows / 2;

                float POItype = Random.value;
                bool isPOI = (POItype < .5);
                int POINT = (isPOI ? 1 : 0);
                //isPOI = true;

                Vector3 center = new Vector3(i + chunksize / 2, j + chunksize/2, 0);

                for (int xPos = i; xPos < i + chunksize; xPos ++)
                    for (int yPos = j; yPos < j + chunksize; yPos++) {
                        Vector3 loc = new Vector3(xPos, yPos, 0);
                        GameObject fogInstance = Instantiate(fog, loc, Quaternion.identity) as GameObject;
                        fogInstance.transform.SetParent(chunks[x, y].transform);

                        if (xPos == 0 && yPos == 0) {
                            GameObject partyInstance = Instantiate(party, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            partyInstance.transform.SetParent(boardHolder);
                            GameObject baseInstance = Instantiate(partyBase, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            baseInstance.transform.SetParent(boardHolder);
                        } else {
                            
                            if (((loc - center).sqrMagnitude > 25 || !isPOI) && POINT * Random.value < 0.98) {
                                GameObject instance = Instantiate(underbrush, loc, Quaternion.identity) as GameObject;
                                instance.transform.SetParent(chunks[x, y].transform);


                                if (Random.value > .99) {
                                    GameObject enemy_instance = Instantiate(enemy, loc, Quaternion.identity) as GameObject;
                                    enemy_instance.transform.SetParent(chunks[x, y].transform);
                                }


                            } else if (loc.Equals(center) && POItype < .1) {
                                GameObject instance = Instantiate(POI, loc, Quaternion.identity) as GameObject;
                                instance.transform.SetParent(chunks[x, y].transform);
                            } else if (isPOI && POItype > .35) {
                                if (loc.Equals(center)) {
                                    GameObject instance = Instantiate(item, loc, Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(chunks[x, y].transform);
                                }
                            } else if (isPOI && POItype >= .1) {
                                GameObject instance = Instantiate(lake, loc, Quaternion.identity) as GameObject;
                                instance.transform.SetParent(chunks[x, y].transform);
                            }
                            
                        }
                    }



            }
        }


        
    }


	// Use this for initialization
	void Start () {
        boardHolder = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
