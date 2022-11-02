using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    // Positions and team for all pieces
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //Create all pieces and assigning their coordinates
        playerWhite = new GameObject[] {
            Create("white_rook", 0, 0),
            Create("white_knight", 1, 0),
            Create("white_bishop", 2 ,0),
            Create("white_queen", 3, 0),
            Create("white_king", 4, 0),
            Create("white_bishop", 5, 0),
            Create("white_knight", 6, 0),
            Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1),
            Create("white_pawn", 1, 1),
            Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1),
            Create("white_pawn", 4, 1),
            Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1),
            Create("white_pawn", 7, 1),
        };
        playerBlack = new GameObject[] {
            Create("black_rook", 0, 7),
            Create("black_knight", 1, 7),
            Create("black_bishop", 2, 7),
            Create("black_king", 4, 7),
            Create("black_queen", 3, 7),
            Create("black_bishop", 5, 7),
            Create("black_knight", 6, 7),
            Create("black_rook", 7, 7),
            Create("black_pawn", 0, 6),
            Create("black_pawn", 1, 6),
            Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6),
            Create("black_pawn", 4, 6),
            Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6),
            Create("black_pawn", 7, 6),
        };

        // Set all pieces in position
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    // in-depth creation of the pieces and assigning their moveplates 
    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0,0,-1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.Setxboard(x);
        cm.Setyboard(y);
        cm.Activate();
        return obj;
    }

    // setting moveplates position
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        positions[cm.Getxboard(), cm.Getyboard()] = obj;
    }

    // for selecting nothing on the board
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    // getter of the position of the selected object on the board
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    // check if the selected object is on the board
    public bool PositionOnboard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    // setting who's turn
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    // managing gameover
    public bool IsGameOver()
    {
        return gameOver;
    }

    // setting turn pass
    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        } 
        else
        {
            currentPlayer = "white";
        }
    }

    // start of a new game
    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }

    // Telling the winner
    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("Winner_Text").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("Winner_Text").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("Restart_Text").GetComponent<Text>().enabled = true;
    }
}
