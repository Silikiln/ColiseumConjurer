using UnityEngine;
using System.Collections;

public class Shape : Grabbable {
    Slot nearSlot;

    public override void Dropped()
    {
        // Skrillex would love this line
        base.Dropped();

        if (nearSlot != null && nearSlot.ValidEntry(transform))
        {
            Destroy(gameObject);
            Destroy(nearSlot.gameObject);
            ((ShapeSlot)TrialHandler.CurrentTrial).SlotFilled();
        }
    }

    public void Near(Slot slot) { nearSlot = slot; }

    public void Far() { nearSlot = null; }
}
