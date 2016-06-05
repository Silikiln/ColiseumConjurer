using UnityEngine;
using System.Collections;

public class MovingEntity : Entity {
    [SerializeField]
    private float BaseSpeed = 1;

    [SerializeField]
    private float BaseMaxSpeed = 1.5f;

    public float MoveSpeedModifier = 1;
    public float MaxSpeedModifier = 1;

    public float MoveSpeed { get { return BaseSpeed * MoveSpeedModifier; } }

    public float MaxSpeed { get { return BaseMaxSpeed * MaxSpeedModifier; } }

    protected new Rigidbody2D rigidbody;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
