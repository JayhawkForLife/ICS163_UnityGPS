using UnityEngine;
using System.Collections;

public class myLocationScript : MonoBehaviour {
	
	public GUISkin myGUISkin;
	
	private myNetworkHelper network_helper;
	private MyGUIScript local_gui;
	
	private bool working = false;
	
	// Use this for initialization
	IEnumerator Start () {
		local_gui = GameObject.Find ("myGUI").GetComponentInChildren<MyGUIScript> ();
		network_helper = GameObject.Find ("myNetworkHelper").GetComponentInChildren<myNetworkHelper> ();

		Input.location.Start();

		if(this.myGUISkin == null)  
		{  
			local_gui.addDebug("Please assign a GUIskin on the editor!");  
			this.enabled = false;  
			yield return false;  
		} 
		
		if (!Input.location.isEnabledByUser) {
			local_gui.addDebug ("User has not enabled Location");
			yield return false;
		}
		
		Input.location.Start (1f,1f);
		
		// Wait until service initializes
		int maxWait = 20;
		while ((Input.location.status == LocationServiceStatus.Initializing) && (maxWait > 0)) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}
		
		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			print ("Timed out");
			yield return false;
		}
		
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			local_gui.addDebug ("Unable to determine device location");
			return false;
		}
		else {
			local_gui.addDebug("Location: " + Input.location.lastData.latitude + " " +
			                   Input.location.lastData.longitude + " " +
			                   (long)Input.location.lastData.timestamp);
			working = true;
		}
		
		//Input.location.Stop ();
		
	}
	
	float lastLongitude = -1;
	
	// Update is called once per frame
	void Update () {
		if (working) {
			if(local_gui.pen_down){
				//check to see if the location has changed since last Update
				if(lastLongitude != Input.location.lastData.longitude){
					//do something;

					network_helper.addPoint(local_gui.stroke_name.ToString(), (long)Input.location.lastData.timestamp, (double)Input.location.lastData.latitude,
					                        (double)Input.location.lastData.longitude, (double)Input.location.lastData.altitude);

					lastLongitude = Input.location.lastData.longitude;

					local_gui.addDebug("Location: " + Input.location.lastData.latitude + " " +
					                   Input.location.lastData.longitude + " " +
					                   (long)Input.location.lastData.timestamp);
				}
				//Input.location.lastData.latitude;
				//Input.location.lastData.altitude;
				//Input.location.lastData.timestamp;
				//If it has add it to our NetworkHelper
			}
		}
		
	}
	
	void OnGUI () {
		GUI.skin = myGUISkin;
		
		GUI.Label (new Rect (0, Screen.height - 100, Screen.width, 100), "" + Input.location.lastData.longitude);
		
	}
}
