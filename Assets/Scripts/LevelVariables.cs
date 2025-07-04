using UnityEngine;

public class LevelVariables : MonoBehaviour
{
    public GameObject topScreen;
    public GameObject botScreen;
    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float botBoundary;
    public float midBoundary;

    private void Start()
    {
        leftBoundary = topScreen.GetComponent<Renderer>().bounds.min.x;
        rightBoundary = topScreen.GetComponent <Renderer>().bounds.max.x;
        topBoundary = topScreen.GetComponent<Renderer>().bounds.max.y;
        botBoundary = botScreen.GetComponent<Renderer>().bounds.min.y;
        midBoundary = topScreen.GetComponent<Renderer>().bounds.min.y;
    }
}
