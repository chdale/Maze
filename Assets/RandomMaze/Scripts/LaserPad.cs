using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPad : MonoBehaviour {
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<GrowScript>();
        if (player != null)
        {
            player.Grow();
            gameController.StopTime();
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
