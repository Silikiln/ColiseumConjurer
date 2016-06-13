using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {
    public bool isStatic = false;
    public float grabDistance = .75f;
    public float grabbedLinearDrag = 0;
    public float grabbedAngularDrag = 0;

    public bool Held { get; private set; }

    float originalLinearDrag;
    float originalAngularDrag;
    RigidbodyConstraints2D originalConstraints;

    new Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalLinearDrag = rigidbody.drag;
        originalAngularDrag = rigidbody.angularDrag;
        originalConstraints = rigidbody.constraints;
    }

    public virtual void Grabbed()
    {
        Held = true;
        rigidbody.drag = grabbedLinearDrag;
        rigidbody.angularDrag = grabbedAngularDrag;
        rigidbody.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
    }

    public virtual void Dropped()
    {
        Held = false;
        rigidbody.drag = originalLinearDrag;
        rigidbody.angularDrag = originalAngularDrag;
        rigidbody.constraints = originalConstraints;
    }
}
