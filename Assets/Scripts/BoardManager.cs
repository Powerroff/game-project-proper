using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;

    public GameObject underbrush;
    public GameObject party;
    public GameObject partyBase;
    public GameObject POI;
    public GameObject fog;
    public GameObject lake;
    public GameObject item;
    public GameObject enemy;

    private Transform boardHolder;

    public void boardSetup() {
        //for (int x = 0; x < columns; x++) {
        //    for (int y = 0; y < rows; y++) {
        //        int i = x - columns / 2;
        //        int j = y - rows / 2;

        //        if (i == 0 && j == 0) {
        //            gameobject partyinstance = instantiate(party, new vector3(i, j, 0), quaternion.identity) as gameobject;
        //            partyinstance.transform.setparent(boardholder);
        //            gameobject baseinstance = instantiate(partybase, new vector3(i, j, 0), quaternion.identity) as gameobject;
        //            baseinstance.transform.setparent(boardholder);
        //        } else {
        //            gameobject toinstantiate = underbrush;
        //            gameobject instance = instantiate(toinstantiate, new vector3(i, j, 0f), quaternion.identity) as gameobject;
        //            instance.transform.setparent(boardholder);
        //        }
        //    }
        //}


        int chunksize = 40;

        for (int x = 0; x < columns / chunksize; x++) {
            for (int y = 0; y < rows / chunksize; y++) {
                int i = chunksize * x - columns / 2;
                int j = chunksize * y - columns / 2;

                float POItype = Random.value;
                bool isPOI = (POItype < .5);
                int POINT = (isPOI ? 1 : 0);
                //isPOI = true;

                Vector3 center = new Vector3(i + chunksize / 2, j + chunksize/2, 0);

                for (int xPos = i; xPos < i + chunksize; xPos ++)
                    for (int yPos = j; yPos < j + chunksize; yPos++) {
                        if (xPos == 0 && yPos == 0) {
                            GameObject partyInstance = Instantiate(party, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            partyInstance.transform.SetParent(boardHolder);
                            GameObject baseInstance = Instantiate(partyBase, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            baseInstance.transform.SetParent(boardHolder);
                        } else {
                            Vector3 loc = new Vector3(xPos, yPos, 0);
                            if (((loc - center).sqrMagnitude > 25 || !isPOI) && POINT * Random.value < 0.98) {
                                GameObject instance = Instantiate(underbrush, loc, Quaternion.identity) as GameObject;
                                instance.transform.SetParent(boardHolder);
                            } else if (loc.Equals(center) && POItype < .25) {
                                GameObject instance = Instantiate(POI, loc, Quaternion.identity) as GameObject;
                                instance.transform.SetParent(boardHolder);
                            } else if (isPOI && POItype > .25) {
                                GameObject instance = Instantiate(lake, loc, Quaternion.identity) as GameObject;
                                instance.transform.SetParent(boardHolder);
                            }
                            //GameObject fogInstance = Instantiate(fog, loc, Quaternion.identity) as GameObject;
                            //fogInstance.transform.SetParent(boardHolder);
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
