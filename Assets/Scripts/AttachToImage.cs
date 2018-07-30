using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;


public class AttachToImage : MonoBehaviour {

	public int targetImage;
	public GameObject prefabToPlace;
	public GameObject scanReminderUI;
	private GameObject prefabInstance;

	private List<AugmentedImage> augmentedImagesSeen = new List<AugmentedImage>();
	private AugmentedImage image = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Check that motion tracking is tracking.
		if(Session.Status != SessionStatus.Tracking) {
			image = null;
			return;
		}

		if( image == null ) {
			Session.GetTrackables( augmentedImagesSeen, TrackableQueryFilter.Updated );
			foreach( AugmentedImage thisImage in augmentedImagesSeen ) {
				if( thisImage.DatabaseIndex == targetImage ){
					image = thisImage;
					scanReminderUI.SetActive(false);
				}
			}
		}

		if( image.TrackingState == TrackingState.Stopped ) {
			image = null;
			Destroy( prefabInstance );
			scanReminderUI.SetActive(true);
			prefabInstance = null;
		} else {
			if( prefabInstance == null ) {
				prefabInstance = Instantiate( prefabToPlace );
			}
			prefabInstance.transform.position = image.CenterPose.position;
			prefabInstance.transform.rotation = image.CenterPose.rotation;
		}


	}
}
