using UnityEngine;
using System.Collections;

public class ShapeController : Grabbable {
    SlotController nearSlot;

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

    public void Near(SlotController slot) { nearSlot = slot; }

    public void Far() { nearSlot = null; }
}
