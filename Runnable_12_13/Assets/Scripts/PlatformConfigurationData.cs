using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class PlatformConfigurationData :    MonoBehaviour
{

    public float deltaSpace = 0.2f; 
    public int M = 16;
  public   int N = 8 ;
  public  float RandomHeight = 7.0f ;


   override public string ToString()
    {
        return String.Format("{0},{1},{2},{3}", M, N, deltaSpace, RandomHeight);
         
    }


   



}

           















