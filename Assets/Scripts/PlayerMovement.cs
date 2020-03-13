﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 4f;
    public bool pruning = false;
    public bool growing = false;

    Subscription<BuffEvent> buffSubscription;
    bool isSpeedingUp = false;
    bool isSpeedingDown = false;
    float curBuffTime = 0f;
    float duration = 0f;

    public int PlayerID = 1;
    void Start() {
        rb = GetComponent<Rigidbody>();
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
    }

    void Update() {
        Move();
        Prune();
        Grow();
        if (isSpeedingUp && curBuffTime + duration > Time.time)
        {
            MoveSpeed = 6f;
            EventBus.Publish<BuffStatusEvent>(new BuffStatusEvent("SPEED UP", PlayerID));
        }
        else if (isSpeedingDown && curBuffTime + duration > Time.time)
        {
            MoveSpeed = 2f;
            EventBus.Publish<BuffStatusEvent>(new BuffStatusEvent("SPEED DOWN", PlayerID));
        }
        else 
        {
            MoveSpeed = 4f;
            EventBus.Publish<BuffStatusEvent>(new BuffStatusEvent("", PlayerID));
        }
    }

    

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

    void Grow() {
        if ((PlayerID == 1 && Input.GetKey(KeyCode.E)) || (PlayerID == 2 && Input.GetKey(KeyCode.Period))){
            //EventBus.Publish<PruneEvent>(new PruneEvent(PlayerID));
            //Debug.Log("Player " + PlayerID.ToString() + " prunes at " + transform.position.ToString() + "!");
            growing = true;
        } else {
            growing = false;
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