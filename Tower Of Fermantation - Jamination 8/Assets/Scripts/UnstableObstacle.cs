using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableObstacle : MonoBehaviour
{
    public ObstacleType obstacleType;
    [Header("Positioning")]
    public float moveSpeed;
    public Vector3[] positions;
    Vector3 currentTarget;
    public bool unslidingTop; // When character steps on it, this object becomes parent so character doesn't slide off
    bool firstPosTargeted = true;
    //public bool impulsing;
    public static float obstacleUnstability;
    private void Start()
    {
        if (obstacleType == ObstacleType.positioning) {
            currentTarget = positions[0];
        }
    }
    private void Update()
    {
        if (obstacleType == ObstacleType.rotating){
            transform.Rotate(Vector3.up, 135 * (PlayerController.currentUnstability + obstacleUnstability) * Time.deltaTime);
        }
        else if (obstacleType == ObstacleType.positioning) {
            transform.position += (currentTarget-transform.position).normalized*(PlayerController.currentUnstability + obstacleUnstability)*moveSpeed*Time.deltaTime;
            if ((currentTarget - transform.position).sqrMagnitude<0.1f)
            {
                firstPosTargeted = !firstPosTargeted;
                currentTarget = positions[(firstPosTargeted?0:1)];
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (unslidingTop)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = transform;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (unslidingTop)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = null;
            }
        }

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (impulsing)
    //    {
    //        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
    //        if (player != null && !player.impulsedAway)
    //        {
    //            player.impulsedAway = true;
    //            StartCoroutine(ImpulsableAgain(player));
    //            player.playerRb.AddForce(((collision.gameObject.transform.position - collision.GetContact(0).point).normalized + Vector3.up) * 30000);
    //        }
    //    }
    //}
    //IEnumerator ImpulsableAgain(PlayerController player)
    //{
    //    yield return new WaitForSeconds(1);
    //    player.impulsedAway = false;
    //}
}
public enum ObstacleType { rotating, positioning}