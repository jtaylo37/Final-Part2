using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class laserGeneration : MonoBehaviour
{
    int particleCount = 10; //number of lasers per level
    int perpLCount = 2; //number of perpendicular lasers that move side to side
    float laserSpeed = 0.5f; //the speed at which moving lasers move (m/s)
    float perpSpeed = 0.5f; // the spped at which the perpendicular lasers move
    float start = 0f; //used to determine how much time has passed (instead of a yield WaitForSeconds)
    public GameObject emitter; //set emitter game object
    public GameObject laser; //set laser game object
    public GameObject[] emitter1; //list of all of the primary emitters for each laser in scene
    public GameObject[] lasers; //list of all of the lasers for each laser in scene
    public GameObject[] emitter2; //list of all of the receiving emitters for each laser in scene
    public GameObject[] perpLasersL; //list of all of the lasers for each perpendicular laser
    int[] movement; //list of directions that lasers are moving (0 = none, 1 = down, 2 = up)
    bool[] perpMovement; //list of directions that perpendicular lasers are moving (true =  positive, false = negative)
    public GameObject room; //the room that is moving

    void Awake()
    {
        //set the number of lasers for each level
        emitter1 = new GameObject[particleCount];
        lasers = new GameObject[particleCount];
        emitter2 = new GameObject[particleCount];
        movement = new int[particleCount];

        perpLasersL = new GameObject[perpLCount];
        perpMovement = new bool[perpLCount];
    }

    // Start is called before the first frame update
    void Start()
    {
        //set locations for each laser in the level
        for (int i = 0; i < particleCount; i++)
        {
            //Instantiate x and y values
            float x = 0;
            float y = 0;

            //set x value to be ast set intervals (every 1m)
            x = -(2f) * (float)i + 1f;

            //Randomize y-value within set range
            y = Random.Range(0.5f, 4f); 

            if(i < particleCount/2)
            {
                movement[i] = 0;
            }
            else
            {
                //determine laser movement direction at start for moving lasers
                int direction; //1-1.5 goes down, 1.5-2 goes up
                if (y < 1.5f)
                {
                    direction = 2;
                }
                 else if (y >= 3)
                 {
                    direction = 1;
                }
                else
                {
                    direction = Random.Range(1,2);
                }

                movement[i] = (int)direction;
            }
            
            //create Emitter1 particle
            emitter1[i] = GameObject.Instantiate(emitter);
            emitter1[i].transform.position = new Vector3(x, y, -1.97f);
            emitter1[i].transform.rotation = Quaternion.Euler(new (0, 0, 0));;
            emitter1[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            emitter1[i].tag = "EMITTER1";

            //create Laser particle with collider
            lasers[i] = GameObject.Instantiate(laser);
            lasers[i].transform.position = new Vector3(x, y, 0);
            lasers[i].transform.rotation = Quaternion.Euler(new (0, 90, 90));;
            lasers[i].transform.localScale = new Vector3(0.1f, 02f, 0.1f);
            lasers[i].tag = "LASER";
            GameObject currLaser = lasers[i];
            currLaser. AddComponent<CapsuleCollider>();
            Collider laserCol = currLaser.GetComponent<Collider>();
            laserCol.isTrigger = false;

            //create Emitter2 particle
            emitter2[i] = GameObject.Instantiate(emitter);
            emitter2[i].transform.position = new Vector3(x, y, 1.97f);
            emitter2[i].transform.rotation = Quaternion.Euler(new (0, 180, 0));
            emitter2[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            emitter2[i].tag = "EMITTER2";
        }

        //set locations for the perpendicular lasers
        for (int i = 0; i < perpLCount; i++)
        {
            //Instantiate x and y values
            float z = 0;
            float y = 0;

            //set z and y values to have 1 laser on each side of room, with one being high and one being low
            //also set initial movement direction
            if(i < perpLCount/2)
            {
                z = -1.5f;
                y = Random.Range(0.5f, 1.5f); 
                perpMovement[i] = true;
            }
            else
            {
                z = 1.5f;
                y = Random.Range(1.5f, 2.5f);
                perpMovement[i] = false;
            }

            //create Laser particle
            perpLasersL[i] = GameObject.Instantiate(laser);
            perpLasersL[i].transform.position = new Vector3(0, y, z);
            perpLasersL[i].transform.rotation = Quaternion.Euler(new (0, 0, 90));;
            perpLasersL[i].transform.localScale = new Vector3(0.1f, 20f, 0.1f);
            perpLasersL[i].tag = "PERP";

        }
    }

    // Update is called once per frame
    void Update()
    {
        //increment how much time has passed since the first Update was called
        start = start + Time.deltaTime;

        //while not at the end of the room
        if (room.transform.position.x >= 25 && room.transform.position.x <= 28)
        {
            //move the perpendicular lasers side to side
            // for(int i = 0; i < perpLCount; i++)
            // {
            //     if (perpMovement[i] == true)
            //     {
            //         //move in the positive direction
            //         perpLasersL[i].transform.Translate(0, 0, perpSpeed * Time.deltaTime);

            //         //check that lasers are within bounds, if not then switch direction
            //         if(perpLasersL[i].transform.position.z >= 1.5)
            //         {
            //             perpMovement[i] = false;
            //         }
            //     }
            //     else
            //     {
            //         //move in the negative direction
            //         perpLasersL[i].transform.Translate(0, 0, -perpSpeed * Time.deltaTime);

            //         //check that lasers are within bounds, if not then switch direction
            //         if(perpLasersL[i].transform.position.z <= -1.5)
            //         {
            //             perpMovement[i] = true;
            //         }
            //     }
            // }
        }

        //move primary lasers that move
        // for(int i = 0; i < particleCount; i++)
        // {
        //     if (movement[i] == 2)
        //     {
        //         //move in the positive direction (up)
        //         emitter1[i].transform.Translate(0, laserSpeed * Time.deltaTime, 0);
        //         lasers[i].transform.Translate(laserSpeed * Time.deltaTime, 0, 0); //what the fuck! now the lasers have x and y flipped
        //         emitter2[i].transform.Translate(0, laserSpeed * Time.deltaTime, 0);

        //         //check that lasers are within bounds, if not then switch direction
        //         if(lasers[i].transform.position.y >= 4)
        //         {
        //             movement[i] = 1;
        //         }
        //     }
        //     else if (movement[i] == 1)
        //     {
        //         //move in the negative direction (down)
        //         emitter1[i].transform.Translate(0, -laserSpeed * Time.deltaTime, 0);
        //         lasers[i].transform.Translate(-laserSpeed * Time.deltaTime, 0, 0); //what the fuck! now the lasers have x and y flipped
        //         emitter2[i].transform.Translate(0, -laserSpeed * Time.deltaTime, 0);

        //         //check that lasers are within bounds, if not then switch direction
        //         if(lasers[i].transform.position.y <= 0.5)
        //         {
        //             movement[i] = 2;
        //         }
        //     }
        // }
    }
}
