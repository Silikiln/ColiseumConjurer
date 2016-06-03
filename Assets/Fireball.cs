using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Hit");
        Destroy(gameObject);
    }
}
