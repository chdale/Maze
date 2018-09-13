using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    private bool keyEntered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "KeyHole")
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<OVRGrabbable>().enabled = false;
            gameObject.transform.parent.position = collision.gameObject.transform.position + new Vector3(.116639f, .057997f, -.0210001f);
            gameObject.transform.rotation = new Quaternion(0.576f, 15.436f, 111.519f, 1);

            keyEntered = true;
        }
    }
}
