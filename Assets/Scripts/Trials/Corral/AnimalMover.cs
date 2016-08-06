using UnityEngine;
using System.Collections;

//script used to move the animal object it is attached to
public class AnimalMover : MovingEntity {
    public Transform target;
    public bool canMove = true;
    void FixedUpdate()
    {
        if (canMove){
            if (target == null){
                if (PlayerController.Instance != null)
                    target = PlayerController.Instance.transform;
                else
                    return;
            }

            //check if the player is within the detection range and facing the animal
            float playerDistance = Vector2.Distance(target.position, transform.position);
            if (playerDistance < 2){
                //the animal needs to move
                Vector2 moveDirection = transform.position - target.position;
                rigidbody.AddForce(moveDirection.normalized * MoveSpeed);
                if (rigidbody.velocity.magnitude > MaxSpeed)
                    rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
            }
        }
    }

    public void modifyStatsByAnimal()
    {
        int size = 0;
        switch (tag)
        {
            case "Cow":
                size = 20;
                break;
            case "Pig":
                size = 15;
                break;
            case "Chicken":
                size = 5;
                break;
            case "Sheep":
                size = 10;
                break;
            case "Rabbit":
                size = 1;
                break;
            default:
                break;
        }

        GetComponent<AnimalMover>().BaseSize = size;
    }
}
