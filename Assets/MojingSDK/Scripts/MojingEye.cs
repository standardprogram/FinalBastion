//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
// 
// Author: Xu Xiang
//------------------------------------------------------------------------------

using UnityEngine;
using System.Reflection;
using System;

[RequireComponent(typeof(Camera))]
public class MojingEye : MonoBehaviour
{
    // Whether this is the left eye or the right eye, or the mono eye.
    public Mojing.Eye eye;

    // The stereo controller in charge of this eye (and whose mono camera
    // we will copy settings from).
    private MojingVRHead head = null;
    public MojingVRHead Head
    {
        // This property is set up to work both in editor and in player.
        get
        {
            if (transform.parent == null)
            { 
                // Should not happen.
                return null;
            }
            if (head == null)
            {
                head = transform.parent.GetComponentInParent<MojingVRHead>();
            }
            return head;
        }
    }

    public bool VRModeEnabled
    {
        get
        {
            return vrModeEnabled;
        }
        set
        { 
            vrModeEnabled = value;
            UpdateVrMode();
        }
    }
    [SerializeField]
    private bool vrModeEnabled = true;

    public void UpdateVrMode()
    {
        try 
        {
            switch (eye)
            {
                case Mojing.Eye.Left:
                    if (!vrModeEnabled)
                        EnableEye(false);
                    else
                        EnableEye(true);
                    break;

                case Mojing.Eye.Right:
                    if (!vrModeEnabled)
                        EnableEye(false);
                    else
                        EnableEye(true);
                    break;

                case Mojing.Eye.Center:
                    if (!vrModeEnabled)
                        EnableEye(true);
                    else
                        EnableEye(false);
                    break;

                case Mojing.Eye.Mono:
                    EnableEye(true);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log( e.ToString());
        }
    }
    
    public void EnableEye(bool enable)
    {
        MojingLog.LogTrace("Enable Camera " + eye.ToString() + ": " + enable.ToString());
        enabled = enable;
        if (eye == Mojing.Eye.Left || eye == Mojing.Eye.Right)
        {
            // Setup FOV
			GetComponent<Camera>().fieldOfView = MojingSDK.Unity_GetGlassesFOV();
            
            //*****Solve the problem of splash screen when start up
            if (enable)
            {
                if (Mojing.SDK.NeedDistortion)
                {
#if !UNITY_EDITOR
                    switch (eye)
                    {
                        case Mojing.Eye.Left:
                            GetComponent<Camera>().targetTexture = MojingRender.StereoScreen[0];
                            break;
                        case Mojing.Eye.Right:
                            GetComponent<Camera>().targetTexture = MojingRender.StereoScreen[1];
                            break;
                    }
#endif
                }
                else
                {
                    GetComponent<Camera>().targetTexture = null;
                }
            }
            //*****
        }
		GetComponent<Camera>().enabled = enable;
    }

    void Start()
    {
        var ctlr = Head;
        if ((ctlr == null) && (eye != Mojing.Eye.Mono))
        {
            Debug.LogError("MojingEye must be child of a MojingVRHead.");
            enabled = false;
        }
        SetUpEye();
    }

    public void OnPreCull()
    {
        if (Mojing.SDK.bWaitForMojingWord)
        {
            EnableEye(false);
            return;
        }
		if ( GetComponent<Camera>() != null)
        {
            SetUpEye();
        }
        else 
        {
            MojingLog.LogError(eye.ToString() + ": no camera found.");
        }
    }

    public void SetUpEye()
    {
        // Do not change any settings of Center Camera except localtion
        if (eye == Mojing.Eye.Center)
        {
            transform.localPosition = 0 * Vector3.right;
        }
        else
        {
            // Setup the rect & transform
            Rect rect = new Rect(0, 0, 1, 1);
            float ipd = Mojing.SDK.lens.separation; // *controller.stereoMultiplier;
#if UNITY_EDITOR
            switch (eye)
            {
                case Mojing.Eye.Left:
                    rect.width = 0.5f;
                    transform.localPosition = (-ipd / 2) * Vector3.right;
                    break;

                case Mojing.Eye.Right:
                    rect.x = 0.5f;
                    rect.width = 0.5f;
                    transform.localPosition = (ipd / 2) * Vector3.right;
                    break;
            }
#else
            switch (eye)
            {
                case Mojing.Eye.Left:
                    if (Mojing.SDK.NeedDistortion)
                    {
                        rect.width = 1.0f;
                    }
                    else
                    {
                        rect.width = 0.5f;
                    }
                    transform.localPosition = (-ipd / 2) * Vector3.right;
                    break;

                case Mojing.Eye.Right:
                    if (Mojing.SDK.NeedDistortion)
                    {
                        rect.width = 1.0f;
                    }
                    else
                    {
                        rect.x = 0.5f;
                        rect.width = 0.5f;
                    }
                    transform.localPosition = (ipd / 2) * Vector3.right;
                    break;
            }
#endif
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            GetComponent<Camera>().rect = rect;
        }
    }
}
