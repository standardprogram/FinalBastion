//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
//------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using MojingSample.CrossPlatformInput;

namespace MojingSample.CrossPlatformInput.MojingInput
{
    public class MojingInputManager : MonoBehaviour
    {
        public static MojingInputManager Instance;

        private string state_down = "";
        private string state_up = "";
        private string state_long_down = "";

        private string[] field;
        private int field_num;
        private string current_axis_key;

        private string device_name_attach;
        private string device_name_detach;
        private int attach_flag = 0;

        //------------------
        public string[] buttons = new string[8]{
                "OK",// = 66,
				"C",// = 4,
				"MENU",// = 82,

                //DPAD Mode

                "UP",// = 19
                "DOWN",// = 20
                "LEFT",// = 21 
                "RIGHT",// = 22
                "CENTER",// = 23
			};

        public string[] axes = new string[2] { "Horizontal", "Vertical" };

        [System.NonSerialized]
        public int numAxes, numButtons;
        [System.NonSerialized]
        public CrossPlatformInputManager.VirtualAxis[] _aHandles;
        [System.NonSerialized]
        public CrossPlatformInputManager.VirtualButton[] _bHandles;
        protected int m_Buttons, m_ButtonsPrev;

        //----------------- 

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            int i;
            numAxes = i = axes.Length;
            _aHandles = new CrossPlatformInputManager.VirtualAxis[numAxes];
            for (i = 0; i < numAxes; ++i)
            {
                _aHandles[i] = CrossPlatformInputManager.VirtualAxisReference(this, axes[i], true);
            }

            numButtons = i = buttons.Length;
            _bHandles = new CrossPlatformInputManager.VirtualButton[i];
            for (i = 0; i < numButtons; ++i)
            {
                _bHandles[i] = CrossPlatformInputManager.VirtualButtonReference(this, buttons[i], true);
            }
            m_ButtonsPrev = m_Buttons = 0;
        }

        void OnGUI()
        {
            GUI.skin.label.fontSize = 10;
            GUI.color = Color.black;
            if (attach_flag == 1)
                GUILayout.Label(device_name_attach + " connected", GUILayout.Width(1000));
            else if (attach_flag == 2)
                GUILayout.Label(device_name_detach + " disconnected", GUILayout.Width(1000));
            else
                GUILayout.Label("default state", GUILayout.Width(1000));
            //GUILayout.Label(state_down, GUILayout.Width(1000));
            //GUILayout.Label (state_up, GUILayout.Width(1000));
            //GUILayout.Label (state_long_down, GUILayout.Width(1000));
        }


        //通过接收的串获取键值
        private void getKeyCode(string CurrentBtn)
        {
            field = CurrentBtn.Split('/');
            field_num = CurrentBtn.Split('/').Length;
            for (int i = 0; i < field_num; i++)
            {
            }
            if (field_num == 2)
                current_axis_key = field[field_num - 1];
            else if (field_num == 3)
                current_axis_key = field[field_num - 2] + "/" + field[field_num - 1];
        }

        //按键按下响应
        public void OnButtonDown(string currentBtn)
        {
            state_down = "Button down: " + currentBtn;
            getKeyCode(currentBtn);

            switch (current_axis_key)
            {
                case MojingKeyCode.KEYCODE_ENTER://mojing OK键
                case MojingKeyCode.KEYCODE_BUTTON_START:
                case MojingKeyCode.KEYCODE_BUTTON_A:
                    // Do as you wanna...
                    _bHandles[0].Pressed();
                    break;

                case MojingKeyCode.KEYCODE_BACK://Mojing C键 
                case MojingKeyCode.KEYCODE_BUTTON_SELECT://xiaomi 返回键
                case MojingKeyCode.KEYCODE_BUTTON_B:
                    // Do as you wanna...
                    _bHandles[1].Pressed();
                    break;

                case MojingKeyCode.KEYCODE_MENU://mojing menu键
                case MojingKeyCode.KEYCODE_BUTTON_X:
                    // Do as you wanna...
                    _bHandles[2].Pressed();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_UP://up
                case MojingKeyCode.KEYCODE_DPAD_UP:
                    // Do as you wanna...
                    _bHandles[3].Pressed();
                    break;

                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_DOWN://down
                case MojingKeyCode.KEYCODE_DPAD_DOWN:
                    // Do as you wanna...
                    _bHandles[4].Pressed();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_LEFT://left
                case MojingKeyCode.KEYCODE_DPAD_LEFT:
                    // Do as you wanna...
                    _bHandles[5].Pressed();
                    break;

                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_RIGHT://right
                case MojingKeyCode.KEYCODE_DPAD_RIGHT:
                    // Do as you wanna...
                    _bHandles[6].Pressed();
                    break;

                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_CENTER:
                case MojingKeyCode.KEYCODE_DPAD_CENTER:
                    // Do as you wanna...
                    _bHandles[7].Pressed();
                    break;
                default:

                    return;
            }
        }

        //按键抬起响应
        public void OnButtonUp(string currentButton)
        {
            state_up = "Button up: " + currentButton;
            getKeyCode(currentButton);

            switch (current_axis_key)
            {
                case MojingKeyCode.KEYCODE_ENTER://Mojing ok键
                case MojingKeyCode.KEYCODE_BUTTON_START:
                case MojingKeyCode.KEYCODE_BUTTON_A:
                    // Do as you wanna...
                    _bHandles[0].Released();
                    break;
                case MojingKeyCode.KEYCODE_BACK://Mojing C键 
                case MojingKeyCode.KEYCODE_BUTTON_SELECT://xiaomi 返回键
                case MojingKeyCode.KEYCODE_BUTTON_B:
                    _bHandles[1].Released();
                    // Do as you wanna...
                    break;
                case MojingKeyCode.KEYCODE_MENU://mojing menu键
                case MojingKeyCode.KEYCODE_BUTTON_X:
                    // Do as you wanna...
                    _bHandles[2].Released();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_UP://up
                case MojingKeyCode.KEYCODE_DPAD_UP:
                    // Do as you wanna...
                    _bHandles[3].Released();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_DOWN://down
                case MojingKeyCode.KEYCODE_DPAD_DOWN:
                    // Do as you wanna...
                    _bHandles[4].Released();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_LEFT://left
                case MojingKeyCode.KEYCODE_DPAD_LEFT:
                    // Do as you wanna...
                    _bHandles[5].Released();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_RIGHT://right
                case MojingKeyCode.KEYCODE_DPAD_RIGHT:
                    // Do as you wanna...
                    _bHandles[6].Released();
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_CENTER:
                case MojingKeyCode.KEYCODE_DPAD_CENTER:
                    // Do as you wanna...
                    _bHandles[7].Released();
                    break;
            }

        }
        //按键长按响应
        public void onButtonLongPress(string pressBtn)
        {
            state_long_down = "Long Press: " + pressBtn;
            getKeyCode(pressBtn);

            switch (current_axis_key)
            {
                case MojingKeyCode.KEYCODE_ENTER://Mojing ok键
                case MojingKeyCode.KEYCODE_BUTTON_START:
                case MojingKeyCode.KEYCODE_BUTTON_A:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.KEYCODE_BACK://Mojing C键 
                case MojingKeyCode.KEYCODE_BUTTON_SELECT://xiaomi 返回键
                case MojingKeyCode.KEYCODE_BUTTON_B:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.KEYCODE_MENU://mojing menu键
                case MojingKeyCode.KEYCODE_BUTTON_X:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_LEFT://left
                case MojingKeyCode.KEYCODE_DPAD_LEFT:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_RIGHT://right
                case MojingKeyCode.KEYCODE_DPAD_RIGHT:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_UP://up
                case MojingKeyCode.KEYCODE_DPAD_UP:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_DOWN://down
                case MojingKeyCode.KEYCODE_DPAD_DOWN:
                    // Do as you wanna...
                    break;
                case MojingKeyCode.AXIS_DPAD + "/" + MojingKeyCode.KEYCODE_DPAD_CENTER:
                case MojingKeyCode.KEYCODE_DPAD_CENTER:
                    // Do as you wanna...
                    break;
            }

        }
        public class Key
        {
            public enum KeyState
            {
                KEY_NOTHING,
                KEY_DOWN,
                KEY_UP
            }
            public KeyState keyState = KeyState.KEY_NOTHING;
            public bool IsKeyDown()
            {
                bool res = keyState == KeyState.KEY_DOWN;
                if (res)
                    keyState = KeyState.KEY_NOTHING;
                //MojingLog.LogTrace("IsKeyDown" + res.ToString());
                return res;
            }
            public bool IsKeyUp()
            {
                bool res = keyState == KeyState.KEY_UP;
                if (res)
                    keyState = KeyState.KEY_NOTHING;
                //MojingLog.LogTrace("IsKeyUp" + res.ToString());
                return res;
            }
        }

        public Key touchKey = new Key();

        public void OnTouchEvent(string touchEvent)
        {
            switch (touchEvent)
            {
                case "ACTION_DOWN":
                    //MojingLog.LogTrace("OnTouchEvent: ACTION_DOWN");
                    touchKey.keyState = Key.KeyState.KEY_DOWN;
                    break;

                case "ACTION_UP":
                    //MojingLog.LogTrace("OnTouchEvent: ACTION_UP");
                    touchKey.keyState = Key.KeyState.KEY_UP;
                    break;
            }
        }

        public void onMove(string info)
        {
            getKeyCode(info);
            _aHandles[0].Update(float.Parse(current_axis_key));
            _aHandles[1].Update(float.Parse(current_axis_key));
            //MojingLog.LogTrace(info);
        }

        public void onDeviceAttached(string deviceName)
        {
            device_name_attach = deviceName;
            attach_flag = 1;
            //MojingLog.LogTrace(deviceName + "connected");
        }

        public void onDeviceDetached(string deviceName)
        {
            device_name_detach = deviceName;
            attach_flag = 2;
            //MojingLog.LogTrace(deviceName + "disconnected");
        }

        public void onBluetoothAdapterStateChanged(string state)
        {
            switch (state)
            {
                case "12":
                    // BluetoothAdapter.STATE_ON
                    //MojingLog.LogTrace("Bluetooth ON");
                    break;

                case "10":
                    // BluetoothAdapter.STATE_OFF
                    //MojingLog.LogTrace("Bluetooth OFF");
                    break;
            }
        }
    }
}