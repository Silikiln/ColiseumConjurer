using UnityEngine;
using System.Collections;

public class TestBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Test).TypeHandle);
    }
}
