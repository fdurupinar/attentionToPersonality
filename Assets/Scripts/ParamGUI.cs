
using System;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// GUI functions to tune character personality
/// </summary>
public class ParamGUI : GUIController {
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

    bool _firstRun = true;


    private bool _lockHand = false;


    private void Start() {


        _agentScripts = transform.GetComponentsInChildren<AnimationInfo>();


        _dropDownRectAgents = new DropDownRect(new Rect(115, 20, 90, 300));
        _dropDownRectAnimNames = new DropDownRect(new Rect(210, 20, 90, 300));

        for (int i = 0; i < 32; i++) {
            _driveParams[i] = new DriveParams();
        }



        for (int i = 0; i < 32; i++)
            _driveParams[i].ReadValuesDrives(i);


        _firstRun = true;



        _agentSelInd = 0;



        _persMapper = new PersonalityMapper();




        foreach (AnimationInfo t in _agentScripts)
            Reset(t);

        //   FormatData("motionEffortCoefs.txt");

        MathDefs.SetSeed(30);

    }



    void Update() {

        if (_firstRun) {
            _persMapper.ComputeMotionEffortCoefs(_driveParams);

            _firstRun = false; //no need to compute again

        }


        else if (Input.GetKeyDown("0"))
            Time.timeScale = 0;
        else if (Input.GetKeyDown("1"))
            Time.timeScale = 1f;
        else if (Input.GetKeyDown("2"))
            Time.timeScale = 2f;
        else if (Input.GetKeyDown("3"))
            Time.timeScale = 3f;
        else if (Input.GetKeyDown("4"))
            Time.timeScale = 4f;

        else if (Input.GetKeyDown("s")) {
            GameObject.Find("Camera").GetComponent<Screenshot>().IsRunning =
                !GameObject.Find("Camera").GetComponent<Screenshot>().IsRunning;

            if (GameObject.Find("Camera").GetComponent<Screenshot>().IsRunning) {
                Time.timeScale = 0.5f;
                RecordPersonalities();
            }
            else
                Time.timeScale = 1f;

        }
        else if (Input.GetKeyDown("l"))
            LoadPersonalities();

        //Show keypoints
        else if (Input.GetKeyDown("g")) {
            Debug.Log(_agentScripts[_agentSelInd].CharacterName + " " +
                      _agentScripts[_agentSelInd].CurrKeyInd);

        }


        if (Input.GetMouseButtonDown(0)) {


            if (Camera.main != null) {


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out _hit);
                if (_hit.collider) {
                    for (int i = 0; i < _agentScripts.Length; i++) {
                        if (_hit.collider.transform.parent && _agentScripts[i].gameObject.Equals(_hit.collider.transform.parent.gameObject)) {
                            _agentSelInd = i;

                            break;
                        }

                    }
                }

            }


        }

    }


    private void OnGUI() {


        GUI.color = Color.white;
        GUILayout.BeginArea(_dropDownRectAgents.DdRect);
        GUILayout.Label("Character");
        _dropDownRectAgents.DdList = _agentScripts.Select(s => s.CharacterName).ToList();
        _agentSelInd = _dropDownRectAgents.ShowDropDownRect();

        GUILayout.EndArea();


        GUILayout.BeginArea(_dropDownRectAnimNames.DdRect);
        GUILayout.Label("Animation");
        _dropDownRectAnimNames.DdList = _agentScripts[_agentSelInd].AnimNames.ToList();
        int ind = _dropDownRectAnimNames.ShowDropDownRect();
        _agentScripts[_agentSelInd].AnimName = _agentScripts[_agentSelInd].AnimNames[ind];
        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(5, 20, 105, Screen.height));


        GUILayout.Label("Personality");
        GUI.color = Color.white;

        GUILayout.Label("");
        for (int i = 0; i < 5; i++) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("" + _persMin);
            GUILayout.Label("" + _personalityName[i]);
            GUILayout.Label("" + _persMax);
            GUILayout.EndHorizontal();

            _personality[i] = _agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Personality[i];
            GUI.color = Color.white;



            GUI.backgroundColor = Color.white;
            _personality[i] = GUILayout.HorizontalSlider(_personality[i], _persMin, _persMax).Truncate(1);

            //Assign agent personality
            _agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Personality[i] = _personality[i];


            string[] ocean = { "O", "C", "E", "A", "N" };
            for (int j = 0; j < 5; j++) {
                if (_agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Personality[j] == -1)
                    ocean[j] += "-";
                else if (_agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Personality[j] == 1)
                    ocean[j] += "+";
                else
                    ocean[j] = "";
            }


        }


        GUI.color = Color.white;


        _lockHand = GUILayout.Toggle(_lockHand, "Lock hand");
        foreach (AnimationInfo a in _agentScripts)
            a.GetComponent<IKAnimator>().LockHand = _lockHand;


        if (GUILayout.Button("Reset scene"))
            SceneManager.LoadScene("MotionSelection");

        if (GUILayout.Button("Randomize")) {
            foreach (AnimationInfo a in _agentScripts)
                for (int i = 0; i < 5; i++)
                    a.GetComponent<PersonalityComponent>().Personality[i] = MathDefs.GetRandomNumber(-1f, 1f);

        }

        if (GUILayout.Button("Reset")) {
            foreach (AnimationInfo a in _agentScripts)
                for (int i = 0; i < 5; i++)
                    a.GetComponent<PersonalityComponent>().Personality[i] = 0;
        }

        if (GUILayout.Button("Assign All")) {
            foreach (AnimationInfo a in _agentScripts)
                for (int i = 0; i < 5; i++)
                    a.GetComponent<PersonalityComponent>().Personality[i] = _personality[i];


        }

        if (GUILayout.Button("Assign All Variation")) {
            foreach (AnimationInfo a in _agentScripts)
                for (int i = 0; i < 5; i++) {
                    a.GetComponent<PersonalityComponent>().Personality[i] = _personality[i] +
                                                                            MathDefs.GetRandomNumber(-0.2f, 0.2f);
                    if (a.GetComponent<PersonalityComponent>().Personality[i] > 1)
                        a.GetComponent<PersonalityComponent>().Personality[i] = 1;
                    else if (a.GetComponent<PersonalityComponent>().Personality[i] < -1)
                        a.GetComponent<PersonalityComponent>().Personality[i] = -1;
                }
        }

        if (GUILayout.Button("Play"))
            PlayAgents();



        if (GUILayout.Button("Record")) {
            GameObject.Find("Main Camera").GetComponent<Screenshot>().IsRunning = true;

            Time.timeScale = 0.25f;

            PlayAgents();

        }



        //we need to update after play because playanim resets torso parameters for speed etc. when animinfo is reset

        GUI.color = Color.yellow;
        GUILayout.Label("S:" + _agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Effort[0] + " W:" + _agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Effort[1] + " T:" +
            _agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Effort[2] + " F:" + _agentScripts[_agentSelInd].GetComponent<PersonalityComponent>().Effort[3]);

        GUILayout.EndArea();

    }

    void PlayAgents() {
        int agentInd = 0;


        foreach (AnimationInfo t in _agentScripts) {

            ResetComponents(t);


            _persMapper.MapPersonalityToMotion(t.GetComponent<PersonalityComponent>()); //calls initkeypoints, which stops the animation

            Play(t);

            GUI.color = Color.white;
            agentInd++;
        }


        _persMapper.MapAnimSpeeds(_agentScripts, 0.6f, 1.3f); //map them to the range
    }

    void RecordPersonalities() {
        using (StreamWriter sw = new StreamWriter("Assets/Resources/personalities.txt")) {
            foreach (AnimationInfo a in _agentScripts) {
                sw.WriteLine(a.CharacterName + "\t" + a.GetComponent<PersonalityComponent>().Personality[0] + "\t" +
                             a.GetComponent<PersonalityComponent>().Personality[1] + "\t" + a.GetComponent<PersonalityComponent>().Personality[2] + "\t" + a.GetComponent<PersonalityComponent>().Personality[3] + "\t" +
                             a.GetComponent<PersonalityComponent>().Personality[4]);
            }
        }
    }

    void LoadPersonalities() {
        using (StreamReader sr = new StreamReader("Assets/Resources/personalities.txt")) {
            foreach (AnimationInfo a in _agentScripts) {
                string s = sr.ReadLine();
                String[] p = s.Split('\t');
                for (int i = 0; i < 5; i++)
                    a.GetComponent<PersonalityComponent>().Personality[i] = float.Parse(p[i + 1]);

            }
        }

    }

}

