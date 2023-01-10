using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private BoxCollider detectionArea;
    private Animator animator;

    private int playerInrange =0;
    void Start()
    {
        detectionArea = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInrange -= 1;

            if(playerInrange <= 0)
            {
                animator.SetBool("IsOpen", false);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInrange += 1;
            if (playerInrange > 0)
            {
                Debug.Log("sesame");
                animator.SetBool("IsOpen", true);
                Debug.Log(animator.GetBool("IsOpen"));
            }

        }
    }
}
