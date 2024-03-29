using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public bool omaeWaMouShindeiru = false;
    public GameObject Molly;
    public GameObject NBMolly;
    public GameObject MMolly;
    public GameObject MNBMolly;
    public BigBrainBrimiBoyBigBrain FatherBigBrainBrimiBoyBigBrain;
    public int horizontal = 0;
    public int vertical = 0;

    public bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        FatherBigBrainBrimiBoyBigBrain = GameObject.FindGameObjectWithTag("Player").GetComponent<BigBrainBrimiBoyBigBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MissionNotFailedWellGetEmThisTime(int vertical, int horizontal, int type)
    {
        omaeWaMouShindeiru = true;
        this.horizontal = horizontal;
        this.vertical = vertical + 270;
        if (first == true)
        {
            if (type == 0)
            {
                Instantiate(Molly, FatherBigBrainBrimiBoyBigBrain.peekABoo, Quaternion.identity);
            }
            else if (type == 1)
            {
                Instantiate(NBMolly, FatherBigBrainBrimiBoyBigBrain.peekABoo, Quaternion.identity);
            }
            else if (type == 2)
            {
                Instantiate(MNBMolly, FatherBigBrainBrimiBoyBigBrain.peekABoo, Quaternion.identity);
            }
            else
            {
                Instantiate(MMolly, FatherBigBrainBrimiBoyBigBrain.peekABoo, Quaternion.identity);
            }
        }
        first = false;
    }
}
