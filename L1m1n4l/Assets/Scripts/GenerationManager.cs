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
    GeneratingExit
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
    [SerializeField] GameObject SpawnRoom, ExitRoom;

    public List<GameObject> GeneratedRooms; // Storing the rooms that were already generated

    [Header("Settings")]
    public int mapEmpitiness; // chance of empty room
    public int mapBrightness; // chance of light type spawning in
    private int mapSizeSqr; // Square root mapSize
    private float currentPosX, currentPosZ, currentPosTracker; // These will keep track of our position of the room to be generated
    private float roomSize = 7;
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

        for (int state = 0; state < 5; state++)
        {
            for (int i = 0; i < mapSize; i++)
            {
                if (currentPosTracker == mapSizeSqr) // Move the position back to the beginning of the grid, so it can go upwards
                {
                    currentPosX = 0;
                    currentPosTracker = 0;

                    currentPosZ += roomSize;
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
                }
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
                    
                    GameObject spawnRoom = Instantiate(SpawnRoom, GeneratedRooms[_roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                    Destroy(GeneratedRooms[_roomToReplace]);

                    GeneratedRooms[_roomToReplace] = spawnRoom;
                   
                    break;
            }
        }
    }

    public void NextState()
    {
        currentState++; // Goes to the next state

        // Resetting our variables
        currentPosX = 0;
        currentPosZ = 0;
        currentPosTracker = 0;
        currentPos = Vector3.zero;
    }
}
