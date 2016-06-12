using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {
    public float grabbedDrag = 0;

    public bool Held { get; private set; }

    float originalDrag;

    new Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalDrag = rigidbody.drag;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer != 8) return;

        PlayerController.Instance.NearGrabbable(gameObject);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer != 8) return;

        PlayerController.Instance.LeavingGrabbable(gameObject);
    }

    void OnDestroy() { PlayerController.Instance.LeavingGrabbable(gameObject); }

    public virtual void Grabbed()
    {
        Held = true;
        Vector2 positionOffset = PlayerController.Instance.transform.position - transform.position;
        rigidbody.drag = grabbedDrag;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public virtual void Dropped()
    {
        Held = false;
        rigidbody.drag = originalDrag;
        rigidbody.constraints = RigidbodyConstraints2D.None;
    }
}
