using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        //determine which player object is detecting input
        if (player.gameObject.tag == "Player1")
        {
            HandlePlayer1Input();
        }
        else if (player.gameObject.tag == "Player2")
        {
            HandlePlayer2Input();
        }
    }

    //player1: WASD, SPACE, Q
    private void HandlePlayer1Input()
    {

    }

    //player2: Joystick/Controller/Fire3 button
    private void HandlePlayer2Input()
    {

    }
}
