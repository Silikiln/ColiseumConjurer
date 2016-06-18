using UnityEngine;
using System.Collections;

public class EnemyController : MovingEntity
{

    #region Material Modifiers

    public static float EnemyHealthMultiplier = 1;
    public static float EnemyHealthAdded = 0;

    public static float EnemyMoveSpeedMultiplier = 1;
    public static float EnemyMoveSpeedAdded = 0;

    public static float EnemyDamageMultiplier = 1;
    public static float EnemyDamageAdded = 0;

    public static float EnemyDamageReceivedMultiplier = 1;
    public static float EnemyDamageReceivedAdded = 0;

    public static float EnemySizeMultiplier = 1;
    public static float EnemySizeAdded = 0;

    public static float EnemyCountMultiplier = 1;
    public static float EnemyCountAdded = 0;
    public static int CurrentEnemyCount = 0;

    public static int TotalEnemyCount {
        get
        {
            return (int)(CurrentEnemyCount * EnemyCountMultiplier + EnemyCountAdded);
        }
    }

    #endregion

    protected override float HealthMultiplier { get { return EnemyHealthMultiplier; } }
    protected override float HealthAdded { get { return EnemyHealthAdded; } }

    protected override float DamageDealtMultiplier { get { return EnemyDamageMultiplier; } }
    protected override float DamageDealtAdded { get { return EnemyDamageAdded; } }

    protected override float DamageRecievedMultiplier { get { return EnemyDamageReceivedMultiplier; } }
    protected override float DamageRecievedAdded { get { return EnemyDamageReceivedAdded; } }

    protected override float SizeMultiplier { get { return EnemySizeMultiplier; } }
    protected override float SizeAdded { get { return EnemySizeAdded; } }

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
