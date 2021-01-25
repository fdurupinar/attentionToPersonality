
using System;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GUI functions to tune character personality
/// </summary>
public class SimpleParamGUI : GUIController {
    RaycastHit _hit;

    int _agentSelInd;



    private float[] _personality = new float[5]; //-1 0 1 
    private string[] _personalityName = { "O", "C", "E", "A", "N" };

    private int _persMin = -1;
    private int _persMax = 1;

    private DriveParams[] _driveParams = new DriveParams[32];

    private PersonalityMapper _persMapper;

    private AnimationInfo[] _agentScripts;


    private DropDownRect _dropDownRectAnimNames;
    private DropDownRect _dropDownRectAgents;

    


    private bool _lockHand = false;


    private void Start() {


        _agentScripts = transform.GetComponentsInChildren<AnimationInfo>();


        _dropDownRectAgents = new DropDownRect(new Rect(115, 20, 90, 300));
        _dropDownRectAnimNames = new DropDownRect(new Rect(210, 20, 90, 300));

        for(int i = 0; i < 32; i++) {
            _driveParams[i] = new DriveParams();
        }



        for(int i = 0; i < 32; i++)
            _driveParams[i].ReadValuesDrives(i);



        _agentSelInd = 0;


        foreach(AnimationInfo t in _agentScripts)
            Reset(t);


        MathDefs.SetSeed(30);

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

    void StopAgents() {
        foreach(AnimationInfo t in _agentScripts) {
            StopAnimations(t);

        }
    }

    

}

