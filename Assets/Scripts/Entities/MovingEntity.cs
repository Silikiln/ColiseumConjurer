using UnityEngine;
using System.Collections;

public class MovingEntity : Entity {
    [SerializeField]
    protected float BaseSpeed = 1;

    [SerializeField]
    protected float BaseMaxSpeed = 1.5f;

    [SerializeField]
    protected float MinMoveSpeed = 1;

    [SerializeField]
    protected float MaxMoveSpeed = 40;

    [SerializeField]
    protected float MinMaxSpeed = 1f;

    [SerializeField]
    protected float MaxMaxSpeed = 30f;

    protected virtual float MoveSpeedMultiplier { get { return 1; } }
    protected virtual float MoveSpeedAdded { get { return 0; } }

    protected virtual float MaxSpeedMultiplier { get { return 1; } }
    protected virtual float MaxSpeedAdded { get { return 0; } }

    public virtual float MoveSpeed { get { return Mathf.Clamp(BaseSpeed * MoveSpeedMultiplier + MoveSpeedAdded, MinMoveSpeed, MaxMoveSpeed); } }
    public virtual float MaxSpeed { get { return Mathf.Clamp(BaseMaxSpeed * MaxSpeedMultiplier + MaxSpeedAdded, MinMaxSpeed, MaxMaxSpeed); } }

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
