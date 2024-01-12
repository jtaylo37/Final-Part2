// COPY AND PASTE ALL CODE INTO CORRECT SECTIONS
// 
// MAKE SURE BOTH VARIABLES ARE ADDED AND THE CORRECT INSTANCES ARE ATTACHED IN UNITY



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToCollisionCode : MonoBehaviour
{
    public GameObject text; //the text that will appear upon completing the game (GameObject GameOver)
    public GameObject room; //the room that is moving (GameObject Room)
    GameObject[] lasers; // a list of all the objects tagged 'LASER'
    GameObject[] emitters1; // a list of all the objects tagged 'EMITTER1'
    GameObject[] emitters2; // a list of all the objects tagged 'EMITTER2'
    GameObject[] perp; // a list of all the objects tagged 'PERP'

    void Start()
    {
        //make sure text is not visible at start of game
        text.SetActive(false);
    }

    void Update()
    {
        //once room stops moving, make text visible and hide everything else
        if(room.transform.position.x >= 10)//ADD CODE FOR COLLISIONS HERE 
        {
            text.SetActive(true);

            room.SetActive(false);
            lasers = GameObject.FindGameObjectsWithTag("LASER");
            foreach(GameObject l in lasers)
            { 
                l.SetActive(false);
            }
            emitters1 = GameObject.FindGameObjectsWithTag("EMITTER1");
            foreach(GameObject e1 in emitters1)
            { 
                e1.SetActive(false);
            }
            emitters2 = GameObject.FindGameObjectsWithTag("EMITTER2");
            foreach(GameObject e2 in emitters2)
            { 
                e2.SetActive(false);
            }
            perp = GameObject.FindGameObjectsWithTag("PERP");
            foreach(GameObject p in emitters2)
            { 
                p.SetActive(false);
            }
        }
    }
}
