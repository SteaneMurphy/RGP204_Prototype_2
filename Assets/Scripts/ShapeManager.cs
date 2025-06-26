using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public GameObject[] shapes;
    public GameObject shapeContainer;
    public GameObject currentTopShape;
    public GameObject currentBotShape;
    public GameObject topSpawn;
    public GameObject botSpawn;

    //timer
    [SerializeField] float tickSpeed;
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0f;
    }

    private void Update()
    {
        if (currentTopShape != null && currentBotShape != null) 
        {
            DetectInput();
            UpdateTimer();
        }
    }

    public void SpawnShapes() 
    {
        GameObject randomShape1 = shapes[Random.Range(0, 7)];
        GameObject randomShape2 = shapes[Random.Range(0, 7)];

        //top shape spawn
        GameObject topShape = Instantiate(randomShape1, topSpawn.transform.position, Quaternion.identity);
        topShape.transform.parent = shapeContainer.transform;
        currentTopShape = topShape;

        //bottom shape spawn
        GameObject botShape = Instantiate(randomShape2, botSpawn.transform.position, Quaternion.identity);
        botShape.transform.parent = shapeContainer.transform;
        currentBotShape = botShape;
    }

    private void DetectInput() 
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            currentTopShape.transform.position += new Vector3(-1f, 0f, 0f);
            currentBotShape.transform.position += new Vector3(-1f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentTopShape.transform.position += new Vector3(1f, 0f, 0f);
            currentBotShape.transform.position += new Vector3(1f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTopShape.transform.Rotate(0, 0, 90);
            currentBotShape.transform.Rotate(0, 0, 90);
        }
    }

    private void UpdateTimer() 
    {
        if (timeElapsed > tickSpeed) 
        {
            UpdateFall();
            timeElapsed = 0;
        }
        else 
        {
            timeElapsed += Time.deltaTime;
        }
    }

    private void UpdateFall() 
    {
        currentTopShape.transform.position += new Vector3(0f, -1f, 0f);
        currentBotShape.transform.position += new Vector3(0f, 1f, 0f);
    }
}
