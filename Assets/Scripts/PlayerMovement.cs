using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 4f;
    public bool pruning = false;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        Move();
        Prune();
    }

    public int PlayerID = 1;

    void Move() {
        //Temporary, will move to controllers
        Vector2 velocity = Vector2.zero;
        velocity.x = Input.GetAxis("Horizontal" + PlayerID.ToString());
        velocity.y = Input.GetAxis("Vertical" + PlayerID.ToString());
        rb.velocity = MoveSpeed * velocity.normalized;
    }

    void Prune() {
        if ((PlayerID == 1 && Input.GetKey(KeyCode.Q)) || (PlayerID == 2 && Input.GetKey(KeyCode.Slash))){
            //EventBus.Publish<PruneEvent>(new PruneEvent(PlayerID));
            //Debug.Log("Player " + PlayerID.ToString() + " prunes at " + transform.position.ToString() + "!");
            pruning = true;
        } else {
            pruning = false;
        }
    }

}


class PruneEvent {
    public int PlayerID;
    //More variables here if necessary

    public PruneEvent(int id) {
        PlayerID = id;
    }
}