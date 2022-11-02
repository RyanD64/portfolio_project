using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject movePlate;

    // Positions
    private int xboard = -1;
    private int yboard = -1;

    // Keeping track of the turn of the players
    private string player;

    // References for the sprites of the game
    public Sprite black_queen, black_king, black_knight, black_bishop, black_pawn, black_rook;
    public Sprite white_queen, white_king, white_knight, white_bishop, white_pawn, white_rook;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        // create the pieces
        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    // place the pieces on the board
    public void SetCoords() {
        float x = xboard;
        float y = yboard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int Getxboard()
    {
        return xboard;
    }

    public int Getyboard()
    {
        return yboard;
    }

    public void Setxboard(int x)
    {
        xboard = x;
    }

    public void Setyboard(int y)
    {
        yboard = y;
    }

    // initiate the moveplates when selecting another piece
    private void OnMouseUp()
    {
        if  (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    // destroy the moveplates when called
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    //initiate moveplate in directions for movement
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xboard, yboard - 1);
                break;
            case "white_pawn":
                PawnMovePlate(xboard, yboard + 1);
                break;
        }
    }

    // creating moveplates in line
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xboard + xIncrement;
        int y = yboard + yIncrement;

        while (sc.PositionOnboard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnboard(x,y) && sc.GetPosition(x,y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x,y);
        }
    }

    // creating moveplates in L(for knight in chess movement)
    public void LMovePlate()
    {
        PointMovePlate(xboard + 1, yboard + 2);
        PointMovePlate(xboard - 1, yboard + 2);
        PointMovePlate(xboard + 2, yboard + 1);
        PointMovePlate(xboard + 2, yboard - 1);
        PointMovePlate(xboard + 1, yboard - 2);
        PointMovePlate(xboard - 1, yboard - 2);
        PointMovePlate(xboard - 2, yboard + 1);
        PointMovePlate(xboard - 2, yboard - 1);
    }

    // creating moveplates around the piece
    public void SurroundMovePlate()
    {
        PointMovePlate(xboard, yboard + 1);
        PointMovePlate(xboard, yboard - 1);
        PointMovePlate(xboard - 1, yboard - 1);
        PointMovePlate(xboard - 1, yboard - 0);
        PointMovePlate(xboard - 1, yboard + 1);
        PointMovePlate(xboard + 1, yboard - 1);
        PointMovePlate(xboard + 1, yboard - 0);
        PointMovePlate(xboard + 1, yboard + 1);
    }

    // managing colisions and attacks with moveplates
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnboard(x,y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            } else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    // managing moveplates for pawns
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnboard(x,y))
        {
            if (sc.GetPosition(x,y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnboard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnboard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }

            switch (this.name)
            {
                case "white_pawn":
                    if (yboard == 1)
                       MovePlateSpawn(x, y + 1);
                    break;
                case "black_pawn":
                    if (yboard == 6)
                       MovePlateSpawn(x, y - 1);
                    break; 
            }
        }
    }

    // handling the positioning of the moveplates
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject map = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mapScript = map.GetComponent<MovePlate>();
        mapScript.SetReference(gameObject);
        mapScript.SetCoords(matrixX, matrixY);
    }

    // handling the positioning of the attack moveplates
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject map = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mapScript = map.GetComponent<MovePlate>();
        mapScript.attack = true;
        mapScript.SetReference(gameObject);
        mapScript.SetCoords(matrixX, matrixY);
    }

    // handling promotions
    void Update()
    {
        switch (this.name)
        {
            case "black_pawn":
                if (yboard == 0)
                {
                    GameObject cp = controller.GetComponent<Game>().GetPosition(xboard, yboard);
                    cp.name = "black_queen";
                    this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black";
                }
            break;

            case "white_pawn":
                if (yboard == 7)
                {
                    GameObject cp = controller.GetComponent<Game>().GetPosition(xboard, yboard);
                    cp.name = "white_queen";
                    this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white";
                }
            break;
        }
    }
}
