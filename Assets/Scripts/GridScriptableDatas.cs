using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Data", menuName = "Custom Datas/Grid Data")]
public class GridScriptableDatas : ScriptableObject
{
    [Header("Difficulty")]
    [SerializeField] public Difficulty difficult;

    [Header("Grids"), Space(1)]
    [SerializeField] public GameObject grid;
    [SerializeField] public GameObject gridCell;
    [SerializeField] public GameObject gridParent;
    [SerializeField] public int gridSize;

    [Header("Start and End"), Space(1)]
    [SerializeField] public GameObject startPipe;
    [SerializeField] public GameObject endItem;

    [Header("All Pipes"), Space(1)]
    [SerializeField] public GameObject rightUp;
    [SerializeField] public GameObject rightDown;
    [SerializeField] public GameObject leftUp;
    [SerializeField] public GameObject leftDown;
    [SerializeField] public GameObject horizontal;
    [SerializeField] public GameObject vertical;
}

