using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private bool activating;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator activate(bool active) {
        if (!activating && active != isActive) {
            isActive = active;
            activating = true;
            int i = 0;
            foreach (Transform child in transform) {
                i++;
                child.gameObject.SetActive(active);
                //Debug.Log("" + child.transform.position.x + "," + child.transform.position.y);
                if (i > 5) {
                    i = 0;
                    yield return null;
                }
            }
            activating = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
