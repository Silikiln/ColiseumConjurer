using UnityEngine;
using System.Collections;

public class MovingEntity : Entity {
    [SerializeField]
    protected float BaseSpeed = 1;

    [SerializeField]
    protected float BaseMaxSpeed = 1.5f;

    protected virtual float MoveSpeedMultiplier { get { return 1; } }
    protected virtual float MoveSpeedAdded { get { return 0; } }

    protected virtual float MaxSpeedMultiplier { get { return 1; } }
    protected virtual float MaxSpeedAdded { get { return 0; } }

    public float MoveSpeed { get { return BaseSpeed * MoveSpeedMultiplier + MoveSpeedAdded; } }
    public float MaxSpeed { get { return BaseMaxSpeed * MaxSpeedMultiplier + MaxSpeedAdded; } }

    protected new Rigidbody2D rigidbody;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();

        Debug.Log(string.Format("Speed: {0}\nMax Speed: {1}", MoveSpeed, MaxSpeed));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
