using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] Transform WorldGrid;
    [SerializeField] List<GameObject> RoomTypes;
    [SerializeField] int mapSize = 16; // Size of map
    [SerializeField] Slider MapSizeSlider;
    [SerializeField] Button GenerateButton;


    private int mapSizeSqr; // Square root mapSize
    private float currentPosX, currentposZ, currentPosTracker; // These will keep track of our position of the room to be generated
    private float roomSize = 7;
    private Vector3 currentPos; // The current position of the room to be generated

    private void Update() 
    {
        mapSize = (int)Mathf.Pow(MapSizeSlider.value, 4);

        mapSizeSqr = (int)Mathf.Sqrt(mapSize);
    }
    public void ReloadWorld() // Reload world
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current loaded scene
    }

    public void GenerateWorld() // Creates world
    {
        GenerateButton.interactable = false;
        for (int i = 0; i < mapSize; i++)
        {
            if (currentPosTracker == mapSizeSqr) // Move the position back to the beginning of the grid, so it can go upwards
            {
                currentPosX = 0;
                currentPosTracker = 0;
                
                currentposZ += roomSize;
            }

            currentPos = new(currentPosX, 0, currentposZ);

            Instantiate(RoomTypes[Random.Range(0, RoomTypes.Count)], currentPos, Quaternion.identity, WorldGrid); // Instantiates the current room type at the currenPos

            currentPosTracker++; // Keeps track of the position X, without using the room size
            currentPosX += roomSize; // Adds more position to the currenPosX, which makes it go to the right a bit more
        }
    }
}
