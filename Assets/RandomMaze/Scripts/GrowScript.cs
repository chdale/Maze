using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;

public class GrowScript : MonoBehaviour {
    private Animation grow;
    private SDK_InputSimulator inputSimulator;
    private OVRPlayerController playerController;

    private void Start()
    {
        grow = GetComponent<Animation>();
        inputSimulator = GetComponent<SDK_InputSimulator>();
        playerController = transform.parent.GetComponent<OVRPlayerController>();
    }

    public void Grow()
    {
        StartCoroutine(GrowCoroutine());
    }

    private IEnumerator GrowCoroutine()
    {
        if (inputSimulator != null)
        {
            inputSimulator.enabled = false;
        }
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        grow.Play();
        yield return new WaitForSeconds(2f);
        if (inputSimulator != null)
        {
            inputSimulator.enabled = true;
            inputSimulator.playerMoveMultiplier = 100f;
        }
        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.Acceleration = 1.5f;
        }
    }
}
