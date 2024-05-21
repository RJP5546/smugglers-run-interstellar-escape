using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMovement : MonoBehaviour
{
    public static GameManager gm;
    public static TunnelManager tm;

    public float velocity;
    public float endTunnelZ = -40;

    public float Zpos;

    private void Start()
    {
        //get reference to GameManager
        gm = GameManager.instance;
        //get reference to TunnelManager
        tm = TunnelManager.TMinstance;

    }
    // Update is called once per frame
    void Update()
    {
        //get current velocity from GM
        velocity = gm.getCurrSpeed();
        //move the object towards the player
        gameObject.transform.position = gameObject.transform.position + (Vector3.back)*velocity*Time.deltaTime;
        Zpos = gameObject.transform.position.z;

        if (this.gameObject.transform.position.z < endTunnelZ)
        {
            //Check to see if this item is a tunnel or an obsticle
            if(gameObject.layer == 3)
            {
                //Call the tunnel manager to SpawnTunnel() and pass the tunnel to destroy
                tm.SpawnTunnel(this.gameObject);
            }
            //if its an obsticle, destroy it
            else if(gameObject.layer == 6 || gameObject.layer == 7) { Destroy(gameObject);}

        }
    }
}
