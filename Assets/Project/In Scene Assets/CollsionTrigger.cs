using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollsionTrigger : MonoBehaviour
{
    GameObject text; //the text that will appear upon completing the game (GameObject GameOver)
    GameObject room; //the room that is moving (GameObject Room)
    GameObject[] lasers; // a list of all the objects tagged 'LASER'
    GameObject[] emitters1; // a list of all the objects tagged 'EMITTER1'
    GameObject[] emitters2; // a list of all the objects tagged 'EMITTER2'
    GameObject[] perp; // a list of all the objects tagged 'PERP'
    private bool collided = false;

    void Start()
    {
        text = GameObject.Find("GameOver");
        room = GameObject.Find("Room");
        //make sure text is not visible at start of game
        text.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }


    void OnCollisionEnter(Collision collision)
    {
        
         // laser name or may have to use .CompareTag(remember will have to create tag for the laser)
        if (collision.gameObject.CompareTag("LASER"))
        {
            // Collision logic here
            Debug.Log("Collided with object having YourTag");
            collided = true;

            text.gameObject.GetComponent<MeshRenderer>().enabled = true;
            //print("text on");

            room.SetActive(false);
            //print("room off);

            lasers = GameObject.FindGameObjectsWithTag("LASER");
            foreach(GameObject l in lasers)
            { 
                l.SetActive(false);
                //print("lasers off");
            }
            emitters1 = GameObject.FindGameObjectsWithTag("EMITTER1");
            foreach(GameObject e1 in emitters1)
            { 
                e1.SetActive(false);
                //print("e2 off");
            }
            emitters2 = GameObject.FindGameObjectsWithTag("EMITTER2");
            foreach(GameObject e2 in emitters2)
            { 
                e2.SetActive(false);
                //print("e5 off");
            }
            perp = GameObject.FindGameObjectsWithTag("PERP");
            foreach(GameObject p in emitters2)
            { 
                p.SetActive(false);
                //print("perp off");
            }
        }
            //SceneManager.LoadScene("GameOver");
            // Handle the collision event
        
    }

}
