using UnityEngine;
using System.Collections;
using MojingSample.CrossPlatformInput;

public class UiInputManager : MonoBehaviour {

	private Menu menu;

	// Use this for initialization
	void Start () {
		menu = gameObject.GetComponent<Menu>();
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonUp("C"))
		{
			Debug.Log("C-----GetButtonUp");

			if(!menu.CloseMenu()) {
				//如果菜单没打开，退出游戏
				Application.Quit();
			}
		}
		
		if (CrossPlatformInputManager.GetButtonUp("MENU"))
		{
			Debug.Log("MENU-----GetButtonDown");
			menu.SwitchMenuState();
		
		}

		if (CrossPlatformInputManager.GetButtonUp ("UP")) {
			if(menu.IsActive)
				menu.HoverUp();
		}

		if (CrossPlatformInputManager.GetButtonUp ("DOWN")) {
			if(menu.IsActive)
				menu.HoverDown();
		}

		if (CrossPlatformInputManager.GetButtonUp ("RIGHT")) {
			if(menu.IsActive)
				menu.HoverRight();
		}

		if (CrossPlatformInputManager.GetButtonUp ("LEFT")) {
			if(menu.IsActive)
				menu.HoverLeft();
		}
	}

		
}
