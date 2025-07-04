using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public GameObject[] shapes;
    public GameObject topContainer;
    public GameObject botContainer;
    public GameObject currentTopShape;
    public GameObject currentBotShape;
    public GameObject topSpawn;
    public GameObject botSpawn;

    public LevelVariables levelScript;
    public AudioManager audioManager;
    public CameraShake shake;

    //powerup variables
    public bool slowDownGame = false;
    public float slowDownSpeed;

    //timer top
    public float topTickSpeed;
    private float storeTopTickSpeed;
    private float topTimeElapsed;
    private bool topFastMode = false;
    public bool topFreeze = false;

    //time bottom
    public float botTickSpeed;
    private float storeBotTickSpeed;
    private float botTimeElapsed;
    private bool botFastMode = false;
    public bool botFreeze = false;

    private void Start()
    {
        topTimeElapsed = 0f;
        storeTopTickSpeed = topTickSpeed;
        botTimeElapsed = 0f;
        storeBotTickSpeed = botTickSpeed;
    }

    private void Update()
    {
        if (currentTopShape != null && currentBotShape != null)
        {
            DetectInput();
            AdjustTopMode();
            AdjustBotMode();
            UpdateTimerTop();
            UpdateTimerBot();
            CheckForCompleteLines();
        }
    }

    private void AdjustTopMode()
    {
        if (topFastMode)
        {
            topTickSpeed = -2f;
        }
        else if (slowDownGame) 
        {
            topTickSpeed = slowDownSpeed;
        }
        else 
        {
            topTickSpeed = storeTopTickSpeed;
        }
    }

    private void AdjustBotMode() 
    {
        if (botFastMode)
        {
            botTickSpeed = -2f;
        }
        else if (slowDownGame)
        {
            botTickSpeed = slowDownSpeed;
        }
        else
        {
            botTickSpeed = storeBotTickSpeed;
        }
    }

    public void SpawnTop()
    {
        GameObject randomShape = shapes[Random.Range(0, 7)];

        //top shape spawn
        GameObject topShape = Instantiate(randomShape, topSpawn.transform.position, Quaternion.identity);
        topShape.transform.parent = topContainer.transform;
        currentTopShape = topShape;
    }

    public void SpawnBot() 
    {
        GameObject randomShape = shapes[Random.Range(0, 7)];

        //bottom shape spawn
        GameObject botShape = Instantiate(randomShape, botSpawn.transform.position, Quaternion.identity);
        botShape.transform.parent = botContainer.transform;
        currentBotShape = botShape;
    }

    public void DeleteTop() 
    {
        Destroy(currentTopShape);
    }

    public void DeleteBot() 
    {
        Destroy(currentBotShape);
    }

    public void ClearTop()
    {
        foreach (Transform shape in topContainer.transform)
        {
            if (shape.gameObject == currentTopShape)
            {
                continue;
            }
            else 
            {
                Destroy(shape.gameObject);
            }
        }
    }

    public void ClearBot() 
    {
        foreach (Transform shape in botContainer.transform)
        {
            if (shape.gameObject == currentBotShape)
            {
                continue;
            }
            else
            {
                Destroy(shape.gameObject);
            }
        }
    }

    private void DetectInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(CalculateLeftmostPoint(currentTopShape) > levelScript.leftBoundary && !CalculateLeftCollisionTop(currentTopShape)) 
            {
                currentTopShape.transform.position += new Vector3(-1f, 0f, 0f);
            }
            if(CalculateLeftmostPoint(currentBotShape) > levelScript.leftBoundary && !CalculateLeftCollisionBot(currentBotShape)) 
            {
                currentBotShape.transform.position += new Vector3(-1f, 0f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(CalculateRightmostPoint(currentTopShape) < levelScript.rightBoundary && !CalculateRightCollisionTop(currentTopShape)) 
            {
                currentTopShape.transform.position += new Vector3(1f, 0f, 0f);
            }
            if (CalculateRightmostPoint(currentBotShape) < levelScript.rightBoundary && !CalculateRightCollisionBot(currentBotShape))
            {
                currentBotShape.transform.position += new Vector3(1f, 0f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            topFastMode = true;
            audioManager.PlaySound("blockDrop");
            shake.Shake(0.2f, 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            botFastMode = true;
            audioManager.PlaySound("blockDrop");
            shake.Shake(0.2f, 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioManager.PlaySound("rotation");
            currentTopShape.transform.Rotate(0, 0, 90);
            currentBotShape.transform.Rotate(0, 0, 90);
        }
    }

    private void UpdateTimerTop()
    {
        if (!topFreeze) 
        {
            if (topTimeElapsed > topTickSpeed)
            {
                UpdateFallTop();
                topTimeElapsed = 0;
            }
            else
            {
                topTimeElapsed += Time.deltaTime;
            }
        }
    }

    private void UpdateTimerBot()
    {
        if (botFreeze) 
        {
            return;
        }

        if (botTimeElapsed > botTickSpeed)
        {
            UpdateFallBot();
            botTimeElapsed = 0;
        }
        else
        {
            botTimeElapsed += Time.deltaTime;
        }
    }

    private void UpdateFallTop()
    {
        if (CalculateBotmostPoint(currentTopShape) > levelScript.midBoundary && !CalculateTopCollision(currentTopShape)) 
        {
            currentTopShape.transform.position += new Vector3(0f, -1f, 0f);
        }
        else 
        {
            topFastMode = false;
            SpawnTop();
        }
    }

    private void UpdateFallBot()
    {
        if (CalculateTopmostPoint(currentBotShape) < levelScript.midBoundary && !CalculateBotCollision(currentBotShape))
        {
            currentBotShape.transform.position += new Vector3(0f, 1f, 0f);
        }
        else
        {
            botFastMode = false;
            SpawnBot();
        }
    }

    private float CalculateLeftmostPoint(GameObject currentShape) 
    {
        float leftmostPoint = levelScript.rightBoundary;

        foreach (Transform block in currentShape.transform) 
        {
            float leftSide = block.GetComponent<Renderer>().bounds.min.x;
            if (leftSide < leftmostPoint) 
            {
                leftmostPoint = leftSide;
            }
        }

        return leftmostPoint;
    }

    private float CalculateRightmostPoint(GameObject currentShape)
    {
        float rightmostPoint = levelScript.leftBoundary;

        foreach (Transform block in currentShape.transform)
        {
            float rightSide = block.GetComponent<Renderer>().bounds.max.x;
            if (rightSide > rightmostPoint)
            {
                rightmostPoint = rightSide;
            }
        }

        return rightmostPoint;
    }

    private float CalculateTopmostPoint(GameObject currentShape)
    {
        float topmostPoint = levelScript.botBoundary;

        foreach (Transform block in currentShape.transform)
        {
            float topside = block.GetComponent<Renderer>().bounds.max.y;
            if (topside > topmostPoint)
            {
                topmostPoint = topside;
            }
        }

        return topmostPoint;
    }

    private float CalculateBotmostPoint(GameObject currentShape)
    {
        float botmostPoint = levelScript.topBoundary;

        foreach (Transform block in currentShape.transform)
        {
            float botside = block.GetComponent<Renderer>().bounds.min.y;
            if (botside < botmostPoint)
            {
                botmostPoint = botside;
            }
        }

        return botmostPoint;
    }

    private bool CalculateTopCollision(GameObject currentShape)
    {
        foreach (Transform block in currentShape.transform)
        {
            foreach (Transform shape in topContainer.transform)
            {
                if (shape.gameObject == currentShape) 
                {
                    continue;
                }
                foreach (Transform subBlock in shape.transform)
                {
                    if (block.transform.position.x == subBlock.transform.position.x)
                    {
                        if (block.GetComponent<Renderer>().bounds.min.y <= subBlock.GetComponent<Renderer>().bounds.max.y)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CalculateBotCollision(GameObject currentShape)
    {
        foreach (Transform block in currentShape.transform)
        {
            foreach (Transform shape in botContainer.transform)
            {
                if (shape.gameObject == currentShape)
                {
                    continue;
                }
                foreach (Transform subBlock in shape.transform)
                {
                    if (block.transform.position.x == subBlock.transform.position.x)
                    {
                        if (block.GetComponent<Renderer>().bounds.max.y >= subBlock.GetComponent<Renderer>().bounds.min.y)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CalculateRightCollisionTop(GameObject currentShape)
    {
        foreach (Transform block in currentShape.transform)
        {
            foreach (Transform shape in topContainer.transform)
            {
                if (shape.gameObject == currentShape)
                {
                    continue;
                }
                foreach (Transform subBlock in shape.transform)
                {
                    if (block.transform.position.y == subBlock.transform.position.y)
                    {
                        if (block.GetComponent<Renderer>().bounds.max.x >= subBlock.GetComponent<Renderer>().bounds.min.x)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CalculateLeftCollisionTop(GameObject currentShape)
    {
        foreach (Transform block in currentShape.transform)
        {
            foreach (Transform shape in topContainer.transform)
            {
                if (shape.gameObject == currentShape)
                {
                    continue;
                }
                foreach (Transform subBlock in shape.transform)
                {
                    if (block.transform.position.y == subBlock.transform.position.y)
                    {
                        if (block.GetComponent<Renderer>().bounds.min.x >= subBlock.GetComponent<Renderer>().bounds.max.x)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CalculateRightCollisionBot(GameObject currentShape)
    {
        foreach (Transform block in currentShape.transform)
        {
            foreach (Transform shape in botContainer.transform)
            {
                if (shape.gameObject == currentShape)
                {
                    continue;
                }
                foreach (Transform subBlock in shape.transform)
                {
                    if (block.transform.position.y == subBlock.transform.position.y)
                    {
                        if (block.GetComponent<Renderer>().bounds.max.x >= subBlock.GetComponent<Renderer>().bounds.min.x)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CalculateLeftCollisionBot(GameObject currentShape)
    {
        foreach (Transform block in currentShape.transform)
        {
            foreach (Transform shape in botContainer.transform)
            {
                if (shape.gameObject == currentShape)
                {
                    continue;
                }
                foreach (Transform subBlock in shape.transform)
                {
                    if (block.transform.position.y == subBlock.transform.position.y)
                    {
                        if (block.GetComponent<Renderer>().bounds.min.x >= subBlock.GetComponent<Renderer>().bounds.max.x)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void CheckForCompleteLines() 
    {
        // for each line above the midpoint
        for (float i = 0.5f; i < levelScript.topBoundary; i++) 
        {
            int blocksHit = 0;
            Vector3 rayOrigin = new Vector3(-1f, i, 0f);
            Debug.DrawRay(rayOrigin, Vector2.right * 15f, Color.red, 1f);
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.right, 15f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Block1" ||
                    hit.collider.gameObject.tag == "Block2" ||
                    hit.collider.gameObject.tag == "Block3" ||
                    hit.collider.gameObject.tag == "Block4" ||
                    hit.collider.gameObject.tag == "Block5" ||
                    hit.collider.gameObject.tag == "Block6" ||
                    hit.collider.gameObject.tag == "Block7")
                {
                    blocksHit++;
                }
            }

            if (blocksHit == 10) 
            {
                foreach (RaycastHit2D hit in hits) 
                {
                    if (hit.collider.gameObject.tag == "Block1" ||
                        hit.collider.gameObject.tag == "Block2" ||
                        hit.collider.gameObject.tag == "Block3" ||
                        hit.collider.gameObject.tag == "Block4" ||
                        hit.collider.gameObject.tag == "Block5" ||
                        hit.collider.gameObject.tag == "Block6" ||
                        hit.collider.gameObject.tag == "Block7")
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }

        //// for each line below the midpoint
        //for (float i = -0.5f; i > levelScript.botBoundary; i++)
        //{
        //    int blocksHit = 0;
        //    Vector3 rayOrigin = new Vector3(-1f, i, 0f);
        //    Debug.DrawRay(rayOrigin, Vector2.right * 15f, Color.red, 1f);
        //    RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.right, 15f);

        //    foreach (RaycastHit2D hit in hits)
        //    {
        //        if (hit.collider.gameObject.tag == "Block1" ||
        //            hit.collider.gameObject.tag == "Block2" ||
        //            hit.collider.gameObject.tag == "Block3" ||
        //            hit.collider.gameObject.tag == "Block4" ||
        //            hit.collider.gameObject.tag == "Block5" ||
        //            hit.collider.gameObject.tag == "Block6" ||
        //            hit.collider.gameObject.tag == "Block7")
        //        {
        //            blocksHit++;
        //        }
        //    }

        //    if (blocksHit == 10)
        //    {
        //        foreach (RaycastHit2D hit in hits)
        //        {
        //            if (hit.collider.gameObject.tag == "Block1" ||
        //                hit.collider.gameObject.tag == "Block2" ||
        //                hit.collider.gameObject.tag == "Block3" ||
        //                hit.collider.gameObject.tag == "Block4" ||
        //                hit.collider.gameObject.tag == "Block5" ||
        //                hit.collider.gameObject.tag == "Block6" ||
        //                hit.collider.gameObject.tag == "Block7")
        //            {
        //                Destroy(hit.collider.gameObject);
        //            }
        //        }
        //    }
        //}

        //move remaining blocks down by amount of lines removed
    }
}
