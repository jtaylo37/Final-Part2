using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class congrats : MonoBehaviour
{
    GameObject text; //the text that will appear upon completing the game
    GameObject room; //the room that is moving

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("Congratulations");
        room = GameObject.Find("Room");
        //make sure text is not visible at start of game
        text.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //once room stops moving, make text visible
        if(room.transform.position.x >= 30) 
        {
            text.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
