using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GenerationState
{
    Idle,
    GeneratingRooms,
    GeneratingLighting,

    GeneratingSpawn,
    GeneratingExit,
    GeneratingChair,

    GeneratingBarrier
}

public class GenerationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform WorldGrid;
    [SerializeField] List<GameObject> RoomTypes; // prefabs rooms
    [SerializeField] List<GameObject> LightTypes; // prefabs lights
    [SerializeField] int mapSize = 16; // Size of map
    [SerializeField] Slider MapSizeSlider, EmptinessSlider, BrightnessSlider;
    [SerializeField] Button GenerateButton;
    [SerializeField] GameObject E_Room;
    [SerializeField] GameObject B_Room; // Barrier room type
    [SerializeField] GameObject SpawnRoom, ExitRoom, ChairRoom;

    public List<GameObject> GeneratedRooms; // Storing the rooms that were already generated

    [SerializeField] GameObject PlayerObject, MainCameraObject;
    [SerializeField] GameObject PlayerCanvas, MainCanvas;

    [Header("Settings")]
    public int mapEmpitiness; // chance of empty room
    public int mapBrightness; // chance of light type spawning in
    private int mapSizeSqr; // Square root mapSize
    private float currentPosX, currentPosZ, currentPosTracker, currentRoom; // These will keep track of our position of the room to be generated
    public float roomSize = 7;
    private Vector3 currentPos; // The current position of the room to be generated
    public GenerationState currentState; // the current state of generation

    private void Update() 
    {
        mapSize = (int)Mathf.Pow(MapSizeSlider.value, 4);

        mapSizeSqr = (int)Mathf.Sqrt(mapSize);

        mapEmpitiness = (int)EmptinessSlider.value;

        mapBrightness = (int)BrightnessSlider.value;
    }
    public void ReloadWorld() // Reload world
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current loaded scene
    }

    public void GenerateWorld() // Creates world
    {
        for (int i = 0; i < mapEmpitiness; i++)
        {
            RoomTypes.Add(E_Room); // Adds empty rooms to the RoomType array
        }

        GenerateButton.interactable = false;

        for (int state = 0; state < 7; state++)
        {
            for (int i = 0; i < mapSize; i++)
            {
                if (currentPosTracker == mapSizeSqr) // Move the position back to the beginning of the grid, so it can go upwards
                {
                    if(currentState == GenerationState.GeneratingBarrier) GenerateBarrier(); // Right
                    
                    currentPosX = 0;
                    currentPosTracker = 0;

                    currentPosZ += roomSize;
                    
                    if(currentState == GenerationState.GeneratingBarrier) GenerateBarrier(); // Left
                }

                currentPos = new(currentPosX, 0, currentPosZ);
            
                switch (currentState) {
                    
                    case GenerationState.GeneratingRooms:
                        GeneratedRooms.Add(Instantiate(RoomTypes[Random.Range(0, RoomTypes.Count)], currentPos, Quaternion.identity, WorldGrid)); // Instantiates the current room type at the currenPos
                    break;
                    
                    case GenerationState.GeneratingLighting:
                        int lightSpawn = Random.Range(-1, mapBrightness);

                        if(lightSpawn == 0)
                        Instantiate(LightTypes[Random.Range(0, LightTypes.Count)], currentPos, Quaternion.identity, WorldGrid); // Instantiates the current room type at the currenPos
                    break;

                    case GenerationState.GeneratingBarrier:
                    
                    if (currentRoom <= mapSizeSqr && currentRoom >= 0)
                    {
                        GenerateBarrier(); // Bottom of map
                    }
                    if (currentRoom <= mapSize && currentRoom >= mapSize - mapSizeSqr)
                    {
                        GenerateBarrier(); // Top of map
                    }

                    break;
                }
                currentRoom++;
                currentPosTracker++; // Keeps track of the position X, without using the room size
                currentPosX += roomSize; // Adds more position to the currenPosX, which makes it go to the right a bit more
            }
            NextState();

            switch(currentState)
            {
                case GenerationState.GeneratingExit:

                    int roomToReplace = Random.Range(0, GeneratedRooms.Count);

                    GameObject exitRoom = Instantiate(ExitRoom, GeneratedRooms[roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                    Destroy(GeneratedRooms[roomToReplace]);

                    GeneratedRooms[roomToReplace] = exitRoom;
                    
                    break;
                
                case GenerationState.GeneratingSpawn:

                    int _roomToReplace = Random.Range(0, GeneratedRooms.Count);
                    
                    spawnRoom = Instantiate(SpawnRoom, GeneratedRooms[_roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                    Destroy(GeneratedRooms[_roomToReplace]);

                    GeneratedRooms[_roomToReplace] = spawnRoom;
                   
                    break;
                
                case GenerationState.GeneratingChair:

                    int __roomToReplace = Random.Range(0, GeneratedRooms.Count);
                    
                    GameObject chairRoom = Instantiate(ChairRoom, GeneratedRooms[__roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                    Destroy(GeneratedRooms[__roomToReplace]);

                    GeneratedRooms[__roomToReplace] = chairRoom;
                   
                    break;
            }
        }
    }

    public GameObject spawnRoom;

    public void SpawnPlayer()
    {
        PlayerObject.SetActive(false);
        PlayerCanvas.SetActive(false);

        PlayerObject.transform.position = new Vector3(spawnRoom.transform.position.x, 3f, spawnRoom.transform.position.z);

        PlayerObject.SetActive(true);
        PlayerCanvas.SetActive(true);

        MainCameraObject.SetActive(false);
        MainCanvas.SetActive(false);
    }

    public void DisablePlayer()
    {
        MainCameraObject.SetActive(true);
        MainCanvas.SetActive(true);
        
        PlayerObject.SetActive(false);
        PlayerCanvas.SetActive(false);
        
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
        
        Debug.Log("Player disabled");
    }

    public void NextState()
    {
        currentState++; // Goes to the next state

        // Resetting our variables
        currentRoom = 0;
        currentPosX = 0;
        currentPosZ = 0;
        currentPosTracker = 0;
        currentPos = Vector3.zero;
    }

    public void WinGame()
    {
        SceneManager.LoadScene(2);
        Debug.Log("Player got out of level 01");
    }

    public void GenerateBarrier()
    {
        currentPos = new(currentPosX, 0, currentPosZ);

        Instantiate(B_Room, currentPos, Quaternion.identity, WorldGrid);
    }
}
