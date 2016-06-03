using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float MoveSpeed = 1;
    public float MaxSpeed = 1.5f;

    public GameObject fireballPrefab;
    public float projectileSpeed = 500;

    new Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            GameObject fireball = (GameObject)Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            Vector2 angle = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            fireball.GetComponent<Rigidbody2D>().AddForce(angle.normalized * projectileSpeed);
        }
    }
	
	void FixedUpdate () {
        Movement();
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
