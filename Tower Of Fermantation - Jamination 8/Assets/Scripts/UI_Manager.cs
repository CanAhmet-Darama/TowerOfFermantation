using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public GameObject stableImage;
    public GameObject unstableImage;
    public AudioSource audioSource;
    public TextMeshProUGUI checkpointText;
    private void Start()
    {
        instance = this;
    }
    public void UnstableWarning()
    {
        StartCoroutine(WaitForWarning());
    }
    public void CheckpointUI()
    {
        StartCoroutine(CheckpointWarning());
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
    public IEnumerator CheckpointWarning()
    {
        checkpointText.enabled = true;
        yield return new WaitForSeconds(3);
        checkpointText.enabled = false;
    }
}
