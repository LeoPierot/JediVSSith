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
    public void Open()
    {
        GetComponent<LaserSwordScript>().Activate();
    }
    public void Close()
    {
        GetComponent<LaserSwordScript>().Deactivate();
    }
} 

