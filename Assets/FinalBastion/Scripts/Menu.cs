using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	GameObject menuPanel;

	// Use this for initialization
	void Start () {
		menuPanel = GameObject.Find ("MenuPanel");
		menuPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsActive {
		get{ return menuPanel.activeSelf;} 
	}

	public void SwitchMenuState() {
		Debug.Log ("SwitchMenuState:"+menuPanel.activeSelf);

		menuPanel.SetActive (!menuPanel.activeSelf);
		if (menuPanel.activeSelf) {
			
		
		}
	}


	public bool CloseMenu() {
		Debug.Log ("CloseMenu");
		if (menuPanel.activeSelf) {
			menuPanel.SetActive (false);
			return true;
		} else {
			return false;
		}
	
	}

	public void HoverUp() {
		F3DFXController.instance.PrevWeapon ();
	}

	public void HoverDown() {
		F3DFXController.instance.NextWeapon ();
	}

	public void HoverRight() {
		F3DFXController.instance.NextWeapon ();
	}

	public void HoverLeft() {
		F3DFXController.instance.PrevWeapon ();
	
	}

}
