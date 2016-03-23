//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
// 
// Author: Xu Xiang
//------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;

public class MojingRender
{

    // The texture that Unity renders the scene to do the distortion. 
    // If no need to do distortion, then StereoScreen shall be null.

    private static RenderTexture[] stereoScreen = new RenderTexture[2];
    private static RenderTexture dscreen = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
    private static RenderTexture[] defaultScreen = { dscreen, dscreen };
    public static RenderTexture[] StereoScreen
    {
        get
        {
            for (int i = 0; i < 2; i++)
            {
                // Don't need it except for distortion correction.
                if (!Mojing.SDK.VRModeEnabled || (!Mojing.SDK.NeedDistortion) || Application.isEditor)
                {
                    return defaultScreen;
                }
                if (stereoScreen[i] == null && Mojing.SDK.NeedDistortion)
                {
                    int size = MojingSDK.Unity_GetTextureSize();
                    MojingLog.LogTrace("Creating new default screen texture with " + size.ToString() + " Pixels");

                    stereoScreen[i] = new RenderTexture(size, size, 24, RenderTextureFormat.Default);
                    stereoScreen[i].anisoLevel = 0;
                    stereoScreen[i].antiAliasing = Mathf.Max(QualitySettings.antiAliasing, 1);
                }
            }
            return stereoScreen;
        }
        set
        {
            if (value == stereoScreen)
            {
                return;
            }
            if (!Mojing.SDK.NeedDistortion && value != null)
            {
                MojingLog.LogError("Can't set StereoScreen: No distortion correction is needed.");
                return;
            }
            if (stereoScreen != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    stereoScreen[i].Release();
                }
            }
            stereoScreen = value;
        }
    }

}
