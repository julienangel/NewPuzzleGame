using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public static CameraManager cameraManager;

	private Camera cameraProperties;

	// Use this for initialization
	void Start () {
		cameraManager = this;
		cameraProperties = GetComponent<Camera> ();
	}

	public void SetPositionAndOrtographicSize(int boardSize)
	{
		float xPosition = (boardSize / 2);
		transform.position = new Vector3 (xPosition -0.5f, xPosition - 0.5f, -10);

		cameraProperties.orthographicSize = (boardSize * 4.5f) / 5;
	}
}
