﻿//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
// 
// Author: Xu Xiang
//------------------------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class Mojing : MonoBehaviour
{
    // Distinguish the stereo eyes.
    public enum Eye
    {
        Left,
        Center,
        Right,
        Mono
    }

    // The singleton instance of the Mojing class.
   public static Mojing SDK
    {
        get
        {
            if (sdk == null && ConfigItem.MojingSDKActive)
            {
				try
				{
                	sdk = UnityEngine.Object.FindObjectOfType<Mojing>();
				}
				catch(Exception e)
				{
					Debug.Log (e.ToString ());
					sdk = null;
				}
            }
            if (sdk == null && ConfigItem.MojingSDKActive)
            {
                MojingLog.LogTrace("Creating Mojing SDK object");
                var go = new GameObject("Mojing");
                sdk = go.AddComponent<Mojing>();
                go.transform.localPosition = Vector3.zero;
            }
            return sdk;
        }
    }
   private static Mojing sdk = null;


   public string GlassesKey
   {
       get
       {
           return glassesKey;
       }
       set
       {
           MojingLog.LogTrace("Change glasses from " + glassesKey + " to " + value);
           if (value != glassesKey)
           {
               glassesKey = value;
               MojingSDK.Unity_ChangeMojingWorld(value, UseTimeWarp);
               bWaitForMojingWord = true;
               // NeedDistortion = MojingSDK.Unity_IsGlassesNeedDistortion();
               MojingLog.LogTrace("New glasses is " + glassesKey);
           }
       }
   }
   private string glassesKey = null;

   public bool VRModeEnabled
   {
       get
       {
           return vrModeEnabled;
       }
       set
       {
           foreach (MojingVRHead head in heads)
           {
               head.VRModeEnabled = value;
           }
           if (vrModeEnabled != value)
           {
               vrModeEnabled = value;
               if (value)
                   OnEnable();
               else
                   OnDisable();
           }
       }
   }
   [SerializeField]
   private static bool vrModeEnabled = true;

   private MojingVRHead[] heads = null;

    // If the glasses need do image distortion
   public bool NeedDistortion
   {
       get
       {
           return needDistortion || UseTimeWarp;
       }
       set
       {
           MojingLog.LogTrace("needDistortion = " + value.ToString());
           {
               needDistortion = value;
               if (needDistortion)
                   MojingLog.LogTrace(glassesKey + " need distortion.");
               else
                   MojingLog.LogTrace(glassesKey + " DO NOT need distortion.");
               MojingLog.LogTrace("Leave setNeedDistortion");
           }
       }
   }
   private bool needDistortion = true;

   public bool bWaitForMojingWord = false;

   public bool UseTimeWarp
   {
       get
       {
           return ConfigItem.bUseTimeWarp;
       }
       set
       {
           ConfigItem.bUseTimeWarp = value;
       }
   }

   private bool bDuplicateMojing = false;

   private Manufacturers manufacturers_list;
   private Products product_list;
   public static Glasses glasses_list;
   public static List<string> glassesKeyList = new List<string>();
   public static List<string> glassesNameList = new List<string>();
   private string resetID;
   void Awake()
   {
        m_bResetTextureId = true;
        if (sdk == null)
        {
           MojingLog.LogWarn("Mojing SDK object is not sets.");
           sdk = this;
#if !UNITY_EDITOR && UNITY_IOS
			MojingSDK.Unity_Init("C3845343263589043","3908040197953231","8b20789122bae89442edadf780250ad5","www.mojing.cn");

            /*
            mode:   0 --- default value, Send Axis's direction (LEFT/RIGHT/UP/DOWN/CENTER) 
                    1 --- Send Axis's position (x,y,z)
                    2 --- Send both Axis's direction and postion
            */
            MojingSDK.Unity_SetGamePadAxisMode(0);

			MojingSDK.Unity_OnSurfaceChanged(Screen.width, Screen.height);
#endif
            MojingSDK.Unity_SetEngineVersion("Unity " + Application.unityVersion);
           //DontDestroyOnLoad(sdk);  //Remain Mojing GameObject even when change Scenes，just for Android

       }
       if (sdk != this)
       {
           MojingLog.LogWarn("Mojing SDK object should be a singleton.");
           bDuplicateMojing = true;
           enabled = false;
           return;
       }

       try
       {
           //清除Glasses列表
           glassesNameList.Clear();
           glassesKeyList.Clear();

           CreateDummyCamera();
           Application.targetFrameRate = 60;
           //MojingRender.StereoScreen = null;

           //解析json文件中的glass列表，获取glassesKeyList
          manufacturers_list = MojingSDK.GetManufacturers("zh");
          foreach (ManufacturerInfo MI in manufacturers_list.ManufacturerList)
          {
              product_list = MojingSDK.GetProducts(MI.KEY, "zh");
              foreach (ProductInfo PI in product_list.ProductList)
              {
                  glasses_list = MojingSDK.GetGlasses(PI.KEY, "zh");
                  foreach (GlassInfo GI in glasses_list.GlassList)
                  {
                      string GlassName = MI.Display + " " + PI.Display + " " + GI.Display;
                      string GlassKey = GI.KEY;
                      glassesKey = GlassKey;   //获取初始glassKey，mojingvrhead awake中用
                      glassesKeyList.Add(GlassKey);
                      glassesNameList.Add(GlassName);
                  }
              }
          }
       }
       catch (Exception e)
       {
           MojingLog.LogError(e.ToString());
       }
       MojingLog.LogTrace("Leave Mojing.Awake");
   }

    private void CreateDummyCamera()
    {
        var go = gameObject;
        if (go.GetComponent<Camera>())
        {
            go = new GameObject("VR Dummy Camera");
            go.transform.parent = gameObject.transform;
        }
        var cam = go.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Nothing;
        cam.cullingMask = 0;
        cam.useOcclusionCulling = false;
        cam.depth = 100;
    }

    public void OnSwitch(string statue)
    {
        switch(statue)
        {
            case "PAUSE":
                OnDisable();
                break;

            case "RESUME":
               // MojingRender.StereoScreen = null;
                OnEnable();
                break;
        }
    }

    public Camera getMainCamera()
    {
        foreach (MojingVRHead head in heads)
        {
            Camera camera = head.getMainCamera();
            if (camera != null)
                return camera;
        }
        return null;
    }

    private bool m_bResetTextureId = true;
    private int m_ResetCount = 8;
    //Get Texture ID when Awake/OnApplicationFocus/OnSurfaceChanged
    public void ResetTextureID(string resetFlag)
    {
        if (resetFlag == "textureReset")
        {
            m_bResetTextureId = true;
            m_ResetCount = 8;
        }
    }


   private bool updated = false;
   private int _Last_left_TextureID = 0;
   private int _Last_right_TextureID = 0;
   private int tID = 0;
   private int sID = 0;
   //private long frameCount = 0;
   IEnumerator EndOfFrame()
   {
	   //frameCount = 0;
       MojingLog.LogTrace("Enter Mojing.EndOfFrame thread");
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if ((!bWaitForMojingWord) && VRModeEnabled && NeedDistortion)
            {
                if (m_bResetTextureId)
                {
                    int tID = MojingRender.StereoScreen[0].GetNativeTextureID();
                    int sID = MojingRender.StereoScreen[1].GetNativeTextureID();
                    if (tID == 0 || sID == 0)
                    {
                        // Skip a frame, the old texture id is invalid and no new texture id to use
                        //MojingLog.LogTrace("Skip a frame since texture id is zero: " + tID + "," + sID);
                        m_bResetTextureId = true;
                    }
                    else
                    {
                        if(_Last_left_TextureID != tID || _Last_right_TextureID != sID)
                        {
                            _Last_left_TextureID = tID;
                            _Last_right_TextureID = sID;
                        }
                        //MojingLog.LogTrace("Set Texture ID: " + _Last_left_TextureID + "," + _Last_right_TextureID);
                        MojingSDK.SetTextureID(_Last_left_TextureID, _Last_right_TextureID);
                        MojingSDK.Unity_DistortFrame();
                        if (--m_ResetCount == 0)
                        {
                            m_bResetTextureId = false;
                        }
                    }
                }
                else
                {
                    //MojingLog.LogTrace("Read Texture ID = " + tID + ", " + sID + " while set Texture ID: " + _Last_left_TextureID + "," + _Last_right_TextureID);
                    MojingSDK.SetTextureID(_Last_left_TextureID, _Last_right_TextureID);
                    MojingSDK.Unity_DistortFrame();
                }
            }
       }
       //MojingLog.LogTrace("Leave Mojing.EndOfFrame thread");
   }

   // need to account for the limits due to screen size.
    public class Lens
    {
        public void UpdateProfile()
        {
            MojingLog.LogTrace("Mojing.Lens.UpdateProfile");
            FOV = MojingSDK.Unity_GetGlassesFOV();
            separation = MojingSDK.Unity_GetGlassesSeparation();
        }
        public float separation;
        public float lowerOffset;    // Offset of lens center from bottom.
        public float screenDistance; // Distance from lens center to the phone screen.
        public float FOV;
    }
    public Lens lens = new Lens();

    public class Mobile
    {
        public int nWidth;
        public int nHeight;
        public float width;   // The long edge of the phone.
        public float height;  // The short edge of the phone.
        public float border;  // Distance from bottom of the glasses to the bottom edge of screen.
        public void UpdateProfile()
        {
            MojingLog.LogTrace("Screen size old: " + width + " x " + height);
            MojingSDK.Unity_GetScreenSize(ref width, ref height);
            MojingLog.LogTrace("Screen size new: " + width + " x " + height);
            border = 0;
        }
    }
    public Mobile mobile = new Mobile();
    private void UpdateProfile()
    {
        lens.UpdateProfile();
        mobile.UpdateProfile();
    }

    public Pose3D headPose = new Pose3D();
    private Matrix4x4 headView = new Matrix4x4();

//#if UNITY_EDITOR
    // Simulated neck model.
    private static readonly Vector3 neckOffset = new Vector3(0, 0.075f, -0.08f);
    
    // Use mouse to emulate head in the editor.
  private float mouseX = 0;
  private float mouseY = 0;
  private float mouseZ = 0;
//#endif

  private float neckModelScale = 0.0f;
  // Mock settings for in-editor emulation while playing.
  public bool autoUntiltHead = true;
  
    public void UpdateState()
    {
        try
        {
#if UNITY_EDITOR
            Quaternion rot;
            bool rolled = false;
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                mouseX += Input.GetAxis("Mouse X") * 5;
                if (mouseX <= -180)
                {
                    mouseX += 360;
                }
                else if (mouseX > 180)
                {
                    mouseX -= 360;
                }
                mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
                mouseY = Mathf.Clamp(mouseY, -85, 85);
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                rolled = true;
                mouseZ += Input.GetAxis("Mouse X") * 5;
                mouseZ = Mathf.Clamp(mouseZ, -85, 85);
            }
            if (!rolled && autoUntiltHead)
            {
                // People don't usually leave their heads tilted to one side for long.
                mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
            }
            rot = Quaternion.Euler(mouseY, mouseX, mouseZ);
            var neck = (rot * neckOffset - neckOffset.y * Vector3.up) * neckModelScale;
            headPose.Set(neck, rot);
#else
            MojingSDK.Unity_getLastHeadView(ref headView);
            // Debug.Log("-----headView" + headView.ToString());

#if UNITY_ANDROID
            headPose.SetRightHanded(headView);
#elif UNITY_IOS
            headPose.Set(headView);
#endif
#endif
        }
        catch (Exception e)
        {
            MojingLog.LogError(e.ToString());
        }
    }

    void OnEnable()
    {
        MojingLog.LogTrace("Enter Mojing.OnEnable");
        if (VRModeEnabled)
        {
            MojingSDK.Unity_EnterMojingWorld(GlassesKey, UseTimeWarp);
        }

#if UNITY_IOS
		MojingSDK.Unity_StartTracker (100);
#endif

        bWaitForMojingWord = true;
        heads = FindObjectsOfType<MojingVRHead>();
        //frameCount = 0;
        StartCoroutine("EndOfFrame");
        MojingLog.LogTrace("Leave Mojing.OnEnable  ");
	}

     void Update()
    {
        if (bWaitForMojingWord)
        {
            try
            {
                if (MojingSDK.Unity_IsInMojingWorld(sdk.GlassesKey))
                {
                    bWaitForMojingWord = false;
                    MojingSDK.SetCenterLine();
                    NeedDistortion = MojingSDK.Unity_IsGlassesNeedDistortion();
                    UpdateProfile();
                    VRModeEnabled = vrModeEnabled;
				}
            }
            catch (Exception e)
            {
                MojingLog.LogError(e.ToString());
            }
        }
    }

    void OnDisable()
    {
        MojingLog.LogTrace("Enter Mojing.OnDisable");
        try
        {
            if (!bDuplicateMojing)
            {
                MojingSDK.Unity_LeaveMojingWorld();
                StopCoroutine("EndOfFrame");
            }
			//#if UNITY_IOS
			//MojingSDK.Unity_StopTracker();
			//#endif
        }
        catch (Exception e)
        {
            MojingLog.LogError(e.ToString());
        }
        MojingLog.LogTrace("Leave Mojing.OnDisable");
    }

    void OnLevelWasLoaded(int level)
    {
        try
        {
            MojingLog.LogTrace("Enter Mojing.OnLevelWasLoaded");

            if (!bDuplicateMojing)
            {
                heads = FindObjectsOfType<MojingVRHead>();
                VRModeEnabled = vrModeEnabled;
            }
        }
        catch (Exception e)
        {
            MojingLog.LogError(e.ToString());
        }
    }

    void OnDestroy()
    {
        MojingLog.LogTrace("Enter Mojing.OnDestroy");
        if (sdk == this)
        {
            sdk = null;
        }
        MojingLog.LogTrace("Leave Mojing.OnDestroy");
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            ResetTextureID("textureReset");
        }
    }
}
