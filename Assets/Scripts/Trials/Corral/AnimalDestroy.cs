using UnityEngine;
using System.Collections;

public class AnimalDestroy : MonoBehaviour {

    void OnDestroy()
    {
        ((Corral)TrialHandler.CurrentTrial).SpawnAnimals(1);
    }
}
