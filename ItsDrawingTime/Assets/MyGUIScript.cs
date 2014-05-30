using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGUIScript : MonoBehaviour {

	public GUISkin myGUISkin;
	public List<string> debugs;
	private string group_name;
	private string drawing_name;
	public bool pen_down = false;
	public int stroke_name;
	private Color pen_color;
	ColorPicker[] color_picker;

	private myNetworkHelper network_helper;

	// Use this for initialization
	void Start () {

		if(this.myGUISkin == null)
		{
			addDebug("please assign a GUIskin on the editor!");
			this.enabled = false;
			return;
		}

		network_helper = GameObject.Find ("myNetworkHelper").GetComponentInChildren<myNetworkHelper>();

		group_name = "groupname";
		drawing_name = "" + Random.value;
		stroke_name = 0;

		color_picker = GameObject.FindObjectsOfType<ColorPicker>();

		foreach( ColorPicker elem in color_picker)
		{
			elem.useExternalDrawer = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.skin = myGUISkin;
		group_name = GUI.TextField (new Rect(0, 0, Screen.width, 100), group_name);
		drawing_name = GUI.TextField (new Rect(0, 100, Screen.width, 100), drawing_name);

		foreach(ColorPicker cp in color_picker)
		{
			cp._DrawGUI();
		}

		bool oldPenDown = pen_down;

		pen_down = GUI.Toggle (new Rect(0, 600, Screen.width, 100), pen_down, "Pen Down");

		if(GUI.changed){
			if(oldPenDown != pen_down){

				if(!pen_down)
				{
					network_helper.addStrokeColor(stroke_name.ToString(), pen_color);
					stroke_name++;
				}

			}
			
		}

		if(GUI.Button (new Rect(0, 700, Screen.width, 100), "Upload"))
		{
			// Upload the data
			if(!pen_down)
			{
				network_helper.uploadPoints(group_name, drawing_name);
			}
		}

		int tempy = 800;
		foreach(string d in debugs)
		{
			GUI.Label (new Rect(0, tempy, Screen.width, 100),d);
			tempy += 100;
		}
		while(debugs.Count > 4)
		{
			debugs.RemoveAt(0);
		}
	}

	public void addDebug(string e){
		debugs.Add(e);
		Debug.Log (e);
	}

	void OnSetColor(Color color)
	{
		pen_color = color;
		addDebug ("NewColor" + color);
	}

	void OnGetColor(ColorPicker picker)
	{
		picker.NotifyColor(pen_color);
	}
}
