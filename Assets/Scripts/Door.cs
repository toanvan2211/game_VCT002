using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key, enemy, button
}

public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public BoxCollider2D triggerCollider;
    public Inventory playerInventory;

    private void Update()
    {
        if (Input.GetButtonDown("attack"))
        {
            if (playerInRange && thisDoorType == DoorType.key)
            {
                //Does the player have a key?
                if (playerInventory.numberOfKey > 0)
                {
                    //Remove a player key
                    playerInventory.numberOfKey--;
                    //If so, then call the open method
                    Open();
                }
            }
        }
    }

    public void Open()
    {
        //Turn off the door's sprite renderer
        doorSprite.enabled = false;
        //Set the door is open
        open = true;
        //Turn off the door's box collider
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
    }

    public void Close()
    {

    }
}
