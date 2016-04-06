using UnityEngine;
using System.Collections;
using UnityEngine.UI;//引用UI命名空间

public class Menu : MonoBehaviour {
	private Color WHITE = new Color (1.0f, 1.0f, 1.0f);
	private Color RED = new Color (1.0f, 0.5f, 0.5f);
	private Vector2 NORMAL_SIZE = new Vector2(160.0f, 30.0f);
	private Vector2 FOCUS_SIZE = new Vector2 (168.0f, 34.0f);

	private GameObject menuPrefab, menu;
	private bool isShown;

	private int cursorIndex;
	private int curBtnIndex;

	public GameObject[] buttonList;

	// Use this for initialization
	void Start () {
		//menuPanel = GameObject.Find ("MenuPanel");
		isShown = false;
		menuPrefab = Resources.Load ("Prefabs/WeaponMenu") as GameObject;

		cursorIndex = 0;
		curBtnIndex = 0;

	}

	private IEnumerator InitMenuButton() {
		yield return new WaitForSeconds (0.1f);
			 
		buttonList = new GameObject[6];
		buttonList [0] = GameObject.Find ("Vulcan");
		buttonList [1] = GameObject.Find ("Sologun");
		buttonList [2] = GameObject.Find ("Sniper");
		buttonList [3] = GameObject.Find ("Shotgun");
		buttonList [4] = GameObject.Find ("Seeker");
		buttonList [5] = GameObject.Find ("Railgun");

		UpdateButtonState ();
	}


	// Update is called once per frame
	void Update () {

	}

	public bool IsShown {
		get{ return isShown;} 
	}

	public void SwitchMenuState() {
		if (isShown)
			CloseMenu ();
		else
			OpenMenu ();
	}


	public bool OpenMenu() {
		if (isShown)
			return false;

		isShown = true;

		Vector3 position = gameObject.transform.position;
		Quaternion rotation = gameObject.transform.rotation;
			
		menu = GameObject.Instantiate (menuPrefab, position, rotation) as GameObject;
			
		//target.transform.rotation = Quaternion.Euler (0, 0, 90);
		menu.transform.parent = this.gameObject.transform;
		menu.SetActive (true);

		StartCoroutine (InitMenuButton ());

		return true;
	}

	public bool CloseMenu() {
		if (!isShown)
			return false;

		isShown = false;
			
		if (menu != null) {
			Destroy (menu);	
			menu = null;
		}
		return true;
	}

	private void UpdateButtonState() {
		GameObject btnObj = buttonList [curBtnIndex] as GameObject;
		btnObj.GetComponent<Image> ().color = WHITE;
		btnObj.GetComponent<RectTransform>().sizeDelta = NORMAL_SIZE;

		btnObj = buttonList [cursorIndex] as GameObject;
		btnObj.GetComponent<Image> ().color = RED;
		btnObj.GetComponent<RectTransform>().sizeDelta = FOCUS_SIZE;

		curBtnIndex = cursorIndex;
	}

	public void HoverUp() {
		if (cursorIndex > 0)
			cursorIndex --;

		if (cursorIndex != curBtnIndex) {
			F3DFXController.instance.SwitchWeapon (cursorIndex);

			UpdateButtonState ();
		}

		F3DFXController.instance.PrevWeapon ();
	}

	public void HoverDown() {
		if (cursorIndex < 5)
			cursorIndex ++;

		if (cursorIndex != curBtnIndex) {
			F3DFXController.instance.SwitchWeapon (cursorIndex);
			
			UpdateButtonState ();
		}
	//	F3DFXController.instance.NextWeapon ();
	}

	public void HoverRight() {

	//	F3DFXController.instance.NextWeapon ();
	}

	public void HoverLeft() {

	//	F3DFXController.instance.PrevWeapon ();
	}

}
