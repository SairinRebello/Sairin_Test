using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;
    private GameObject thisParent;

    private float dragDistance;

    private void Start()
    {
        float distance = (float)GridGenerator.gridSize / 10;
        dragDistance = 1.1f - distance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                thisParent = transform.parent.gameObject;
                transform.SetParent(null);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
                if (gameObject.transform.childCount != 0)
                {
                    gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
                }
                startPosition = transform.position;
                offset = startPosition - mousePosition;
                offset.z = 0;
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            gameObject.GetComponent<Collider2D>().enabled = false;


            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Grids"))
            {
                GameObject otherObject = hit.collider.gameObject;
                Vector3 otherGridPosition = otherObject.transform.position;
                GameObject otherParent = otherObject.transform.parent.gameObject;

                if (Mathf.Abs(startPosition.x - otherGridPosition.x) + Mathf.Abs(startPosition.y - otherGridPosition.y) >= dragDistance)
                {
                    Debug.Log("move");
                    otherObject.transform.SetParent(thisParent.transform);
                    otherObject.transform.position = startPosition;
                    transform.SetParent(otherParent.transform);
                    transform.position = otherGridPosition;
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    if (gameObject.transform.childCount != 0)
                    {
                        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
                }
                else
                {
                    Debug.Log("do not move");
                    transform.SetParent(thisParent.transform);
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    if (gameObject.transform.childCount != 0)
                    {
                        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
                    transform.position = startPosition;
                }
            }
            else
            {
                Debug.Log("did not hit");
                transform.SetParent(thisParent.transform);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                if (gameObject.transform.childCount != 0)
                {
                    gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
            }
            gameObject.GetComponent<Collider2D>().enabled = true;

        }

        if (isDragging)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPos.z = 0;

            float xDiff = Mathf.Abs(newPos.x - startPosition.x);
            float yDiff = Mathf.Abs(newPos.y - startPosition.y);

            if (xDiff > yDiff)
            {
                newPos.y = startPosition.y;
                newPos.x = Mathf.Clamp(newPos.x, startPosition.x - dragDistance, startPosition.x + dragDistance);
            }
            else
            {
                newPos.x = startPosition.x;
                newPos.y = Mathf.Clamp(newPos.y, startPosition.y - dragDistance, startPosition.y + dragDistance);
            }

            transform.position = newPos;
        }
    }

}
