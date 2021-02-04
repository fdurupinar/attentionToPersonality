
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls arm flourishes and torso rotations
/// </summary>
public class FlourishAnimator : MonoBehaviour {
    public List<Vector3> HandLPrev;
    public List<Vector3> HandRPrev;


    private AnimationInfo _animInfo;
    private TorsoController _torso;

    //Low-level parameters
    public float WbMag; //Wrist bend multiplier
    public float WxMag; //Wrist extension magnitude

    public float DMag; //Displacement magnitude
    public float EtMag; //Elbow twist magnitude
    public float WtMag; //
    public float EfMag; //Elbow frequency magnitude
    public float WfMag; //


    public float TrMag; //Torso rotation magnitude
    public float TfMag; //Torso rotation frequency

    private bool _stopRotation;
    private float _lastTorsoAngle;
    //private float _distanceWalked = 0;

    public int LoopPlayCount;
    public Vector3 pos1, pos2;
    bool oneLastTime = false;


    private void Awake() {
      
        _animInfo = GetComponent<AnimationInfo>();
        _torso = GetComponent<TorsoController>();

        HandLPrev = new List<Vector3>();
        HandRPrev = new List<Vector3>();

    
        
    }


    public void Reset() {
        _stopRotation = false;
        _lastTorsoAngle = 0;

        LoopPlayCount = 0;
        oneLastTime = false;


    }

    public void ResetParameters() {

        TrMag = TfMag = 0f;       
        WbMag = 0f;
        WxMag = WtMag = WfMag = 0f;
        EtMag = DMag = EfMag = 0f;

    }

    //Has to be lateupdate because we overwrite the transforms
    private void LateUpdate() {

        float t;
        //t = _animInfo.NormalizedT;
        t = _animInfo.GlobalT;

        if(!GetComponent<Animation>().isPlaying || _animInfo.DisableLMA) {
            if(!oneLastTime)
                oneLastTime = true;
            else
                return;
           
            
        }
        
        
        if (t > 1)
            t = 1;


        //Torso rotation
        float torsoRot = TrMag * Mathf.Sin(TfMag * Mathf.PI * t);

        float begin = GetComponent<IKAnimator>().LockBegin;
        float end = GetComponent<IKAnimator>().LockEnd;
        float diff = end - begin;
        if (!_stopRotation && GetComponent<IKAnimator>().LockHand && t > begin && t < end ){//&& t < end) {
            
            _stopRotation = true;

            _lastTorsoAngle = torsoRot;
        }

        if (_stopRotation)
            torsoRot = _lastTorsoAngle;

       if (t >= end && GetComponent<IKAnimator>().LockHand) {
            _stopRotation = false;
            torsoRot = TrMag * Mathf.Sin(TfMag * Mathf.PI * (t - diff));
        }

        _torso.Spine.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * torsoRot, _torso.Root.up) * _torso.Spine.rotation;



        float breath = 1f - GetComponent<IKAnimator>().SquashMag * Mathf.Sin(GetComponent<IKAnimator>().SquashF * Mathf.PI * t) / 5f;
        Vector3 breathVec = new Vector3(1.05f / breath, 1f / breath, 1.05f / breath);

        if(breath != 0) {
            _torso.Spine1.localScale = breathVec;

            //correct children's scales
            for(int i = 0; i < _torso.Spine1.childCount; i++)
                _torso.Spine1.GetChild(i).localScale = new Vector3(1 / breathVec.x, 1 / breathVec.y, 1 / breathVec.z); //correct child
        }

        //Arm rotations        
        Flourish(0,  t);

        //Don't do flourishes if hand is locked
        if (GetComponent<IKAnimator>().LockHand ) {

         //   if(t <= end)
            _torso.Clavicle[1].rotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * torsoRot, _torso.Root.up) * _torso.Clavicle[1].rotation;
        }
        else
            Flourish(1,  t);



       //if (_animInfo.AnimName.ToUpper().Contains("WALK")) {
       //    float stepSize = _animInfo.AnimSpeed * Time.deltaTime;
       //    _distanceWalked += stepSize;
       //    transform.Translate(transform.forward * _distanceWalked);

           
       //}
              

    }

    
    
    //t = normalized time between initial and final keys
    private void Flourish(int ind, float t) {


        //rotate wrist around the x-axis in local coordinate system	(x-axis in EMOTE coordinate system)	
        //  if (WbMag == 0)  //FUNDA commented out
        //    wristBend = 0.6f; //In EMOTE
        //else
        float wristBend = WbMag * (Mathf.Sin(2f * WfMag * Mathf.PI * t) + 1f - WxMag);

        //rotation of wrist around the y-axis	(z-axis in EMOTE coordinate system)			
        float wristTwist = WtMag * Mathf.Sin(WfMag * Mathf.PI * t);



        //inverse elbows and wrists
        if (_animInfo.CharacterName.Contains("CHUCK")) {
            if (ind == 1)
                wristTwist *= -1;
            _torso.Wrist[ind].transform.rotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * wristBend, _torso.Wrist[ind].right) * _torso.Wrist[ind].transform.rotation;
            _torso.Wrist[ind].transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * wristTwist, _torso.Wrist[ind].up) * _torso.Wrist[ind].transform.rotation;


        }
        else {
            if (ind == 1)
                wristBend *= -1;

            _torso.Wrist[ind].transform.rotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * wristBend, _torso.Wrist[ind].forward) * _torso.Wrist[ind].transform.rotation;
            _torso.Wrist[ind].transform.rotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * wristTwist, _torso.Wrist[ind].right) * _torso.Wrist[ind].transform.rotation;

        }

        //rotate elbow  around the y-axis	(z-axis in EMOTE coordinate system)	

        float elbowTwist = EtMag * Mathf.Sin(EfMag * Mathf.PI * t);
        float elbowAngle = DMag * Mathf.Sin(EfMag * Mathf.PI * t);


        if (_animInfo.CharacterName.Contains("CHUCK")) {
            
            if (ind == 1)
                elbowTwist *= -1;

            if (ind == 1 && elbowAngle < 0 || ind == 0 && elbowAngle > 0)
                elbowAngle *= -1;

            _torso.Elbow[ind].transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * elbowAngle, _torso.Elbow[ind].forward) * _torso.Elbow[ind].transform.rotation;
            _torso.Elbow[ind].transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * elbowTwist, _torso.Elbow[ind].up) * _torso.Elbow[ind].transform.rotation;

        }
        else {
            if (ind == 1)
                elbowAngle *= -1;

            if (ind == 1 && elbowAngle > 0 || ind ==0 && elbowAngle < 0)
                elbowAngle *= -1;

            _torso.Elbow[ind].transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * elbowAngle, _torso.Elbow[ind].up) * _torso.Elbow[ind].transform.rotation;
            _torso.Elbow[ind].transform.rotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * elbowTwist, _torso.Elbow[ind].right) * _torso.Elbow[ind].transform.rotation;
            
            
        }

    }

    

}
