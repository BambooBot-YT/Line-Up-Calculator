using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBrainBrimiBoyBigBrain : MonoBehaviour
{
    public GameObject Molly;

    private float lastPitch = 0;
    private float lastYaw = 0;

    public Vector3 origin;

    public float throwForceMagnitude = 10f;
    public float horizontalAdjustment = 1;
    public float verticalAdjustment = 1;
    public float bounceSpeedFilterAdjsutment = 1;
    public float verticalForceAdjust = 1;
    public float horizontalAngleAdjust = 0;

    public float throdledSpeed = 1;

    public float speed = 1f;

    void Start()
    {
        origin = transform.position + Vector3.up;
        StartCoroutine(SpawnMolly());
    }

    IEnumerator SpawnMolly()
    {
        while (true) // TODO: until found
        {
            yield return new WaitForSeconds(1/speed);
            for (int i = 0; i < 100; i++)
            {
                NextMolly();
            }
        }
    }


    public void NextMolly()
    {
        lastYaw += 1f;
        lastPitch += (1f / 360f);
        GameObject molly = Instantiate(Molly, origin, Quaternion.identity);
        ViperKillJoyMolly mollyScript = molly.GetComponent<ViperKillJoyMolly>();
        mollyScript.SetVelocity(Quaternion.Euler(-lastPitch, lastYaw, 0f) * Vector3.forward);
    }
}
