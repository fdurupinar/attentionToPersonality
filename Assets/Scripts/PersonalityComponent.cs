using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

public enum OCEAN {
    O,
    C,
    E,
    A,
    N
}

public class PersonalityComponent : MonoBehaviour {
    public float[] Personality  = new float[5];
   
    public float [] Effort = new float[4];
    

    public List<string>[] Adjectives = new List<string>[10]; //descriptive adjectives for each personality pole
    
    
    
    void Start () {

   

        for (int i = 0; i < Personality.Length; i++)
            Personality[i] = 0;


   
    }


    public string Effort2String() {
        string str = "";

        if (Effort[0] < 0)
            str += "Indirect";
        else if (Effort[0] > 0)
            str += "Direct";

        str += " ";
        if (Effort[1] < 0)
            str += "Light";
        else if (Effort[1] > 0)
            str += "Strong";

        str += " ";
        if (Effort[2] < 0)
            str += "Sustained";
        else if (Effort[2] > 0)
            str += "Sudden";

        str += " ";
        if (Effort[3] < 0)
            str += "Free";
        else if (Effort[3] > 0)
            str += "Bound";

        
        return str;

    }
  

	
}
