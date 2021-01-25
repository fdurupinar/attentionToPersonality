
using System;
using UnityEngine;
using System.Collections;
using System.IO;


/// <summary>
/// Class to keep and update motion parameter values for each of the 32 Drives
/// </summary>
public class DriveParams {

    const string driveFileName = "Assets/Resources/drives.txt";

    //Timing
    public  float Speed = 1f;
    public  float V0 = 0f;
    public  float V1 = 0f;
    public  float Ti = 0.5f;
    public  float Texp = 1.0f;
    public  float Tval = 0f;
    public  float Continuity = 0f;
    public  float T0 = 0f;
    public  float T1 = 1f;
    public float GoalThreshold = 0f;
    //Flourishes
    public  float TrMag = 0f; //torso rotation
    public  float TfMag = 0f;

    public int FixedTarget = 0; //for direct head movement
    public  float HrMag = 0f; //head rotation
    public  float HfMag = 0f;
    public int HSign; //sign of hr magnitude
    public  int ExtraGoal = 0;
    public  int UseCurveKeys = 0;


    public  float SquashMag = 0f;
    public  float SquashF = 0f; //breah frequench
    public  float WbMag = 0f;
    public  float WxMag = 0f;
    public  float WtMag = 0f;
    public  float WfMag = 0f;
    public  float EtMag = 0f;
    public  float DMag = 0f;
    public  float EfMag = 0f;

    //Shape for drives
    public  float EncSpr0 = 0f;
    public  float SinRis0 = 0f;
    public  float RetAdv0 = 0f;

    public  float EncSpr1 = 0f;
    public  float SinRis1 = 0f;
    public  float RetAdv1 = 0f;

    public float EncSpr2 = 0f;
    public float SinRis2 = 0f;
    public float RetAdv2 = 0f;

    public  float ShapeTi = 0f;

    
    //Arm shape for drives
    public  Vector3[] Arm = new Vector3[2];

    public DriveParams() {
        ResetDriveParameters();
    
    }

    public void ResetDriveParameters() {
        Speed = 1f;
        Tval = 0;
        V0 = -1f;
        V1 = -1f;
        Ti = 0.5f;
        T0 = 0.0f;
        T1 = 1f;
        Texp = 1f;
        GoalThreshold = 0f;
        Tval = 0f;
        Continuity = 0f;        
        TrMag = 0f;
        TfMag = 0f;
        FixedTarget = 0;
        HrMag = 0f;
        HfMag = 0f;
        HSign = 1;
        ExtraGoal = 0;
        UseCurveKeys = 0;
        SquashMag = 0f;
        SquashF = 0f;
        WbMag = 0f;
        WxMag = 0f;
        WtMag = 0f;
        WfMag = 0f;
        EtMag = 0f;
        EfMag = 0f;
        DMag = 0f;
        EncSpr0 = 0f;
        SinRis0 = 0f;
        RetAdv0 = 0f;
        EncSpr1 = 0f;
        SinRis1 = 0f;
        RetAdv1 = 0f;
        EncSpr2 = 0f;
        SinRis2 = 0f;
        RetAdv2 = 0f;
        Arm[0] = Vector3.zero;
        Arm[1] = Vector3.zero;

    }

    public void ReadValuesDrives(int driveInd) {
        ReadValuesDrives(driveInd, driveFileName);

    }


    public void ReadValuesDrives(int driveInd, string fileName) {
        
        string[] content = File.ReadAllLines(fileName);

        String[] tokens = content[driveInd + 1].Split('\t');

        int i = 2;
        Speed = float.Parse(tokens[i++]);
        V0 = float.Parse(tokens[i++]);
        V1 = float.Parse(tokens[i++]);
        Ti = float.Parse(tokens[i++]);
        Texp = float.Parse(tokens[i++]);
        Tval = float.Parse(tokens[i++]);
        T0 = float.Parse(tokens[i++]);
        T1 = float.Parse(tokens[i++]);        
        HrMag = float.Parse(tokens[i++]);
        HSign = HrMag >= 0 ? 1 : -1;
        HfMag = float.Parse(tokens[i++]);
        SquashMag = float.Parse(tokens[i++]);
        WbMag = float.Parse(tokens[i++]);
        WxMag = float.Parse(tokens[i++]);
        WtMag = float.Parse(tokens[i++]);
        WfMag = float.Parse(tokens[i++]);
        EtMag = float.Parse(tokens[i++]);
        EfMag = float.Parse(tokens[i++]);
        DMag = float.Parse(tokens[i++]);
        TrMag = float.Parse(tokens[i++]);
        TfMag = float.Parse(tokens[i++]);
        EncSpr0 = float.Parse(tokens[i++]);
        SinRis0 = float.Parse(tokens[i++]);
        RetAdv0 = float.Parse(tokens[i++]);
        EncSpr1 = float.Parse(tokens[i++]);
        SinRis1 = float.Parse(tokens[i++]);
        RetAdv1 = float.Parse(tokens[i++]);
        Continuity = float.Parse(tokens[i++]);
        Arm[0].x = float.Parse(tokens[i++]);
        Arm[0].y = float.Parse(tokens[i++]);
        Arm[0].z = float.Parse(tokens[i++]);
        Arm[1].x = float.Parse(tokens[i++]);
        Arm[1].y = float.Parse(tokens[i++]);
        Arm[1].z = float.Parse(tokens[i++]);        
        ExtraGoal = int.Parse(tokens[i++]);
        UseCurveKeys = int.Parse(tokens[i++]);
        SquashF = float.Parse(tokens[i++]);
        FixedTarget = int.Parse(tokens[i++]);
        EncSpr2 = float.Parse(tokens[i++]);
        SinRis2 = float.Parse(tokens[i++]);
        RetAdv2 = float.Parse(tokens[i++]);
        ShapeTi = float.Parse(tokens[i++]);        
        GoalThreshold = float.Parse(tokens[i++]);

    }

    
}
