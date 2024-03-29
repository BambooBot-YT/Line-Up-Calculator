using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBrainBrimiBoyBigBrain : MonoBehaviour
{
    public GameObject Molly;
    public int low = 0;
    public int high = 180;
    public int left = 0;
    public int right = 360;
    public int manufactureTag = 0;
    public float chestHeigt = 0;

    public float lastMollyVertical = 0;
    public float lastMollyHorizontal = 0;

    public Spike spikeBrain;

    public Vector3 peekABoo;

    public Vector3 directionToSpike;
    public float angleToSpike;

    public float throwForceMagnitude = 10f;
    public float horizontalAdjustment = 1;
    public float verticalAdjustment = 1;
    public float bounceSpeedFilterAdjsutment = 1;
    public float angleAdjust = 0;
    public float verticalForceAdjust = 1;
    public float horizontalAngleAdjust = 0;

    public float throdledSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        lastMollyVertical = low;
        lastMollyHorizontal = left;
        manufactureTag = 0;
        Vector3 currentPosition = transform.position;
        currentPosition.y += chestHeigt;
        peekABoo = currentPosition;
        Instantiate(Molly, currentPosition, Quaternion.identity);


        spikeBrain = GameObject.FindGameObjectWithTag("spike").GetComponent<Spike>();

        directionToSpike = spikeBrain.transform.position - transform.position;

        Debug.Log("Direction to Spike: " + directionToSpike.normalized);
        angleToSpike = Vector3.Angle(Vector3.forward, directionToSpike);
        Debug.Log("Angle to Spike: " + angleToSpike + " degrees");

        Debug.Log(Vector3.Distance(transform.position, spikeBrain.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextMolly()
    {
        if (spikeBrain.omaeWaMouShindeiru == false)
        {
            manufactureTag = manufactureTag + 1;
            float maxDistance = 0;
            do
            { 
                float horizontalAspect = horizontalAdjustment * throwForceMagnitude * ((180 - Mathf.Abs(Mathf.Abs(angleToSpike - lastMollyHorizontal + horizontalAngleAdjust) - 180)) / 180);
                Debug.Log("Spike angle " + angleToSpike);
                Debug.Log("Throw angle " + lastMollyHorizontal);
                Debug.Log("Horizontal part " + horizontalAspect);
                float verticalAspect = verticalAdjustment * throwForceMagnitude * ((lastMollyVertical + angleAdjust) / 90);
                Debug.Log("Vertical part " + verticalAspect);
                maxDistance = horizontalAspect + verticalAspect;
                Debug.Log("Total travel " + maxDistance);

                lastMollyHorizontal = lastMollyHorizontal + 1;
                if (lastMollyHorizontal > right)
                {
                    lastMollyVertical = lastMollyVertical + 1;
                    lastMollyHorizontal = left;
                }
                if (lastMollyVertical > high)
                {
                    break;
                }
                
            }
            //while (Vector3.Distance(transform.position, spikeBrain.transform.position) > maxDistance);
            while (false);
            Vector3 currentPosition = transform.position;
            currentPosition.y += chestHeigt;
            Instantiate(Molly, currentPosition, Quaternion.identity);
        }
    }
}
