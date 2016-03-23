//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
//------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MojingSample.CrossPlatformInput;

public class InputManagerMenu : MonoBehaviour
{

    public MenuController menu_controller;
    public UIListController glass_controller;

    void Update()
    {
        if (CrossPlatformInputManager.GetButton("OK"))
        {
            Debug.Log("OK-----GetButtonDown");
        }
        else if (CrossPlatformInputManager.GetButtonUp("OK") /*|| CrossPlatformInputManager.GetButtonUp("Fire1")*/)
        {
            if (menu_controller != null && glass_controller != null)
            {
                if (UIListController.show_flag)//镜片选择的二级菜单
                    glass_controller.PressCurrent();
                else//场景选择的一级菜单
                    menu_controller.PressCurrent();
            }
        }

        if (CrossPlatformInputManager.GetButton("C"))
        {
            Debug.Log("C-----GetButtonDown");
        }
        else if (CrossPlatformInputManager.GetButtonUp("C"))
        {
#if IOS_DEVICE
				MojingSDK.Unity_StopTracker();
#endif
                Application.Quit();
        }

        if (CrossPlatformInputManager.GetButton("MENU"))
        {
            Debug.Log("MENU-----GetButtonDown");
        }
        else if (CrossPlatformInputManager.GetButtonUp("MENU"))
        {
            Debug.Log("MENU-----GetButtonUp");
        }

        if (CrossPlatformInputManager.GetButton("UP"))
        {
            Debug.Log("UP-----GetButtonDown");
        }

        else if (CrossPlatformInputManager.GetButtonUp("UP"))
        {
            if (menu_controller != null && glass_controller != null)
            {
                if (UIListController.show_flag)
                    glass_controller.HoverPrev();
                else
                    menu_controller.HoverPrev();
            }
        }

        if (CrossPlatformInputManager.GetButton("DOWN"))
        {
            Debug.Log("DOWN-----GetButtonDown");
        }
        else if (CrossPlatformInputManager.GetButtonUp("DOWN") /*|| CrossPlatformInputManager.GetButtonUp("Jump")*/)
        {
            Debug.Log("DOWN-----GetButtonUp");
            if (menu_controller != null && glass_controller != null)
            {
                if (UIListController.show_flag)
                    glass_controller.HoverNext();
                else
                    menu_controller.HoverNext();
            }
        }

        if (CrossPlatformInputManager.GetButton("LEFT"))
        {
            Debug.Log("LEFT-----GetButtonDown");
        }
        else if (CrossPlatformInputManager.GetButtonUp("LEFT"))
        {
            if (menu_controller != null && glass_controller != null)
            {
                if (!UIListController.show_flag)
                    menu_controller.HoverLeft();
            }
        }

        if (CrossPlatformInputManager.GetButton("RIGHT"))
        {
            Debug.Log("RIGHT-----GetButtonUp");
        }
        else if (CrossPlatformInputManager.GetButtonUp("RIGHT"))
        {
            if (menu_controller != null && glass_controller != null)
            {
                if (!UIListController.show_flag)
                    menu_controller.HoverRight();
            }
        }

        if (CrossPlatformInputManager.GetButton("CENTER"))
        {
            Debug.Log("CENTER-----GetButtonDown");
        }
        else if (CrossPlatformInputManager.GetButtonUp("CENTER"))
        {
            Debug.Log("CENTER-----GetButtonUp");
        }
    }
}