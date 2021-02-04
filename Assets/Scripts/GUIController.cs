using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class of GUIControllers
/// Controls play, pause, stop animations
/// </summary>

public class GUIController : MonoBehaviour {


    protected void Reset(AnimationInfo agent) {

        ResetComponents(agent);
        StopAtFirstFrame(agent);

    }

    protected void ResetComponents(AnimationInfo agent) {

        agent.GetComponent<Animation>().Stop();
        agent.GetComponent<FlourishAnimator>().Reset();
        agent.GetComponent<TorsoController>().Reset();
        agent.Reset(agent.AnimName);
        agent.GetComponent<IKAnimator>().Reset();

    }

    protected void StopAtFirstFrame(AnimationInfo agent) {

        if(!agent.GetComponent<Animation>().isPlaying)
            agent.GetComponent<Animation>().Play(agent.AnimName);

        agent.GetComponent<Animation>().clip.SampleAnimation(agent.gameObject, 0); //instead of rewind
        agent.GetComponent<Animation>().Stop();

    }

    protected void Play(AnimationInfo agent) {

        //agent.animation[agent.AnimName].wrapMode = WrapMode.ClampForever;
        agent.GetComponent<Animation>()[agent.AnimName].wrapMode = WrapMode.Once;//WrapMode.Clamp;//WrapMode.Once;//WrapMode.Loop;
        agent.GetComponent<Animation>().Play(agent.AnimName);
        agent.GetComponent<Animation>().enabled = true;

        //capture BVH
        //agent.GetComponent<BVHRecorder>().capturing = true;

    }

    protected void StopBVHCapturing(AnimationInfo agent) {
        //stop capturing BVH
        agent.GetComponent<BVHRecorder>().capturing = false;
        agent.GetComponent<BVHRecorder>().saveBVH();
    }

    protected void StopAnimations(AnimationInfo agent) {

        StopAtFirstFrame(agent);
        agent.GetComponent<TorsoController>().Reset();

        StopAtFirstFrame(agent);
        PlayAnim(agent); //start the next animation
        StopAtFirstFrame(agent);

        //stop capturing BVH
        StopBVHCapturing(agent);
    }


    protected void InitAgent(AnimationInfo agent) {

        agent.Reset(agent.AnimName);
        agent.GetComponent<IKAnimator>().Reset();
        agent.GetComponent<Animation>().enabled = true;
        agent.GetComponent<Animation>().Play(agent.AnimName);

    }


    protected void PlayAnim(AnimationInfo agent) {

        StopAtFirstFrame(agent); //stop first
        InitAgent(agent);

        agent.GetComponent<Animation>()[agent.AnimName].wrapMode = agent.IsContinuous ? WrapMode.Loop : WrapMode.ClampForever;

    }

}
