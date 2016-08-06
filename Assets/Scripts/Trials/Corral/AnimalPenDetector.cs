using UnityEngine;
using System.Collections;

public class AnimalPenDetector : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        //check if the object that collided has a matching tag
        if (coll.gameObject.tag == transform.tag){
            //the player has successfully moved a matching animal inside its pen.
            ((Corral)TrialHandler.CurrentTrial).CorralSuccess();

            //need to add something here that ensures that the animal can't move anymore
            //or to change its tag so it can't count multiple times or something
            //other than destroying it
            //also needs something to make sure the entire animal is inside the pen
            Destroy(coll.gameObject);

        }
    }
}
