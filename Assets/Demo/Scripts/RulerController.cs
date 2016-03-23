//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
//------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class RulerController : MonoBehaviour
{
    public GameObject rulerX;
    public GameObject rulerY;

    int z = 0;

	void Update () {
        Quaternion rot = Mojing.SDK.headPose.Orientation;
        rulerX.transform.localEulerAngles = new Vector3(0,  90, -rot.eulerAngles.x);
        rulerY.transform.localEulerAngles = new Vector3(270, -rot.eulerAngles.y, 0);
        z++;
	}	
}
