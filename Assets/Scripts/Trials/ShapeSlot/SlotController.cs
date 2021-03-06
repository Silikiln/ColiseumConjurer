﻿using UnityEngine;
using System.Collections;

public class SlotController : MonoBehaviour {
    static float angleAcceptableError = 10;
    static float maxDistance = .5f;

    bool filled = false;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (filled || coll.gameObject.layer != 10) return;
        coll.gameObject.GetComponent<ShapeController>().Near(this);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (filled || coll.gameObject.layer != 10) return;
        ShapeController shape = coll.gameObject.GetComponent<ShapeController>();
        if (!shape.Held)
            shape.Dropped();
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (filled || coll.gameObject.layer != 10) return;
        coll.gameObject.GetComponent<ShapeController>().Far();
    }

    public bool ValidEntry(Transform t)
    {
        if (filled) return false;

        float distance = Vector3.Distance(transform.position, t.transform.position);
        if (distance > maxDistance)
            return false;

        float angle = t.rotation.eulerAngles.z % 90;
        return Mathf.Abs((transform.rotation.eulerAngles.z % 90) - angle) <= angleAcceptableError;
    }
}
