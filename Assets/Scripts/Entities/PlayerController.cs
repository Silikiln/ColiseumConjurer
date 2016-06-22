using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MovingEntity {
    public static PlayerController Instance { get; private set; }

    #region Material Modifiers

    public static float CurrentHealthPercent = 1;
    public static int Lives = 1;

    public static float PlayerAttackSpeedMultiplier = 1;
    public static float PlayerAttackSpeedAdded = 0;

    public static float PlayerHealthMultiplier = 1;
    public static float PlayerHealthAdded = 0;

    public static float PlayerDamageDealtMultiplier = 1;
    public static float PlayerDamageDealtAdded = 0;

    public static float PlayerDamageRecievedMultiplier = 1;
    public static float PlayerDamageReceivedAdded = 0;

    public static float PlayerHealMultiplier = 1;
    public static float PlayerHealAdded = 0;

    public static float PlayerMoveSpeedMultiplier = 1;
    public static float PlayerMoveSpeedAdded = 0;

    public static float PlayerMaxSpeedMultiplier = 1;
    public static float PlayerMaxSpeedAdded = 0;

    public static float PlayerSizeMultiplier = 1;
    public static float PlayerSizeAdded = 0;

    // Player Controller Properties

    protected override float HealthMultiplier { get { return PlayerHealthMultiplier; } }
    protected override float HealthAdded { get { return PlayerHealthAdded; } }

    protected override float DamageDealtMultiplier { get { return PlayerDamageDealtMultiplier; } }
    protected override float DamageDealtAdded { get { return PlayerDamageDealtAdded; } }

    protected override float AttackSpeedMultiplier { get { return PlayerAttackSpeedMultiplier; } }
    protected override float AttackSpeedAdded { get { return PlayerAttackSpeedAdded; } }

    protected override float DamageRecievedMultiplier { get { return PlayerDamageRecievedMultiplier; } }
    protected override float DamageRecievedAdded { get { return PlayerDamageReceivedAdded; } }

    protected override float HealMultiplier { get { return PlayerHealMultiplier; } }
    protected override float HealAdded { get { return PlayerHealAdded; } }

    protected override float SizeMultiplier { get { return PlayerSizeMultiplier; } }
    protected override float SizeAdded { get { return PlayerSizeAdded; } }

    protected override float MoveSpeedMultiplier { get { return PlayerMoveSpeedMultiplier; } }
    protected override float MoveSpeedAdded { get { return PlayerMoveSpeedAdded; } }

    protected override float MaxSpeedMultiplier { get { return PlayerMaxSpeedMultiplier; } }
    protected override float MaxSpeedAdded { get { return PlayerMaxSpeedAdded; } }

    #endregion

    public GameObject fireballPrefab;
    public float projectileSpeed = 500;

    GameObject grabbedObject;
    Rigidbody2D grabbedRigidbody;
    float attackTimer = 0;

    override protected void Start()
    {
        base.Start();
        Instance = this;
        Health = (int)(MaxHealth * CurrentHealthPercent);
    }

    void Update()
    {
        if ((attackTimer -= Time.deltaTime) < 0)
            attackTimer = 0;

        FaceTowardsMouse();
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && attackTimer == 0)
        {
            attackTimer = AttackSpeed;
            Attack();
        }
        else if (Input.GetMouseButtonUp(1))
            Drop();
    }
	
	void FixedUpdate () {
        Movement();
        if (Input.GetMouseButtonDown(1))
            Grab();
    }

    void FaceTowardsMouse()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(position.y, position.x);
        transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    void Grab()
    {
        IEnumerable<Grabbable> sortedObjtecs = FindObjectsOfType<Grabbable>().OrderBy(obj => obj.grabDistance);
        Grabbable toGrab = null;

        float distance, minDistance = 0;
        foreach (Grabbable grab in sortedObjtecs)
        {
            distance = Vector2.Distance(grab.gameObject.transform.position, transform.position);
            if (distance <= grab.grabDistance && (!toGrab || distance < minDistance))
            {
                toGrab = grab;
                minDistance = distance;
            }
        }

        if (!toGrab) return;

        grabbedObject = toGrab.gameObject;
        toGrab.Grabbed();
        grabbedRigidbody = grabbedObject.GetComponent<Rigidbody2D>();

        if (toGrab.isStatic)
            rigidbody.velocity = Vector2.zero;

        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
        joint.enableCollision = true;
        joint.connectedBody = grabbedRigidbody;
        joint.connectedAnchor = grabbedObject.transform.position - transform.position;
    }

    void Drop()
    {
        if (grabbedObject == null) return;

        Destroy(gameObject.GetComponent<Joint2D>());
        grabbedObject.GetComponent<Grabbable>().Dropped();
        grabbedObject = null;
    }

    void Attack()
    {
        GameObject fireball = (GameObject)Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fireball.transform.parent = TrialHandler.Instance.loadOnThis.transform;
        fireball.GetComponent<FireballController>().parent = this;
        float angle = transform.localRotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 aim = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        fireball.GetComponent<Rigidbody2D>().AddForce(aim.normalized * projectileSpeed);
        Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    void Movement()
    {
        Vector2 moveDirection = new Vector2();
        if (Input.GetKey(KeyCode.W))
            moveDirection.y += 1;
        if (Input.GetKey(KeyCode.S))
            moveDirection.y -= 1;
        if (Input.GetKey(KeyCode.D))
            moveDirection.x += 1;
        if (Input.GetKey(KeyCode.A))
            moveDirection.x -= 1;

        /* Slow movement by drag increase when not moving */
        rigidbody.drag = moveDirection == Vector2.zero ? 8 : 3;

        moveDirection.Normalize();
        moveDirection *= MoveSpeed;

        rigidbody.AddForce(moveDirection);
        if (rigidbody.velocity.magnitude > MaxSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
    }

    void OnDestroy()
    {
        Instance = null;
        if (Health <= 0)
            TrialHandler.Instance.PlayerKilled();
        else
            CurrentHealthPercent = HealthPercent;
    }
}
