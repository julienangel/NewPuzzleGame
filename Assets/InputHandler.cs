using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	GameManager gm;

	// Use this for initialization
	void Start () {
		gm = GameManager.gameManager;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			if (gm.gameState == GameManager.GameState.Editor)
			{
				gm.boardManager.EditOrPlacePiece (ConvertScreenToWorldPosition (Input.mousePosition));
			}
		}
	}

	public Vector2 ConvertScreenToWorldPosition(Vector3 mousePosition)
	{
		Vector3 v3 = Camera.main.ScreenToWorldPoint (mousePosition);
		return new Vector2 (v3.x, v3.y);
	}
}
