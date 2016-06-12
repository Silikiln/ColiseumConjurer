using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MovingEntity {
    public static PlayerController Instance { get; private set; }

    public GameObject fireballPrefab;
    public float projectileSpeed = 500;

    List<GameObject> grabbableItems = new List<GameObject>();
    GameObject grabbedObject;

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
        else if (Input.GetMouseButtonDown(1))
            Grab();
        else if (Input.GetMouseButtonUp(1))
            Drop();
    }
	
	void FixedUpdate () {
        Movement();

        if (grabbedObject != null)
        {
            RelativeJoint2D joint = GetComponent<RelativeJoint2D>();
            /*
            float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 newOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            joint.linearOffset = newOffset * joint.linearOffset.magnitude;
            */
            joint.angularOffset = transform.rotation.eulerAngles.z;
        }
    }

    void FaceTowardsMouse()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(position.y, position.x);
        transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    void Grab()
    {
        if (grabbableItems.Count == 0) return;

        // TODO: Find closest from grabbable
        grabbedObject = grabbableItems[0];
        grabbedObject.transform.parent = transform;
        grabbedObject.GetComponent<Grabbable>().Grabbed();

        RelativeJoint2D joint = gameObject.AddComponent<RelativeJoint2D>();
        joint.connectedBody = grabbedObject.GetComponent<Rigidbody2D>();
        joint.enableCollision = true;
        joint.linearOffset = transform.position - grabbedObject.transform.position;
    }

    void Drop()
    {
        if (!grabbedObject) return;

        Destroy(gameObject.GetComponent<RelativeJoint2D>());
        grabbedObject.GetComponent<Grabbable>().Dropped();
        grabbedObject.transform.parent = null;
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

    public void NearGrabbable(GameObject grabbable) { grabbableItems.Add(grabbable); }
    public void LeavingGrabbable(GameObject grabbable) { grabbableItems.Remove(grabbable); }
}
