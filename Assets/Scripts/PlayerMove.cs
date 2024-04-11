using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    Vector2 movement;

    private void Update()
    {
        //Get the horizontal and vertical input of the keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
        //Normalize the vector we got 
        //so that we will not go at twice the speed when we press a diagonal direction
        movement.Normalize();
    }

    private void FixedUpdate()
    {
        //Check if we are in a dialogue or if the inventory is opened
        //If so this function will return and thus the player will not move
        //it's Rigidbody anymore
        if (DialogueManager.GetInstance().dialogueIsPlaying || InventoryUIManager.openedInventory)
            return;

        //Move the player's rigidbody to the movement dirrection
        //multiplied by speed and Time.fixedDeltaTime
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

}
