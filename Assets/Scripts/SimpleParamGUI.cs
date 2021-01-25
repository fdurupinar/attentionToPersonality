

using UnityEngine;

/// <summary>
/// GUI functions to run selected animation
/// </summary>
public class SimpleParamGUI : GUIController {


    
    private DriveParams[] _driveParams = new DriveParams[32];

    
    private AnimationInfo[] _agentScripts;

    
    private void Start() {

        _agentScripts = transform.GetComponentsInChildren<AnimationInfo>();


        for(int i = 0; i < 32; i++) {
            _driveParams[i] = new DriveParams();
        }



        for(int i = 0; i < 32; i++)
            _driveParams[i].ReadValuesDrives(i);


        foreach(AnimationInfo t in _agentScripts)
            Reset(t);



    }



    private void OnGUI() {


        GUI.color = Color.white;

        GUILayout.BeginArea(new Rect(5, 20, 205, Screen.height));

        if(GUILayout.Button("Play"))
            PlayAgents();

        
        GUILayout.EndArea();

    }


    void PlayAgents() {
        

        foreach (AnimationInfo t in _agentScripts) {

            ResetComponents(t);
          
            Play(t);                        
        
        }

        
    }


}

