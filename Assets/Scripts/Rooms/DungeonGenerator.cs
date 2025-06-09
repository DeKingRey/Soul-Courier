using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell // Holds the information of every cell within the maze
    {
        public bool visited = false; // Whether the cell has been visited or not
        public bool[] status = new bool[4]; // Status of each entrance to the cell

    }

    public Vector2 size; // Size of grid
    public int startPos = 0; // Starting point on grid(generally 0)
    public GameObject room;
    public Vector2 offset; // Distance between each room    

    List<Cell> board;

    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var newRoom = Instantiate(room, new Vector3(i*offset.x, 0, -j*offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>(); // Instantiates the room
                newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status); // Sets the door statuses

                newRoom.name = " " + i + "-" + j;
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++) // This creates the board, with the width and height of the grid
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>(); // A stack is like a list though it can be imagined like a stack of plates, where you mostly are in control of the top item(most recent)

        int k = 0; // Keeps track what loop we're at

        while(k < 1000) // We don't need a giant maze
        {
            k++;

            board[currentCell].visited = true;

            // Check the cell's neighbours
            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0) // No available neighbours
            {
                if (path.Count == 0) 
                {
                    break; // Breaks if we reach the last cell 
                }
                else
                {
                    currentCell = path.Pop(); // Current cell will become the previous cell in the path(pop removes from stack and returns index)
                }
            }
            else
            {
                path.Push(currentCell); // Adds current cell on top of the path

                int newCell = neighbours[Random.Range(0, neighbours.Count)]; // Chooses a random neighbouring cell

                // Check the direction of the new cell
                if (newCell > currentCell)
                {
                    // Down or right(new cell is greater than current one)
                    if (newCell - 1 == currentCell) // If new cell - 1 is the current cell we know it's right
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell; // Sets the new cell to be current and sets the right hand entrance to true
                        board[currentCell].status[2] = true; // Opens the left cell
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell; // Sets the new cell to be current and sets the down entrance to true
                        board[currentCell].status[0] = true; // Opens the up cell
                    }
                }
                else
                {
                    // Up or left
                    if (newCell + 1 == currentCell) // If new cell + 1 is the current cell we know it's left
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell; // Sets the new cell to be current and sets the left hand entrance to true
                        board[currentCell].status[3] = true; // Opens the right cell
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell; // Sets the new cell to be current and sets the up entrance to true
                        board[currentCell].status[1] = true; // Opens the down cell
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbours(int cell) // Will return a list of cells by taking the position of the current cell
    {
        List<int> neighbours = new List<int>();

        // Due to this being a 1D array size.x represents the number of columns, and size.y is the number of rows
        // To get the index of something, it must be rounded to an int since size is a vector 2 float
        // e.g. cell = 3 and size.x = 1.0, cell-size.x = 2.0 since it uses a float and this can't be used to index

        // Check up neighbour
        if (cell -  size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited) // Checks if there is a spot above it(if >= 0 then it won't be on the board) and if that cell has been visited
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }

        // Check down neighbour
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited) // Checks if there is a spot below it(if > board height then won't be on board) and if that cell has been visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }

        // Check left neighbour
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited) // Checks if there is a spot to the left of it(they will be a multiple of size if on left side) and if that cell has been visited
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }



        // Check right neighbour
        if ((cell+1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited) // Checks if there is a spot to the right of it(the spot + 1 will be the size of the row) and if that cell has been visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        return neighbours;
    }
}
