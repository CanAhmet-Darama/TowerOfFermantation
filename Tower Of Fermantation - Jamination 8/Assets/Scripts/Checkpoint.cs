using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public byte index;
    public static byte currentCheckpoint;
    static Vector3[] checkpoints;
    public static bool gameWon = false;
    public static Vector3 winPosition;
    public static Vector3 winCamPos;
    private void Start()
    {
        if (checkpoints == null)
        {
            checkpoints = new Vector3[6];
            currentCheckpoint = 0;
        }
        checkpoints[index - 1] = transform.position;
    }
    public static void Repsawn(GameObject obj)
    {
        obj.transform.position = checkpoints[currentCheckpoint];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentCheckpoint < index)
        {
            currentCheckpoint = (byte)(index - 1);
            UI_Manager.instance.CheckpointUI();
        }
        if(currentCheckpoint == 5)
        {
            GameWin();
        }
    }
    public static void GameWin()
    {
        gameWon = true;
        PlayerController.instance.transform.position = winPosition;
        PlayerController.instance.cameraPivot.Find("Camera").transform.position = winCamPos;
        PlayerController.instance.cameraPivot.Find("Camera").transform.LookAt(PlayerController.instance.transform.position);
    }
}
