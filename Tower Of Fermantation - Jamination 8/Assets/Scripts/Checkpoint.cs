using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public byte index;
    public static byte currentCheckpoint;
    static Vector3[] checkpoints;
    private void Start()
    {
        if (checkpoints == null)
        {
            checkpoints = new Vector3[5];
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
            currentCheckpoint = index;
            UI_Manager.instance.CheckpointUI();
        }
    }
}
