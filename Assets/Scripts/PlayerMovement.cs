﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 4f;
    Subscription<BuffEvent> buffSubscription;
    bool isSpeedingUp = false;
    bool isSpeedingDown = false;
    float curBuffTime = 0f;
    float duration = 0f;
    void Start() {
        rb = GetComponent<Rigidbody>();
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
    }

    void Update() {
        Move();
        Prune();
        if (isSpeedingUp && curBuffTime + duration > Time.time)
        {
            MoveSpeed = 6f;
        }
        else if (isSpeedingDown && curBuffTime + duration > Time.time)
        {
            MoveSpeed = 2f;
        }
        else 
        {
            MoveSpeed = 4f;
        }
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
        if ((PlayerID == 1 && Input.GetKeyDown(KeyCode.Q)) || (PlayerID == 2 && Input.GetKeyDown(KeyCode.Slash))){
            EventBus.Publish<PruneEvent>(new PruneEvent(PlayerID));
            Debug.Log("Player " + PlayerID.ToString() + " prunes at " + transform.position.ToString() + "!");
        }
    }

    void _OnBuffUpdated(BuffEvent e)
    {
        if (e.playerIndex == PlayerID)
        {
            switch(e.type)
            {
                // make sure only one speeding buff is exist at one time
                case BuffController.buffType.speedDown:
                    if (isSpeedingUp) {
                        isSpeedingUp = false;
                        curBuffTime = 0f;
                    }
                    if (isSpeedingDown)
                    {
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                    else {
                        isSpeedingDown = true;
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                break;
                case BuffController.buffType.speedUp:
                    if (isSpeedingDown) {
                        isSpeedingDown = false;
                        curBuffTime = 0f;
                    }
                    if (isSpeedingUp)
                    {
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                    else {
                        isSpeedingUp = true;
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                break;
            }
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