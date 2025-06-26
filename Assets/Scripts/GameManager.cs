using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ShapeManager shapeManager;

    private void Start()
    {
        shapeManager.SpawnShapes();
    }
}
