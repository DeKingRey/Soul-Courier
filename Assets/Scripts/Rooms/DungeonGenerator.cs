using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell // Holds the information of every cell within the maze
    {
        public bool visited = false; // Whether the cell has been visited or not
        public bool[][] status = new bool[4][]; // Status of each entrance to the cell

        public int index = -1;

        public bool occupied = false;

        public Cell()
        {
            for (int i = 0; i < 4; i++)
            {
                status[i] = new bool[1];
            }
        }
    }

    [System.Serializable]
    public class Rule // Creates rules for generating rooms
    {
        public GameObject room;
        public int minPos;
        public int maxPos; // These numbers will be based off the amount of rooms on the floor e.g. min = 5, max = 15 so room spawns between 5 and 15

        public bool compulsory; // Whether the room has to spawn at a certain position or not

        public Vector2 offset; // Offset of the room and may change depending on the rooms size
        public Vector2Int roomSize; // Default room size is 3*3

        public int spawnChance;

        // Make the rooms have to spawn at a certain position, though the grid can differentiate depending on the layout
        // Use the current iteration to decide when the room must spawn
        // As the max amount of rooms is 15, for example the final room must spawn between room 15 and 15(so spawns at 15)
        // Maybe do this with a foreach loop

        public int CanSpawn(int i)
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
    public int defaultOffset;
    public Rule[] rooms; 
    public int maxRooms;

    List<Cell> board;
    List<Cell> cells;

    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        bool[] compulsoryUsed = new bool[rooms.Length]; // Creates an array of bools to check if the compulsory rooms have been placed
        foreach (Cell cell in cells)
        {
            if (cell.occupied) continue; // Skips the iteration if the cell is occupied

            int i = board.IndexOf(cell) % size.x; // Gets the column, e.g. 3*3 grid, 6 % 3(3 is the width) = 0, and 6 would be in the 0th column
            int j = board.IndexOf(cell) / size.x; // Gets the row, e.g. 3*3 grid 6 / 3 = 2, 6 would be on the 2nd index 

            int randomRoom = -1; // Gets a random room
            List<int> availableRooms = new List<int>();

            for (int k = 0; k < rooms.Length; k++) // Loops through each room prefab and its rules
            {
                int p = rooms[k].CanSpawn(cell.index); // Checks rules for current position

                if (CheckRoomPlacement(i, j, rooms[k].roomSize, rooms[k].compulsory))
                    {
                    if (p == 2 && !compulsoryUsed[k]) // If the room has to spawn at a certain position
                    {
                        randomRoom = k;
                        compulsoryUsed[k] = true; // Compulsory room won't spawn again if it is used since this loop will go through multiple times as there are two for loops
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
                    List<int> potentialRooms = new List<int>();
                    foreach (int availableRoom in availableRooms)
                    {
                        int chance = rooms[availableRoom].spawnChance;
                        for (int a = 0; a < chance; a++)
                        {
                            potentialRooms.Add(availableRoom);  
                        }
                    }
                    randomRoom = potentialRooms[Random.Range(0, potentialRooms.Count)]; // Gets the random room 
                }
                else
                {
                    randomRoom = 0;
                }
            }

            #region Spawning the Room

            Vector2 offset = rooms[randomRoom].offset;
            float offsetX = i * defaultOffset + (offset.x - defaultOffset) / 2f;
            float offsetY = j * defaultOffset + (offset.y - defaultOffset) / 2f;

            var newRoom = Instantiate(
                rooms[randomRoom].room, 
                new Vector3(offsetX, 0, -offsetY),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>(); // Instantiates the room

            Vector2Int rSize = rooms[randomRoom].roomSize;

            TakeBoardSpot(i, j, rSize);

            #region Size Spawning

            if (rSize != new Vector2Int(1, 1) || rooms[randomRoom].compulsory) // If the room isn't the default size
            {
                int index = board.IndexOf(cell); // Gets the cells index

                // Sets the new sizes of the status array
                cell.status[0] = new bool[rSize.x];
                cell.status[1] = new bool[rSize.x];

                // Sets the new sizes of the status array
                cell.status[2] = new bool[rSize.y];
                cell.status[3] = new bool[rSize.y];

                for (int n = 0; n < rSize.x; n++) // Loops through the rooms horizontal size
                {
                    int upIndex = (index + n) - size.x; // Gets the index of the cell above
                    if (upIndex >= 0 && cells.Contains(board[upIndex]))
                    {
                        if (System.Array.Exists(board[upIndex].status[1], s => s)) // Checks every door in the cells array
                        {
                            cell.status[0][n] = true; // Checks if there is a cell above the current one
                        }
                    }

                    int bottomIndex = (index + n); // Gets the horizontal offset of the room
                    int downIndex = bottomIndex + (rSize.y * size.x); // Gets the lowest most position
                    if (downIndex < board.Count && cells.Contains(board[downIndex])) // Checks the actual cell below
                    {
                        if (System.Array.Exists(board[downIndex].status[0], s => s))
                        {
                            cell.status[1][n] = true; // Checks if there is a cell below the current one
                        }
                    } 
                   
                } 

                for (int n = 0; n < rSize.y; n++) // Loops through the rooms vertical size
                {
                    int leftIndex = index + (size.x * n);
                    if (leftIndex % size.x != 0)
                    {
                        if (leftIndex - 1 >= 0 && cells.Contains(board[leftIndex - 1]))
                        {
                            if (System.Array.Exists(board[leftIndex - 1].status[3], s => s))
                            {
                                cell.status[2][n] = true;
                            }
                        }
                    }
                    // e.g. Index = 0, size.x = 3, n = 0, 1, rSize.x = 2, 0 + (3 * 0) + (2 - 1) = 1, 0 + (3 * 1) + (2 - 1) = 4
                    int rightIndex = index + (size.x * n) + (rSize.x - 1); // Accounts for when the room is wide and the right cell is the big room
                    if (rightIndex + 1 < board.Count && cells.Contains(board[rightIndex + 1])) // Checks the actual cell to the right
                    {
                        if (System.Array.Exists(board[rightIndex + 1].status[2], s => s))
                        {                            
                            cell.status[3][n] = true;
                        }
                    }
                }
            }
            #endregion

            newRoom.UpdateRoom(cell.status); // Sets the door statuses
            newRoom.name = $"{rooms[randomRoom].room.gameObject.name} {i}-{j}";
        #endregion
    }
}

    bool CheckRoomPlacement(int x, int y, Vector2Int roomSize, bool required)
    {
        // Loops through every tile the room may take up
        for (int dx = 0; dx < roomSize.x; dx++)
        {
            for (int dy = 0; dy < roomSize.y; dy++)
            {
                int checkX = x + dx;
                int checkY = y + dy; // Checks the positions that will be taken up if it was placed
                // eg. x = 2, y = 3, room size = (2, 1) dy will only be 0, and dx will be 0 then 1, so checkY = 3 + 0 = 0, and dx may equal 2 + 1 = 3, meaning it takes up that spot too 

                if (checkX >= size.x || checkY >= size.y) return false; // Returns false if it goes out of bounds

                if (checkX == size.x - 1 && dx > 0 || checkY == size.y - 1 && dy > 0) return false; // Makes sure the final room spawns
                
                
                int index = checkX + checkY * size.x;
                if (board[index].occupied || board[index] == cells[cells.Count - 1] && !required) return false; // Checks that the cells aren't already visited and it doesn't take the final cell
            }
        }
        return true;
    }

    void TakeBoardSpot(int x, int y, Vector2Int roomSize)
    {
        // Loops through the spots it has taken
        for (int dx = 0; dx < roomSize.x; dx++)
        {
            for (int dy = 0; dy < roomSize.y; dy++)
            {
                int spotX = x + dx;
                int spotY = y + dy; // Spots it will take up

                int index = spotX + spotY * size.x;
                board[index].occupied = true; // Takes up the spot
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>(); // Creates the board of cells
        cells = new List<Cell>();

        for (int i = 0; i < size.x; i++) // This creates the board, with the width and height of the grid
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        List<int> frontier = new List<int>(); // A frontier is an adjacent unvisited cell in the maze

        int startCell = startPos; // Sets the starting cell
        AddFrontiers(startCell, frontier); // Adds all adjacent unvisited valid neighbours of the current cell

        int k = 0; // Keeps track what loop we're at

        while (frontier.Count > 0 && k < maxRooms) // We don't need a giant maze, makes sure there are frontiers
        {
            k++;

            int randIndex = Random.Range(0, frontier.Count); 
            int currentCell = frontier[randIndex]; // Chooses a random frontier cell
            frontier.RemoveAt(randIndex); // The cell is no longer a frontier since it is added to the active cells

            board[currentCell].visited = true; //  Sets the cell to visited
            board[currentCell].index = k; // Gives the index to the cell
            cells.Add(board[currentCell]); // Adds the cell to the cells list

            List<int> visitedNeighbours = GetVisitedNeighbours(currentCell); //  Gets all the adjacent visited cells

            if (visitedNeighbours.Count > 0) // Makes sure the list isn't empty
            {
                int connectCell = visitedNeighbours[Random.Range(0, visitedNeighbours.Count)]; // Chooses a random neighbour to connect to
                OpenDoor(currentCell, connectCell); // Opens the correct doors of the current cell and the connecting cell
            }
            AddFrontiers(currentCell, frontier); // Gets the frontiers of the current cell
        }
        GenerateDungeon();
    }

    void AddFrontiers(int cell, List<int> frontier)
    {
        int[] directions = { -size.x, size.x, -1, 1 }; // Up down left and right directions

        foreach (int dir in directions) // Loops through each direction
        {
            int neighbour = cell + dir; // Gets the neighbouring cell by adding the direction to the current cell   

            if (neighbour >= 0 && neighbour < board.Count && !board[neighbour].visited) // If the neighbour is valid
            {
                // Checks the neighbour doesn't wrap to the above or below row, if so continue skips the loop iteration
                if (dir == -1 && cell % size.x == 0) continue; // Modulo cell since that may be the left most
                if (dir == 1 && cell % size.x == size.x - 1) continue; // Modulo the cell to see if its the right most

                if (!frontier.Contains(neighbour)) // Makes sure the frontier doesn't exist
                    frontier.Add(neighbour); // Adds the neighbour to the frontier(which affects the frontier list everywhere)
            }
        }
    }

    List<int> GetVisitedNeighbours(int cell)
    {
        List<int> visited = new List<int>();
        int[] directions = { -size.x, size.x, -1, 1 }; // All direction additions

        foreach (int dir in directions) // Loops throguh directions
        {
            int neighbor = cell + dir; // Finds the neighbour by adding the direction to the cellz

            if (neighbor >= 0 && neighbor < board.Count && board[neighbor].visited) // Ensures the neighbour is valid and has been visited
            {
                // Prevents the neighbour from wrapping horizontally
                if (dir == -1 && cell % size.x == 0) continue;
                if (dir == 1 && cell % size.x == size.x - 1) continue;

                visited.Add(neighbor); // Adds the visited neighbour to the list
            }
        }
        return visited;
    }

    void OpenDoor(int from, int to)
    {
        int dir = to - from; // Gets the direction of the connecting cell, e.g. to 4 from 7, 7 - 4 = 3, which is down

        if (dir == -size.x) // If the direction is up
        {
            board[from].status[0][0] = true; // Opens the up door of the current cell
            board[to].status[1][0] = true; // Opens the down door of the next cell
        }
        else if (dir == size.x) // If the direction is down
        {
            board[from].status[1][0] = true; // Down door of current cell 
            board[to].status[0][0] = true;   // Up door of connecting cell
        }
        else if (dir == -1) // If the direction is left
        {
            board[from].status[2][0] = true; // Left door of current cell
            board[to].status[3][0] = true;   // Right door of connecting cell
        }
        else if (dir == 1) // If the direction is right
        {
            board[from].status[3][0] = true; // Right door of current cell
            board[to].status[2][0] = true;   // Left door of current cell
        }
    }
}
