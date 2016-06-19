using UnityEngine;
using System.Collections;

public class BlimmyController : EnemyController {
    public float knockbackPower = 16;
    public float enrageThreshold = .5f;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            GameObject player = coll.gameObject;
            player.SendMessage("Hurt", Damage, SendMessageOptions.DontRequireReceiver);
            Rigidbody2D otherRigid = player.GetComponent<Rigidbody2D>();
            otherRigid.velocity = Vector2.zero;
            otherRigid.AddForce(((Vector2)(player.transform.position - transform.position)).normalized * knockbackPower);
        }
    }

    public override void Hurt(int damage)
    {
        base.Hurt(damage);

        (TrialHandler.CurrentTrial as Blimmy).BlimmyHurt(HealthPercent <= enrageThreshold);
    }
}
