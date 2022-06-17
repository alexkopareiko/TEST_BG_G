using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum MOVE : int
{
    UP = 0,
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3
}

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab = null;
    public GameObject greenZonePrefab = null;
    public GameObject dangerZonePrefab = null;
    public GameObject floor = null;
    public List<Vector2Int> track = new List<Vector2Int>();
    public int initialDistanceForDanger = 4;


    [SerializeField]
    private int widthOfMaze = 0;

    [SerializeField]
    private int heightOfMaze = 0;

    private int[, ] maze;

    private int initialX = 1;
    private int initialZ = 1;
    private int finishX;
    private int finishZ;



    // Start is called before the first frame update
    void Start()
    {
        widthOfMaze = (int)floor.transform.localScale.x;
        heightOfMaze = (int)floor.transform.localScale.z;
        maze = new int[widthOfMaze, heightOfMaze];
        finishX = widthOfMaze - 2;
        finishZ = heightOfMaze - 2;

        MazeOneInitialization();

        MazeGenerateObstacles();
        // MazeMakeBorders();
        Walker();
        DrawMaze();
        InstantiateGreenZone();
        GenerateDangerZones();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void DrawMaze() {
        for(int z = 0; z < heightOfMaze; z++) {
            for(int x = 0; x < widthOfMaze; x++) {
                if(maze[z, x] == 1) {
                    Vector3 pos = new Vector3((float)x, wallPrefab.transform.position.y, (float)z);
                    Instantiate(wallPrefab, pos, wallPrefab.transform.rotation);
                }
            } 
        }
    }

    void MazeOneInitialization() {
        for(int z = 0; z < heightOfMaze; z++) {
            for(int x = 0; x < widthOfMaze; x++) {
                maze[z, x] = 1;
            } 
        }
    }

    void MazeMakeBorders() {
        for(int z = 0; z < heightOfMaze; z++) {
            for(int x = 0; x < widthOfMaze; x++) {
                if(z == 0 || z == heightOfMaze - 1 || x == 0 || x == widthOfMaze - 1)
                maze[z, x] = 1;
            } 
        }
    }

    void MazeGenerateObstacles(){
        for(int z = 1; z < heightOfMaze - 1; z++) {
            for(int x = 1; x < widthOfMaze - 1; x++) {
                maze[z, x] = Random.Range(0, 2);
            } 
        }
        maze[finishZ, finishX] = 0;
    }

    void InstantiateGreenZone() {
        Vector3 pos = new Vector3((float)finishX, greenZonePrefab.transform.position.y, (float)finishZ);
        Instantiate(greenZonePrefab, pos, greenZonePrefab.transform.rotation);
    }

    void Walker() {
        Vector2Int currentPos = new Vector2Int(initialX, initialZ);
        track.Add(currentPos);
        maze[currentPos.x, currentPos.y] = 0;
        int i = 0;
        int lastIterationMoved = 0;
        Vector2Int final = new Vector2Int(finishX, finishZ);
        while(currentPos != final && i < 1000) {
            MOVE whereToMove = (MOVE)Random.Range(0, 4);
            //Debug.Log(whereToMove);
            if(whereToMove == MOVE.UP) {
                Vector2Int newPos = new Vector2Int(currentPos.x, currentPos.y + 1);
                currentPos = CheckAndAdd(newPos, currentPos);
                if(newPos == currentPos) lastIterationMoved = i;
                currentPos = HardMove(i, lastIterationMoved, currentPos);
            }
            else if(whereToMove == MOVE.DOWN) {
                Vector2Int newPos = new Vector2Int(currentPos.x, currentPos.y - 1);
                currentPos = CheckAndAdd(newPos, currentPos);
                if(newPos == currentPos) lastIterationMoved = i;
                currentPos = HardMove(i, lastIterationMoved, currentPos);
            }
            else if(whereToMove == MOVE.LEFT) {
                Vector2Int newPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                currentPos = CheckAndAdd(newPos, currentPos);
                if(newPos == currentPos) lastIterationMoved = i;
                currentPos = HardMove(i, lastIterationMoved, currentPos);
            }
            else if(whereToMove == MOVE.RIGHT) {
                Vector2Int newPos = new Vector2Int(currentPos.x + 1, currentPos.y);
                currentPos = CheckAndAdd(newPos, currentPos);
                if(newPos == currentPos) lastIterationMoved = i;
                currentPos = HardMove(i, lastIterationMoved, currentPos);
            }
            maze[currentPos.x, currentPos.y] = 0;
            i++;
            //Debug.Log("Result position " + currentPos);
            //Debug.Log("i " + i);
            //Debug.Log("-------------------------");
            
        }
    }

    // Move If Stuck
    Vector2Int HardMove(int currentIteration, int lastIterationMoved, Vector2Int currentPos) {
        if(currentIteration - lastIterationMoved >= 2) {
            Vector2Int final = new Vector2Int(finishX, finishZ);
            Vector2Int diff = final - currentPos;
            if(diff.x >= 1) currentPos += Vector2Int.right;
            else if(diff.y >= 1) currentPos += Vector2Int.up;
            track.Add(currentPos);
            //Debug.Log("Hard Move");
        }
        return currentPos;
    }

    Vector2Int CheckAndAdd(Vector2Int newPos, Vector2Int currentPos) {
        if(!track.Contains(newPos) && !IsItBorder(newPos)) {
            track.Add(newPos);
            //Debug.Log("I move");
            return newPos;
        }
        //Debug.Log("I won't move");
        return currentPos;
    }

    bool IsItBorder(Vector2Int newPos) {
        if(newPos.x == 0 || newPos.x == widthOfMaze - 1) {
            //Debug.Log("Its a border");
            return true;
        }
        else if(newPos.y == 0 || newPos.y == heightOfMaze - 1) {
            //Debug.Log("Its a border");
            return true;
        }
        return false;
    }

    void GenerateDangerZones() {
        for(int z = 0; z < heightOfMaze; z++) {
            for(int x = 0; x < widthOfMaze; x++) {
                if(maze[z, x] == 0 && (z != finishZ && x != finishX) 
                    && z > initialDistanceForDanger && x > initialDistanceForDanger)
                {
                    int needGenerate = Random.Range(0, 10);
                    if(needGenerate == 2) {
                        Vector3 pos = new Vector3((float)x, dangerZonePrefab.transform.position.y, (float)z);
                        Instantiate(dangerZonePrefab, pos, dangerZonePrefab.transform.rotation);
                    }
                }
            } 
        }
    }
}

