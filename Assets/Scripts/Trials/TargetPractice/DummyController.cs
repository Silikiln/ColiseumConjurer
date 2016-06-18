using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour {
    void OnDestroy()
    {
        (TrialHandler.CurrentTrial as TargetPractice).DummyKilled();
    }
}
