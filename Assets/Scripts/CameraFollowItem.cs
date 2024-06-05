using UnityEngine;

public class CameraFollowItem : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private void LateUpdate()
    {
        if(GameManager.gameStarted)
        {
            Vector3 target = new Vector3(playerTransform.position.x, playerTransform.position.y + 2, playerTransform.position.z);
            transform.position = target;
        }
    }
}
