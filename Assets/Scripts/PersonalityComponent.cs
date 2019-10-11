using UnityEngine;

public enum OCEAN {
    O,
    C,
    E,
    A,
    N
}
/// <summary>
/// Stores personality and Effort values per agent
/// </summary>
public class PersonalityComponent : MonoBehaviour {
    public float[] Personality  = new float[5];
   
    public float [] Effort = new float[4];
    
    
    void Start () {

        for (int i = 0; i < Personality.Length; i++)
            Personality[i] = 0;

    }

	
}
