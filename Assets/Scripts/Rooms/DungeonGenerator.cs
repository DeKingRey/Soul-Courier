using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell // Holds the information of every cell within the maze
    {
        public bool visited = false; // Whether the cell has been visited or not
        public bool[] status = new bool[4]; // Status of each entrance to the cell
        public int index = -1;
    }

    [System.Serializable]
    public class Rule // Creates rules for generating rooms
    {
        public GameObject room;
        public int minPos;
        public int maxPos; // These numbers will be based off the amount of rooms on the floor e.g. min = 5, max = 15 so room spawns between 5 and 15

        public bool compulsory; // Whether the room has to spawn at a certain position or not

        // Make the rooms have to spawn at a certain position, though the grid can differentiate depending on the layout
        // Use the current iteration to decide when the room must spawn
        // As the max amount of rooms is 15, for example the final room must spawn between room 15 and 15(so spawns at 15)
        // Maybe do this with a foreach loop

        public int ProbabilityOfSpawning(int i)
        {
            // 0 - Cannot Spawn 1 - Can Spawn 2 - HAS to spawn at position
             
            if (i >= minPos && i <= maxPos) // If the iteratipon is between the min and max iteration then continue
            {
                return compulsory ? 2 : 1; // If compulsory it returns 2, else returns 1
            }

            return 0;
        }
    }

    public Vector2Int size; // Size of grid
    public int startPos = 0; // Starting point on grid(generally 0)
    public Rule[] rooms;
    public Vector2 offset; // Distance between each room    
    public int maxRooms;

    List<Cell> board;
    List<Cell> cells;

    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        foreach (Cell cell in cells)
        {
            int i = board.IndexOf(cell) % size.x; // Gets the column, e.g. 3*3 grid, 6 % 3(3 is the width) = 0, and 6 would be in the 0th column
            int j = board.IndexOf(cell) / size.x; // Gets the row, e.g. 3*3 grid 6 / 3 = 2, 6 would be on the 2nd index 

            int randomRoom = -1; // Gets a random room
            List<int> availableRooms = new List<int>();

            for (int k = 0; k < rooms.Length; k++) // Loops through each room prefab and its rules
            {
                    int p = rooms[k].ProbabilityOfSpawning(cell.index); // Checks rules for current position
                    Debug.Log($"x: {i}, y: {j}, index: {cell.index}");

                    if (p == 2) // If the room has to spawn at a certain position
                    {
                        randomRoom = k;
                        break;
                    }
                    else if (p == 1) // If the room can spawn
                    {
                        availableRooms.Add(k);
                    }
            }

            if (randomRoom == -1)
            {
                if (availableRooms.Count > 0)
                {
                    randomRoom = Random.Range(0, availableRooms.Count); // Gets the index of the available rooms
                }
                else
                {
                    randomRoom = 0;
                }
            }

            var newRoom = Instantiate(
                rooms[randomRoom].room, 
                new Vector3(i * offset.x, 0, -j * offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>(); // Instantiates the room

            newRoom.UpdateRoom(cell.status); // Sets the door statuses

            newRoom.name = $"Room {i}-{j}";
        }

        /*for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i + j * size.x];
                if (currentCell.visited)
                {
                    int randomRoom = -1; // Gets a random room
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++) // Loops through each room prefab and its rules
                    {
                        for (int x = 0; x < maxRooms; x++)
                        {
                            int p = rooms[k].ProbabilityOfSpawning(x); // Checks rules for current position
                            Debug.Log(rooms[k] + " " + p);

                            if (p == 2) // If the room has to spawn at a certain position
                            {
                                randomRoom = k;
                                break;
                            }
                            else if (p == 1) // If the room can spawn
                            {
                                availableRooms.Add(k);
                            }
                        }
                    }

                    if (randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = Random.Range(0, availableRooms.Count);
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }
                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>(); // Instantiates the room
                    newRoom.UpdateRoom(currentCell.status); // Sets the door statuses

                    newRoom.name = " " + i + "-" + j;
                }
            }
        }*/
    }

    void MazeGenerator()
    {
        board = new List<Cell>();
        cells = new List<Cell>();

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

        while(k < maxRooms) // We don't need a giant maze
        {
            k++;

            board[currentCell].visited = true;
            board[currentCell].index = k;
            cells.Add(board[currentCell]);

            if (currentCell == board.Count - 1) // If it reaches the last cell in the gris the algorithm will stop
            {
                break;
            }

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
        if (cell -  size.x >= 0 && !board[cell - size.x].visited) // Checks if there is a spot above it(if >= 0 then it won't be on the board) and if that cell has been visited
        {
            neighbours.Add(cell - size.x);
        }

        // Check down neighbour
        if (cell + size.x < board.Count && !board[cell + size.x].visited) // Checks if there is a spot below it(if > board height then won't be on board) and if that cell has been visited
        {
            neighbours.Add(cell + size.x);
        }

        // Check left neighbour
        if (cell % size.x != 0 && !board[cell - 1].visited) // Checks if there is a spot to the left of it(they will be a multiple of size if on left side) and if that cell has been visited
        {
            neighbours.Add(cell - 1);
        }



        // Check right neighbour
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited) // Checks if there is a spot to the right of it(the spot + 1 will be the size of the row) and if that cell has been visited
        {
            neighbours.Add(cell + 1);
        }

        return neighbours;
    }
}
