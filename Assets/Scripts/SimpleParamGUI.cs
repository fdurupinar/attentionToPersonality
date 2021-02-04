

using UnityEngine;

/// <summary>
/// GUI functions to run selected animation
/// </summary>
public class SimpleParamGUI : GUIController {


    
    private DriveParams[] _driveParams = new DriveParams[32];

    
    private AnimationInfo _animInfo;
    public int DriveInd;

    
    private void Start() {

        _animInfo = transform.GetComponentInChildren<AnimationInfo>();


        for(int i = 0; i < 32; i++) {
            _driveParams[i] = new DriveParams();
        }


        for(int i = 0; i < 32; i++)
            _driveParams[i].ReadValuesDrives(i, "Assets/Resources/drivesPick.txt");


        _animInfo.AnimName = "Picking";
        Reset(_animInfo);


        UpdateEmoteParams(_animInfo.gameObject, DriveInd);
        PlayAgents();

    }



    private void OnGUI() {


        GUI.color = Color.white;

        GUILayout.BeginArea(new Rect(5, 20, 205, 500));

        if(GUILayout.Button("Play"))
            PlayAgents();

        
        GUILayout.EndArea();

    }


    void PlayAgents() {
               
        ResetComponents(_animInfo);          
        Play(_animInfo);                        
    }


    void UpdateEmoteParams(GameObject agent, int driveInd) {
        if(agent == null) {
            Debug.Log("AgentPrefab not found");
            return;
        }


        agent.GetComponent<AnimationInfo>().AnimSpeed = _driveParams[driveInd].Speed;
        agent.GetComponent<AnimationInfo>().V0 = _driveParams[driveInd].V0;
        agent.GetComponent<AnimationInfo>().V1 = _driveParams[driveInd].V1;

        agent.GetComponent<AnimationInfo>().T0 = _driveParams[driveInd].T0;
        agent.GetComponent<AnimationInfo>().T1 = _driveParams[driveInd].T1;
        agent.GetComponent<AnimationInfo>().Ti = _driveParams[driveInd].Ti;


        agent.GetComponent<AnimationInfo>().Texp = _driveParams[driveInd].Texp;

        float prevTVal = agent.GetComponent<AnimationInfo>().Tval;
        float prevContinuity = agent.GetComponent<AnimationInfo>().Continuity;
        agent.GetComponent<AnimationInfo>().Tval = _driveParams[driveInd].Tval;
        agent.GetComponent<AnimationInfo>().Continuity = _driveParams[driveInd].Continuity;

        if(_driveParams[driveInd].Tval != prevTVal || _driveParams[driveInd].Continuity != prevContinuity)
            agent.GetComponent<AnimationInfo>().InitInterpolators(_driveParams[driveInd].Tval, _driveParams[driveInd].Continuity, 0);



        agent.GetComponent<FlourishAnimator>().TrMag = _driveParams[driveInd].TrMag;
        agent.GetComponent<FlourishAnimator>().TfMag = _driveParams[driveInd].TfMag;

        agent.GetComponent<IKAnimator>().HrMag = _driveParams[driveInd].HrMag;
        agent.GetComponent<IKAnimator>().HfMag = _driveParams[driveInd].HfMag;
        agent.GetComponent<AnimationInfo>().ExtraGoal = _driveParams[driveInd].ExtraGoal;


        agent.GetComponent<IKAnimator>().SquashMag = _driveParams[driveInd].SquashMag; //breathing affects keypoints
        agent.GetComponent<IKAnimator>().SquashF = _driveParams[driveInd].SquashF; //breathing affects keypoints

        agent.GetComponent<FlourishAnimator>().WbMag = _driveParams[driveInd].WbMag;
        agent.GetComponent<FlourishAnimator>().WxMag = _driveParams[driveInd].WxMag;
        agent.GetComponent<FlourishAnimator>().WfMag = _driveParams[driveInd].WfMag;
        agent.GetComponent<FlourishAnimator>().WtMag = _driveParams[driveInd].WtMag;
        agent.GetComponent<FlourishAnimator>().EfMag = _driveParams[driveInd].EfMag;
        agent.GetComponent<FlourishAnimator>().EtMag = _driveParams[driveInd].EtMag;
        agent.GetComponent<FlourishAnimator>().DMag = _driveParams[driveInd].DMag;


        agent.GetComponent<IKAnimator>().ShapeTi = _driveParams[driveInd].ShapeTi;

        agent.GetComponent<IKAnimator>().EncSpr[0] = _driveParams[driveInd].EncSpr0;
        agent.GetComponent<IKAnimator>().SinRis[0] = _driveParams[driveInd].SinRis0;
        agent.GetComponent<IKAnimator>().RetAdv[0] = _driveParams[driveInd].RetAdv0;

        agent.GetComponent<IKAnimator>().EncSpr[1] = _driveParams[driveInd].EncSpr1;
        agent.GetComponent<IKAnimator>().SinRis[1] = _driveParams[driveInd].SinRis1;
        agent.GetComponent<IKAnimator>().RetAdv[1] = _driveParams[driveInd].RetAdv1;

        agent.GetComponent<IKAnimator>().EncSpr[2] = _driveParams[driveInd].EncSpr2;
        agent.GetComponent<IKAnimator>().SinRis[2] = _driveParams[driveInd].SinRis2;
        agent.GetComponent<IKAnimator>().RetAdv[2] = _driveParams[driveInd].RetAdv2;


        agent.GetComponent<AnimationInfo>().UseCurveKeys = _driveParams[driveInd].UseCurveKeys;

        agent.GetComponent<AnimationInfo>().Hor = _driveParams[driveInd].Arm[0].x;
        agent.GetComponent<AnimationInfo>().Ver = _driveParams[driveInd].Arm[0].y;
        agent.GetComponent<AnimationInfo>().Sag = _driveParams[driveInd].Arm[0].z;
        agent.GetComponent<AnimationInfo>().UpdateKeypointsByShape(0); //Update keypoints

        //RightArm 
        //Only horizontal motion is the opposite for each arm
        agent.GetComponent<AnimationInfo>().Hor = -_driveParams[driveInd].Arm[1].x;
        agent.GetComponent<AnimationInfo>().Ver = _driveParams[driveInd].Arm[1].y;
        agent.GetComponent<AnimationInfo>().Sag = _driveParams[driveInd].Arm[1].z;
        agent.GetComponent<AnimationInfo>().UpdateKeypointsByShape(1); //Update keypoints


    }


}

