using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] 
    private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();

        if (cc != null)
        {
            RespawnPlayer(cc);
        }
    }

    private void RespawnPlayer(CharacterController cc)
    {
        cc.enabled = false; // Disable to stop physics from fighting the teleport [cite: 13, 19]
        cc.transform.position = respawnPoint.position; // Move to the empty object [cite: 20]
        cc.enabled = true; // Turn it back on so you can move again [cite: 21]
    }
}