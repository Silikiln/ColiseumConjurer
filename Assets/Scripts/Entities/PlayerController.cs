using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MovingEntity {
    public static PlayerController Instance { get; private set; }

    #region Material Modifiers

    static PlayerController()
    {
        PlayerDamageDealtMultiplier = 1;

        PlayerDamageRecievedMultiplier = 1;
        PlayerHealMultiplier = 1;
    }

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

    #endregion

    public GameObject fireballPrefab;
    public float projectileSpeed = 500;

    GameObject grabbedObject;
    Rigidbody2D grabbedRigidbody;

    override protected void Start()
    {
        base.Start();
        Instance = this;
    }

    void Update()
    {
        FaceTowardsMouse();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            Attack();
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
        fireball.GetComponent<FireballController>().parent = GetComponent<Entity>();
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
}
