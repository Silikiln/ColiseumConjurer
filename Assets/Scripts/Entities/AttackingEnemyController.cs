﻿using UnityEngine;
using System.Collections;

public class AttackingEnemyController : EnemyController {
    public float knockbackPower = 5;

    public Transform target;

    void FixedUpdate()
    {
        if (target == null)
        {
            if (PlayerController.Instance != null)
                target = PlayerController.Instance.transform;
            else
                return;
        }

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
