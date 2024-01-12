using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomMove : MonoBehaviour
{
    float startSpeed = 0.5f; //the speed that the room moves at when the game starts
    float endSpeed = 0.5f; //the speed at which the room will stop moving once it reaches this point
    float increment = 0.1f; //increment by this amount every time the speed is increased
    float incrementWait = 1f; //increment the speed after this distance is traveled
    public float speed; //current speed
    float xFinal = 30; //the distacne at which the room stops moving
    float xPos; //last recorded x posisiotn (for referencing the distance traveled)
    float start = 0f; //used to determine how much time has passed (instead of a yield WaitForSeconds)
    GameObject[] lasers; // a list of all the objects tagged 'LASER'
    GameObject[] emitters1; // a list of all the objects tagged 'EMITTER1'
    GameObject[] emitters2; // a list of all the objects tagged 'EMITTER2'

    // Start is called before the first frame update
    void Start()
    {
        //set intial speed and x position
        speed = startSpeed;
        xPos = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //increment how much time has passed since the first Update was called
        start = start + Time.deltaTime;

        //while not at the end of the room
        if (xPos <= xFinal && start >= 5)
        {
            //move room
            this.transform.Translate(speed * Time.deltaTime, 0, 0); 

            //move lasers
            lasers = GameObject.FindGameObjectsWithTag("LASER");
            foreach(GameObject l in lasers)
            { 
                l.transform.Translate(0, 0, speed * Time.deltaTime); //oh my fucking god why do i need to change z when they are moving in x
            }
            emitters1 = GameObject.FindGameObjectsWithTag("EMITTER1");
            foreach(GameObject e1 in emitters1)
            { 
                e1.transform.Translate(speed * Time.deltaTime, 0, 0); 
            }
            emitters2 = GameObject.FindGameObjectsWithTag("EMITTER2");
            foreach(GameObject e2 in emitters2)
            { 
                e2.transform.Translate(-speed * Time.deltaTime, 0, 0); 
            }

            //incremenet speed if the appropriate distance has passed
           if(this.transform.position.x >= (xPos + incrementWait)) 
           {
            //increase the current xPosition by the increment
            xPos += incrementWait;

            //increase speed if under the speed limit
            if (speed <= endSpeed)
            {
                speed += increment;
            }
           }
        }
    }
}
