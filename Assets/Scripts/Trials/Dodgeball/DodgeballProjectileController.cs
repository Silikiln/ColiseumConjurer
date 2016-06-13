using UnityEngine;
using System.Collections;

public class DodgeballProjectileController : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D coll)
    {
        ((Dodgeball)TrialHandler.CurrentTrial).ProjectileHit(coll.gameObject.layer == 8);
        Destroy(gameObject);
    }
}
