using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlades : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, 0f, 15.0f, Space.Self);
	}
}
