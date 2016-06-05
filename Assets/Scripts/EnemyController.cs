using UnityEngine;
using System.Collections;

public class EnemyController : MovingEntity {
    public float knockbackPower = 5;

    public Transform target;
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 moveDirection = target.position - transform.position;
        rigidbody.AddForce(moveDirection.normalized * MoveSpeed);
        if (rigidbody.velocity.magnitude > MaxSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer != 8) return;

        GameObject player = coll.gameObject;
        player.SendMessage("Hurt", Damage, SendMessageOptions.DontRequireReceiver);
        Rigidbody2D otherRigid = player.GetComponent<Rigidbody2D>();
        otherRigid.velocity = Vector2.zero;
        otherRigid.AddForce(((Vector2)(player.transform.position - transform.position)).normalized * knockbackPower);
    }
}
