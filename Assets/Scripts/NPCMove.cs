using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    public float speed;
    public float range;

    public float minWaitTime;
    public float maxWaitTime;

    public Rigidbody2D rb;
    public Transform NPC;

    public DialogueTrigger trigger;
    Vector2 movePosition;
    Vector2 difference;
    bool waiting;
    bool horizontal;
    private void Awake()
    {
        //Set the movePostion to the NPC's current position
        movePosition = transform.position;
    }
    private void Update()
    {
        //Check if the player is in range of the NPC and if dialogue is playing
        //if so this function will return and thus this NPC will stop moving
        if (trigger.playerInRange && DialogueManager.GetInstance().dialogueIsPlaying)
            return;

        //Get the difference between the Npc's current position and movePosition
        difference = ((Vector2)transform.position - movePosition);

        if (difference.x >= 0)
            difference.x += 1;
        else
            difference.x -= 1;

        if (difference.y >= 0)
            difference.y += 1;
        else
            difference.y -= 1;

        //Check if the NPC is moving more on the Horizontal or Vertical Axis
        if (Mathf.Abs(difference.x) >= Mathf.Abs(difference.y))
        {
            horizontal = true;
        }
        else
            horizontal = false;

        //Check if the distance between the NPC's current position and movePosition
        //is greater that 0.01f
        if (Vector2.Distance(transform.position, movePosition) > 0.01f)
        {
            //Set both Idle parameters to 0
           // Debug.Log("animator.SetFloat(Horizontal Idle, " + 0 + ")");
           // Debug.Log("animator.SetFloat(Verticals Idle, " + 0 + ")");

            if (horizontal)
            {
                //Debug.Log("animator.SetFloat(Horizontal Move, " + -difference.x + ")");
                //Debug.Log("animator.SetFloat(Vertical Move, " + 0 + ")");
            }
            else
            {
               //Debug.Log("animator.SetFloat(Horizontal Move, " + 0 + ")");
               // Debug.Log("animator.SetFloat(Vertical Move, " + -difference.y + ")");
            }
            //if it is we move the NPC's rigidbody towards the movePosition with a speed
            rb.position = Vector3.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
        }
        else
        {
            //Set both Move parameters to 0
            // Debug.Log("animator.SetFloat(Horizontal Move, " + 0 + ")");
             //Debug.Log("animator.SetFloat(Vertical Move, " + 0 + ")");


            if (horizontal)
            {
               // Debug.Log("animator.SetFloat(Horizontal Idle, " + -difference.x + ")");
                //Debug.Log("animator.SetFloat(Vertical Idle, " + 0 + ")");
            }
            else
            {
               // Debug.Log("animator.SetFloat(Horizontal Idle, " + 0 + ")");
               // Debug.Log("animator.SetFloat(Vertical Idle, " + -difference.y + ")");
            }

            //if we are not currently waiting we can start the WaitMove Coroutine
            if (!waiting)
                StartCoroutine(WaitToMove());
        }
            
    }
    IEnumerator WaitToMove()
    {
        //Set the waiting to true so we don't call this Coroutine more than it's neccesarry
        waiting = true;

        //Get a random time to wait
        float timeToWait = Random.Range(minWaitTime, maxWaitTime);

        //Wait that time
        yield return new WaitForSeconds(timeToWait);

        //Set the waiting to false
        waiting = false;

        //Get a random movePositon from the circle that appears in the inspector
        //(The Random.insideUnitCircle's circle will always be create at 0 0 0 
        //so we add the NPC's current position vector to it 
        //to make sure that this circle will be created around itself)
        movePosition = (Vector2)NPC.position + Random.insideUnitCircle * range;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Check if we hitted something that isn't the player and we are not currently waiting
        if (collision.gameObject.tag != "Player" && !waiting)
            //if so we set the movePostion to the NPC's current position so that the NPC
            //will not try to get through walls to reach it's point
            movePosition = transform.position;
    }

    //Draw the circle to see the range of the NPC's movement
    //(This will only work if on the scene tab we have the Gizmos toggle on)
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(NPC.position, range);
    }
}
