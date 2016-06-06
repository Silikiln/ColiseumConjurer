using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        ((PointMove)TrialHandler.main.currentTrial).PointReached();
        Destroy(gameObject);
    }
}
