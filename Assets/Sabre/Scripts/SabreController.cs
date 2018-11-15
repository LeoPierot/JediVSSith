using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DigitalRuby.LaserSword;

public class SabreController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            LaserSwordScript lss = GetComponent<LaserSwordScript>();
            if (lss.state == 0)
            {
                GetComponent<LaserSwordScript>().Activate();
            }
            else if (lss.state == 1)
            {
                GetComponent<LaserSwordScript>().Deactivate();
            }
        }
    }

    public void Open()
    {
        GetComponent<LaserSwordScript>().Activate();
    }
    public void Close()
    {
        GetComponent<LaserSwordScript>().Deactivate();
    }
} 

