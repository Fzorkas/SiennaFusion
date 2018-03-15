using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    public GameObject folow;
    public Vector2 boundingRange;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(folow.transform.position.x, folow.transform.position.y, -10);
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 tempPos = folow.transform.position - transform.position;

        if(Mathf.Abs(tempPos.x) > boundingRange.x)
        {
            transform.position = new Vector3(folow.transform.position.x - (boundingRange.x * Mathf.Sign(tempPos.x)), transform.position.y, -10);
        }
        if (Mathf.Abs(tempPos.y) > boundingRange.y)
        {
            transform.position = new Vector3(transform.position.x, folow.transform.position.y - (boundingRange.y * Mathf.Sign(tempPos.y)), -10);
        }
    }
}
