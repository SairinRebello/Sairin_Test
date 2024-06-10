using UnityEngine;
public class Pipes : MonoBehaviour
{
    [SerializeField] private PipeDirection myDirection;
    [SerializeField] private LayerMask pipeMask;
    [SerializeField] private float distance = .25f;

    private Vector2 direction;
    private string parentName;

    private void Awake()
    {
        parentName = transform.parent.name;
        SetDirection();
        GameManager.Instance.onGameWinCheck += OnWinCheck;
    }

    private void SetDirection()
    {
        direction = myDirection == PipeDirection.Right ? Vector2.right : myDirection == PipeDirection.Left ? Vector2.left :
            myDirection == PipeDirection.Up ? Vector2.up : Vector2.down;
    }

    private void OnWinCheck()
    {
        RaycastHit2D hits = Physics2D.Raycast(transform.position, direction, distance, pipeMask);

        if(hits.collider != null && hits.collider.gameObject != gameObject)
        {
            GameManager.Instance.gameWinPoint++;
        }
        else
        {
            GameManager.Instance.gameLosePoint++;
        }

    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, direction * distance, Color.red);
    }
}
