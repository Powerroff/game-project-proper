using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkLoader : MonoBehaviour
{
    struct objToInstantiate {
        public GameObject obj;
        public Vector3 loc;
        public Transform parent;
        public Chunk chunk;

        public objToInstantiate(GameObject obj, Vector3 loc, Transform parent, Chunk chunk) {
            this.obj = obj;
            this.loc = loc;
            this.parent = parent;
            this.chunk = chunk;
        }
    }

    struct objToActivate {
        public GameObject obj;
        public bool active;

        public objToActivate (GameObject obj, bool active) {
            this.obj = obj;
            this.active = active;
        }
    }

    private Queue<objToInstantiate> instantiateQueue;
    private Queue<objToActivate> activateQueue;
    private System.Diagnostics.Stopwatch watch;
    public int performingFirstLoad;

    public void Setup() {
        instantiateQueue = new Queue<objToInstantiate>();
        activateQueue = new Queue<objToActivate>();
        watch = new System.Diagnostics.Stopwatch();
    }

    public void Instantiate(GameObject obj, Vector3 pos, Transform parent, Chunk chunk) {
        instantiateQueue.Enqueue(new objToInstantiate(obj, pos, parent, chunk));
    }
    public void Activate(GameObject obj, bool active) {
        activateQueue.Enqueue(new objToActivate(obj, active));
    }

    void Update() {
        watch.Reset();
        watch.Start();
        int millisecondBudget = 10;
        while (instantiateQueue.Count > 0 || activateQueue.Count > 0) {
            //Debug.Log(watch.ElapsedMilliseconds);
            if (watch.ElapsedMilliseconds > millisecondBudget)
                break;
            else {
                if (instantiateQueue.Count > 0) {
                    objToInstantiate i = instantiateQueue.Dequeue();
                    GameObject instance = Instantiate(i.obj, i.loc, Quaternion.identity, i.parent);
                    if (i.chunk != null)
                        i.chunk.entities.Add(instance);
                }
                if (activateQueue.Count > 0) {
                    objToActivate i = activateQueue.Dequeue();
                    if (i.obj != null)
                        i.obj.SetActive(i.active);
                }
            }
        }
        if (instantiateQueue.Count == 0 && activateQueue.Count == 0)
            if (performingFirstLoad == 1)
                performingFirstLoad = 2;
    }
}
