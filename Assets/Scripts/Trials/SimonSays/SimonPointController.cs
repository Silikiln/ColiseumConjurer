using UnityEngine;
using System.Collections;

public class SimonPointController : MonoBehaviour {
    private int ID;
    private Color baseColor;
    private Color shadedColor;

    static float ShadePercent = .35f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(int id, Color color)
    {
        ID = id;
        baseColor = color;
        shadedColor = color;
        shadedColor.r *= ShadePercent;
        shadedColor.g *= ShadePercent;
        shadedColor.b *= ShadePercent;
        UseShadedColor(true);
    }

    public void UseShadedColor(bool use) { GetComponent<SpriteRenderer>().color = use ? shadedColor : baseColor; }

    void OnTriggerEnter2D(Collider2D coll)
    {
        ((SimonSays)TrialHandler.CurrentTrial).PointReached(ID);
        UseShadedColor(false);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        UseShadedColor(true);
    }
}
