using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class oldLaserBackup : MonoBehaviour
{
    int particleCount1 = 10; //number of lasers in level 1
    int particleCount2 = 7; //number of laser in level 2
    int perpLCount = 2; //number of perpendicular lasers that move side to side
    int particleCount; //number of lasers per level
    float speed = 0.5f; //the speed at which moving lasers move (m/s)
    public GameObject emitter; //set emitter game object
    public GameObject laser; //set laser game object
    public GameObject[] emitter1; //list of all of the primary emitters for each laser in scene
    public GameObject[] lasers; //list of all of the lasers for each laser in scene
    public GameObject[] emitter2; //list of all of the receiving emitters for each laser in scene
    public GameObject[] perpLasersE1; //list of all of the primary emitters for each perpendicualr laser
    public GameObject[] perpLasersL; //list of all of the lasers for each perpendicular laser
    public GameObject[] perpLasersE2; //list of all of the receiving emitters for each perpendicualr laser
    bool[] movement; //list of directions that lasers are moving (true = up, false = down)
    bool[] perpMovement; //list of directions that perpendicular lasers are moving (true =  positive, false = negative)

    void Awake()
    {
        //determine which level is loaded and set appropriate laser count
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            particleCount = particleCount1;
        }
        if(SceneManager.GetActiveScene().name == "Level2")
        {
            particleCount = particleCount2;
            movement = new bool[particleCount];
        }

        //set the number of lasers for each level
        emitter1 = new GameObject[particleCount];
        lasers = new GameObject[particleCount];
        emitter2 = new GameObject[particleCount];

        perpLasersE1 = new GameObject[perpLCount];
        perpLasersL = new GameObject[perpLCount];
        perpLasersE2 = new GameObject[perpLCount];
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

            if (SceneManager.GetActiveScene().name == "Level1")
            {
                //Randomize x-values within set range 
                float i2;
                if (i == 0) {i2 = 0f;}  
                else {i2 = (float)(i%((float)particleCount/2f));}
                print("i2: " + i2);
                float xStart = (2f/5f) * i2 - 1f;
                float xEnd = xStart + (2f/5f);
                print("xStart: " + xStart + ", xEnd: " + xEnd);
                x = Random.Range(xStart, xEnd);
                print(x);

                //Randomize y-value within set range
                if(i < particleCount/2)
                {
                    y = Random.Range(-0.5f, 0.5f); 
                }
                else
                {
                    y = Random.Range(0.5f, 1.5f);
                }
            }

            if (SceneManager.GetActiveScene().name == "Level2")
            {
                //set x value to be one laser every 1ft (1/3m)
                x = (2f/6f) * (float)i - 1f;
                print(x);

                //Randomize y-value within set range
                y = Random.Range(-0.5f, 3f); 

                //determine laser movement direction at start
                int direction;  //0-0.5 goes down, 0.5-1 goes up
                if (y < 0.5f) {direction = 0;}
                else if (y >= 0.5) {direction = 1;}
                else {direction = Random.Range(0,1);}

                if (direction < 0.5) {movement[i] = false;}
                else {movement[i] = true;}
            }
            
            //create Emitter1 particle
            emitter1[i] = GameObject.Instantiate(emitter);
            emitter1[i].transform.position = new Vector3(x, y, -1.97f);
            emitter1[i].transform.rotation = Quaternion.Euler(new (0, 0, 0));;
            emitter1[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            //create Laser particle
            lasers[i] = GameObject.Instantiate(laser);
            lasers[i].transform.position = new Vector3(x, y, 0);
            lasers[i].transform.rotation = Quaternion.Euler(new (0, 90, 90));;
            lasers[i].transform.localScale = new Vector3(0.1f, 02f, 0.1f);

            //create Emitter2 particle
            emitter2[i] = GameObject.Instantiate(emitter);
            emitter2[i].transform.position = new Vector3(x, y, 1.97f);
            emitter2[i].transform.rotation = Quaternion.Euler(new (0, 180, 0));;
            emitter2[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
                y = Random.Range(-0.5f, 0.5f); 
                perpMovement[i] = true;
            }
            else
            {
                z = 1.5f;
                y = Random.Range(0.5f, 1.5f);
                perpMovement[i] = false;
            }

            //create Emitter1 particle
            perpLasersE1[i] = GameObject.Instantiate(emitter);
            perpLasersE1[i].transform.position = new Vector3(-1.97f, y, z);
            perpLasersE1[i].transform.rotation = Quaternion.Euler(new (0, 90, 0));;
            perpLasersE1[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            perpLasersE1[i].name = "Perpendicular Laser Emitter";

            //create Laser particle
            perpLasersL[i] = GameObject.Instantiate(laser);
            perpLasersL[i].transform.position = new Vector3(0, y, z);
            perpLasersL[i].transform.rotation = Quaternion.Euler(new (0, 0, 90));;
            perpLasersL[i].transform.localScale = new Vector3(0.1f, 02f, 0.1f);

            //create Emitter2 particle
            perpLasersE2[i] = GameObject.Instantiate(emitter);
            perpLasersE2[i].transform.position = new Vector3(1.97f, y, z);
            perpLasersE2[i].transform.rotation = Quaternion.Euler(new (0, -90, 0));;
            perpLasersE2[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move the perpendicular lasers side to side
        for(int i = 0; i < perpLCount; i++)
        {
            if (perpMovement[i] == true)
            {
                //move in the positive direction
                perpLasersE1[i].transform.Translate(-speed * Time.deltaTime, 0, 0); //why the fuck are the x and z flipped for the emitters?? and E1 is negative??
                perpLasersL[i].transform.Translate(0, 0, speed * Time.deltaTime);
                perpLasersE2[i].transform.Translate(speed * Time.deltaTime, 0, 0); //why the fuck are the x and z flipped for the emitters??

                //check that lasers are within bounds, if not then switch direction
                if(perpLasersL[i].transform.position.z >= 1.5)
                {
                    perpMovement[i] = false;
                }
            }
            else
            {
                //move in the negative direction
                perpLasersE1[i].transform.Translate(speed * Time.deltaTime, 0, 0); //why the fuck are the x and z flipped for the emitters?? and E1 is negative??
                perpLasersL[i].transform.Translate(0, 0, -speed * Time.deltaTime);
                perpLasersE2[i].transform.Translate(-speed * Time.deltaTime, 0, 0); //why the fuck are the x and z flipped for the emitters??

                //check that lasers are within bounds, if not then switch direction
                if(perpLasersL[i].transform.position.z <= -1.5)
                {
                    perpMovement[i] = true;
                }
            }
        }

        //move primary lasers in level 2
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            for(int i = 0; i < particleCount; i++)
            {
                if (movement[i] == true)
                {
                    //move in the positive direction (up)
                    emitter1[i].transform.Translate(0, speed * Time.deltaTime, 0);
                    lasers[i].transform.Translate(speed * Time.deltaTime, 0, 0); //what the fuck! now the lasers have x and y flipped
                    emitter2[i].transform.Translate(0, speed * Time.deltaTime, 0);

                    //check that lasers are within bounds, if not then switch direction
                    if(lasers[i].transform.position.y >= 3)
                    {
                        movement[i] = false;
                    }
                }
                else
                {
                    //move in the negative direction (down)
                    emitter1[i].transform.Translate(0, -speed * Time.deltaTime, 0);
                    lasers[i].transform.Translate(-speed * Time.deltaTime, 0, 0); //what the fuck! now the lasers have x and y flipped
                    emitter2[i].transform.Translate(0, -speed * Time.deltaTime, 0);

                    //check that lasers are within bounds, if not then switch direction
                    if(lasers[i].transform.position.y <= -0.5)
                    {
                        movement[i] = true;
                    }
                }
            }
        }
    }
}
