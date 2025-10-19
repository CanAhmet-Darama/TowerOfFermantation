using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject stableImage;
    public GameObject unstableImage;
    public AudioSource audioSource;

    public void UnstableWarning()
    {
        StartCoroutine(WaitForWarning());
    }
    IEnumerator WaitForWarning()
    {
        stableImage.SetActive(false);
        unstableImage.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(1.25f);
        stableImage.SetActive(true);
        unstableImage.SetActive(false);
    }
}
