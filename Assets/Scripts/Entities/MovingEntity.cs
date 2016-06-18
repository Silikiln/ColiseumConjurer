using UnityEngine;
using System.Collections;

public class MovingEntity : Entity {
    [SerializeField]
    protected float BaseSpeed = 1;

    [SerializeField]
    protected float BaseMaxSpeed = 1.5f;

    public float MoveSpeedModifier = 1;
    public float MoveSpeedAdded = 0;

    public float MaxSpeedModifier = 1;
    public float MaxSpeedAdded = 0;

    public float MoveSpeed { get { return BaseSpeed * MoveSpeedModifier + MoveSpeedAdded; } }
    public float MaxSpeed { get { return BaseMaxSpeed * MaxSpeedModifier + MaxSpeedAdded; } }

    protected new Rigidbody2D rigidbody;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
        MoveSpeedModifier = 1;
        MaxSpeedModifier = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
