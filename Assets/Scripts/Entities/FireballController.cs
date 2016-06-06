using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {
    public Entity parent;

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.SendMessage("Hurt", parent.Damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
