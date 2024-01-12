// IMDM Course material
// Author: Myungin Lee
// Date: Fall 2023
// This code demonstrates the general applications of landmark information
// Pose + Left, Right hand landmarks data avaiable. Facial landmark need custom work
// Landmarks label reference: 
// https://developers.google.com/mediapipe/solutions/vision/pose_landmarker
// https://developers.google.com/mediapipe/solutions/vision/hand_landmarker

using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Genesis : MonoBehaviour
{
    public Transform VRRig;
    Vector3 vrOffset = new Vector3(.38f, 1.37f, -0.11f);
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    // Declare landmark vectors 
    public Vector3[] pose = new Vector3[poseLandmark_number];
    public Vector3[] righthandpos = new Vector3[handLandmark_number];
    public Vector3[] lefthandpos = new Vector3[handLandmark_number];
    float scalar = 0.8f;
    public GameObject[] PoseLandmarks, LeftHandLandmarks, RightHandLandmarks;
    public GameObject capsulePrefab;
    int[,] linePairs = new int[,] {
        {11, 12}, {12, 14}, {14, 16}, {16, 20}, {20, 18}, {18, 16},
        {11, 13}, {13, 15}, {15, 19}, {19, 17}, {17, 15},
        {12, 24}, {24, 26}, {26, 28},
        {24, 23},
        {11, 23}, {23, 25}, {25, 27}
    };
    private GameObject head, rhand, lhand, body;
    private GameObject[] capsulePool;
    public static Genesis gen; // singleton
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;
    private void Awake()
    {
        if (Genesis.gen == null)
        {
            Genesis.gen = this;
        }
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        PoseLandmarks = new GameObject[poseLandmark_number];
        LeftHandLandmarks = new GameObject[handLandmark_number];
        RightHandLandmarks = new GameObject[handLandmark_number];
    }
    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.Find("head");
        rhand = GameObject.Find("rhand");
        lhand = GameObject.Find("lhand");
        body = GameObject.Find("body");
        head.transform.localScale = new Vector3(0f, 0f, 0f);
        body.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        rhand.transform.localScale = new Vector3(0f, 0f, 0f);
        lhand.transform.localScale = new Vector3(0f, 0f, 0f);
        // Initiate pose landmarks as spheres
        for (int i = 0; i < poseLandmark_number; i++)
        {
            PoseLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            PoseLandmarks[i].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        }
        // Initiate R+L hands landmarks as spheres
        for (int i = 0; i < handLandmark_number; i++)
        {
            LeftHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            LeftHandLandmarks[i].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            RightHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            RightHandLandmarks[i].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        }

        //COMMENTTTT
        capsulePool = new GameObject[linePairs.GetLength(0)];
        for (int i = 0; i < capsulePool.Length; i++)
        {
            capsulePool[i] = Instantiate(capsulePrefab); //uses prefab
            capsulePool[i].SetActive(false); // Start with the capsules inactive
        }

    }

    void PositionCapsule(GameObject capsule, Vector3 start, Vector3 end, float width)
{
    Vector3 direction = end - start;
    float distance = direction.magnitude;

    capsule.transform.position = start + direction / 2;
    capsule.transform.up = direction;
    capsule.transform.localScale = new Vector3(width, distance / 2, width);
}

    // void CreateCapsuleBetweenPoints(Vector3 start, Vector3 end, float width)    
    // {
    //     Vector3 direction = end - start;
    //     float distance = direction.magnitude;

    //     GameObject capsule = Instantiate(capsulePrefab, start + direction / 2, Quaternion.identity);
    //     capsule.transform.up = direction;
    //     capsule.transform.localScale = new Vector3(width, distance / 2, width); // Scale capsule
    // }


    // Update is called once per frame
    void Update()
    {
        // Case 0. Draw holistic shape
        // Assign Pose landmarks position
        int idx = 0;
        foreach (GameObject pl in PoseLandmarks)
        {
            //pl.transform.transform.position = -pose[idx] * scalar; //* scalar;
            Vector3 localLandmarkPosition1 = -pose[idx] * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition1 = localLandmarkPosition1 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition1 = VRRig.TransformPoint(offsetLocalPosition1); // Convert to world position
            pl.transform.position = worldLandmarkPosition1;

            Color customColor = new Color(idx*100 / 255, idx * 50 / 255, idx * 30 / 255, 1); // Color of pose landmarks
            pl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            idx++;
        }
            //BUILDING LINE
         for (int i = 0; i < linePairs.GetLength(0); i++)
        {   
            int startIdx = linePairs[i, 0];
            int endIdx = linePairs[i, 1];

            if (startIdx < PoseLandmarks.Length && endIdx < PoseLandmarks.Length)
            {
                GameObject capsule = capsulePool[i];
                capsule.SetActive(true);
                PositionCapsule(capsule, PoseLandmarks[startIdx].transform.position, PoseLandmarks[endIdx].transform.position, 0.05f);//change .02 to scale up
            }
        }

    // Deactivate unused capsules
    for (int i = linePairs.GetLength(0); i < capsulePool.Length; i++)
    {
        capsulePool[i].SetActive(false);
    }

        // Assign Left hand landmarks position
        idx = 0;
        foreach (GameObject lhl in LeftHandLandmarks)
        {
            //lhl.transform.transform.position = -lefthandpos[idx] * scalar;
            Vector3 localLandmarkPosition2 = -pose[idx] * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition2 = localLandmarkPosition2 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition2 = VRRig.TransformPoint(offsetLocalPosition2); // Convert to world position
            lhl.transform.position = worldLandmarkPosition2;
            
            Color customColor = new Color(idx * 4 / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of left hand landmarks
            lhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            //Debug.Log("Left Hand Landmark " + idx + ": " + lefthandpos[idx]);
            idx++;
            
        }
        // Assign Right hand landmarks position
        idx = 0;
        foreach (GameObject rhl in RightHandLandmarks)
        {
            //rhl.transform.transform.position = -righthandpos[idx] * scalar;
            Vector3 localLandmarkPosition3 = -pose[idx] * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition3 = localLandmarkPosition3 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition3 = VRRig.TransformPoint(offsetLocalPosition3); // Convert to world position
            rhl.transform.position = worldLandmarkPosition3;
            
            Color customColor = new Color(idx * 4f / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of right hand landmarks
            rhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
             ///Debug.Log("Right Hand Landmark " + idx + ": " + righthandpos[idx]);
            idx++;
            
        }

        // Case 1. Sound synth model ( head, R+L hands + body)
        // This part use existing Gameobjects and assign position. Not relavent to audio at all.
        //head.transform.position = -pose[0] * scalar;
        Vector3 localLandmarkPosition4 = -pose[0] * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition4 = localLandmarkPosition4 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition4 = VRRig.TransformPoint(offsetLocalPosition4); // Convert to world position
            //head.transform.position = worldLandmarkPosition4;

        //rhand.transform.position = -pose[15] * scalar;
        Vector3 localLandmarkPosition5 = -pose[15] * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition5 = localLandmarkPosition5 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition5 = VRRig.TransformPoint(offsetLocalPosition5); // Convert to world position
            rhand.transform.position = worldLandmarkPosition5;

        //lhand.transform.position = -pose[16] * scalar;
        Vector3 localLandmarkPosition6 = -pose[16] * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition6 = localLandmarkPosition6 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition6 = VRRig.TransformPoint(offsetLocalPosition6); // Convert to world position
            lhand.transform.position = worldLandmarkPosition6;

        //body.transform.position = -(pose[11] + pose[12] + pose[23] + pose[24]) / 4 * scalar;
        Vector3 localLandmarkPosition7 = -(pose[11] + pose[12] + pose[23] + pose[24]) / 4 * scalar; // Local position relative to VR rig
            Vector3 offsetLocalPosition7 = localLandmarkPosition7 + vrOffset; // Add offset
            Vector3 worldLandmarkPosition7 = VRRig.TransformPoint(offsetLocalPosition7); // Convert to world position
            body.transform.position = worldLandmarkPosition7;
         //Debug.Log("Body Landmark " + body);
        
    }
}
