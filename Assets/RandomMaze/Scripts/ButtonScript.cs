using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

    [SerializeField]
    private int difficulty;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("player", StringComparison.InvariantCultureIgnoreCase))
        {
            GetComponent<Animation>().Play();
            GetComponent<AudioSource>().Play();
            StartCoroutine(ChangeLevel());
        }
    }

    private IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(difficulty);
    }
}
