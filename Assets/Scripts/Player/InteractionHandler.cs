using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {

    //_______________________________________________________________________________________

    //_______________________________________________________________________________________
    //The player
    private PlayerHandler player;

    //The camera attached to the player;
    private Camera playerCamera;

    //The main menu
    private GameObject mainMenuObject;
    private GameObject blockSpawnMenuObject;

    //The distance the player can interact with objects from
    private float f_interactionDistance;

    //The previously selected objects
    private GameObject[] prevItems;
    private int prevItemsCounter;

    //State if a player is holding down the interact key after an object was interacted with
    private bool b_objectSelected;

    //Status of object spawn menu
    private bool b_spawnMenuActive = true;

    // Compound of all the interactable layers
    private int layerMask;
    private int layerHit;

    //Information pertaining to the object hit
    RaycastHit hit;

    //The backend of all the audio processing
    protected AudioEngine audioEngine;

    //Enumeration for the state - 0 = player, 1 = menu
    private int windowState;

    //When the player selects a socket this draws a line renderer to tell the player that a socket is selected
    private GuideCable guideCable;

    //_______________________________________________________________________________________

    //_______________________________________________________________________________________

    void Start()
    {
        //Zeroing
        prevItems = new GameObject[2];
        prevItemsCounter = 0;
        b_objectSelected = false;

        //Layermask compounding
        int blockerMask = 1 << LAYER.BLOCKER;
        int controlMask = 1 << LAYER.CONTROL;
        int socketMask = 1 << LAYER.SOCKET;
        int caseMask = 1 << LAYER.CASE;

        layerMask = blockerMask | controlMask | socketMask | caseMask;

        //Connect to the audioEngine
        audioEngine = (AudioEngine)FindObjectOfType(typeof(AudioEngine));

        //Default window state - player
        windowState = 0;

        //Configure the menus
        mainMenuObject = GameObject.Find("UI").transform.Find("MainMenu").gameObject;
        blockSpawnMenuObject = GameObject.Find("UI").transform.Find("BlockSpawnMenu").gameObject;

        //Get the player
        player = GameObject.Find("Player").GetComponent<PlayerHandler>();
        playerCamera = player.playerCamera;
        f_interactionDistance = player.f_interactionDistance;

        //Lock the cursor to the centre of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Guide cable attached to player for indicating when a socket is selected
        guideCable = GameObject.Find("GuideCable").GetComponent<GuideCable>();
        guideCable.initialise(playerCamera, f_interactionDistance, layerMask);
    }

    //Used in Player.update() - rate dependent on fps, ensures that where you're looking is what will be selected
    void Update()
    {
        switch(windowState)
        {
            case 0:
                //Interactions
                if (!b_objectSelected && Input.GetMouseButtonDown(0)) // Change the interact key to mouse clicks later on
                {
                    selectObject();
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                        deleteObject();
                    }
                }
                else if (b_objectSelected && Input.GetMouseButtonUp(0))
                {
                    deselectObject();
                }
                else if (b_objectSelected)
                {
                    updateSelectedObject();
                }

                if(guideCable.isDrawing())
                {
                    guideCable.draw();
                    if (Input.GetMouseButtonDown(1))
                    {
                        guideCable.release();
                    }
                }


                //Access the menu
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    openMainMenu();
                }

                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(!b_spawnMenuActive)
                    {
                        openBlockSpawnMenu();
                        b_spawnMenuActive = true;
                    }
                    else
                    {
                        closeBlockSpawnMenu();
                        b_spawnMenuActive = false;
                    }
                }

                spawnBlock();

                //Movements
                if (Input.GetKey("w"))
                {
                    player.move(0);
                }
                if (Input.GetKey("a"))
                {
                    player.move(1);
                }
                if (Input.GetKey("s"))
                {
                    player.move(2);
                }
                if (Input.GetKey("d"))
                {
                    player.move(3);
                }

                //Rotate the player
                player.rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                break;

            case 1:
                //Access the menu
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    closeMainMenu();
                }
                break;
    }

    }

    private void selectObject()
    {
        //Shoot a ray and detect what's in front of the player - Proceed if an interactable was hit
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, f_interactionDistance, layerMask))
        {
            prevItems[prevItemsCounter] = hit.transform.gameObject; //Keep a reference of the object hit
            print(hit.transform.gameObject.name.ToString()); // Tell us what it was
            b_objectSelected = true; // An object has been selected

            layerHit = hit.transform.gameObject.layer; // Take a copy of the object's layer number

            switch(layerHit)
            {
                case LAYER.CONTROL:
                    prevItems[prevItemsCounter].GetComponent<Control>().select(playerCamera.transform.position + playerCamera.transform.forward * 2);
                    break;
                case LAYER.SOCKET:
                    guideCable.startDrawing();

                    if (prevItems[0] == null || prevItems[1] == null) break;
                    CoreSocketCasing selectedSocket = prevItems[prevItemsCounter].GetComponent<CoreSocketCasing>();
                    CoreSocketCasing prevSelectedSocket;

                    if (prevItemsCounter == 1) prevSelectedSocket = prevItems[0].GetComponent<CoreSocketCasing>();
                    else prevSelectedSocket = prevItems[1].GetComponent<CoreSocketCasing>();

                    //Cannot connect to other socket in same component
                    if (prevSelectedSocket != null && selectedSocket.GetComponentInParent<CoreCasing>() != prevSelectedSocket.GetComponentInParent<CoreCasing>())
                    {
                        //If input to output
                        if (((selectedSocket is AudioInputSocketCasing && prevSelectedSocket is AudioOutputSocketCasing) ||
                             (selectedSocket is AudioOutputSocketCasing && prevSelectedSocket is AudioInputSocketCasing)) || 
                            ((selectedSocket is DataInputSocketCasing && prevSelectedSocket is DataOutputSocketCasing) ||
                            (selectedSocket is DataOutputSocketCasing && prevSelectedSocket is DataInputSocketCasing)))
                        {
                            audioEngine.createConnection(selectedSocket, prevSelectedSocket);
                            print("Input/Output audio combo found");
                            prevItems[0] = null;
                            prevItems[1] = null;
                            guideCable.release();
                        }
                    }
                    //If same socket is selected twice
                    else if (prevItems[0].GetComponent<CoreSocketCasing>() == prevItems[1].GetComponent<CoreSocketCasing>())
                    {
                        print("Connection cleared");
                        audioEngine.destroyConnection(selectedSocket);
                        prevItems[0] = null;
                        prevItems[1] = null;
                        guideCable.release();
                    }

                    break;
                case LAYER.CASE:
                    break;
            }
        }
        else
        {
            guideCable.release();
        }
    }

    private void updateSelectedObject()
    {
        switch (layerHit)
        {
            case LAYER.CONTROL:
                float distanceFromObject = Vector3.Distance(player.transform.position, prevItems[prevItemsCounter].transform.position);

                if (distanceFromObject < f_interactionDistance * 2.0f)
                {
                    prevItems[prevItemsCounter].GetComponent<Control>().interact(playerCamera.transform.position + playerCamera.transform.forward * 2);
                }
                else
                {
                    deselectObject();
                }
                break;
            case LAYER.SOCKET:

                break;
            case LAYER.CASE:

                //Move the case
                prevItems[prevItemsCounter].GetComponent<CoreCasing>().move(playerCamera.transform.position + playerCamera.transform.forward * 1.5f, playerCamera.transform.rotation);

                break;
        }
    }

    private void deselectObject()
    {
        switch (layerHit)
        {
            case LAYER.CONTROL:
                prevItems[prevItemsCounter].GetComponent<Control>().release();
                break;
            case LAYER.SOCKET:
                break;
            case LAYER.CASE:
                break;
        }
        
        b_objectSelected = false;
        ++prevItemsCounter;
        if (prevItemsCounter > prevItems.Length - 1)
            prevItemsCounter = 0;
    }

    private void deleteObject()
    {
        switch (layerHit)
        {
            case LAYER.CASE:
                hit.transform.gameObject.GetComponent<CoreCasing>().delete();
                Destroy(hit.transform.gameObject);
                break;
        }

        b_objectSelected = false;
    }

    public void openMainMenu()
    {
        windowState = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        audioEngine.pause();
        mainMenuObject.SetActive(true);
        mainMenuObject.GetComponent<Menu>().onOpen();

        if (b_spawnMenuActive) closeBlockSpawnMenu();
    }

    public void closeMainMenu()
    {
        windowState = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioEngine.play();
        mainMenuObject.SetActive(false);

        if (b_spawnMenuActive) openBlockSpawnMenu();
    }

    public void openBlockSpawnMenu()
    {
        blockSpawnMenuObject.SetActive(true);
    }

    public void closeBlockSpawnMenu()
    {
        blockSpawnMenuObject.SetActive(false);
    }

    public void spawnBlock()
    {
        int blockID = -1;

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                blockID = i;
                break;
            }
        }

        Vector3 inFrontOfPlayer = playerCamera.transform.position + playerCamera.transform.forward * 2;
        Quaternion playerRotation = player.transform.rotation;

        switch(blockID)
        {
            case 1:
                Instantiate(Resources.Load("Prefabs/Modules/Speaker") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 2:
                Instantiate(Resources.Load("Prefabs/Modules/Splitter") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 3:
                Instantiate(Resources.Load("Prefabs/Modules/Mixer") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 4:
                Instantiate(Resources.Load("Prefabs/Modules/Oscillator") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 5:
                Instantiate(Resources.Load("Prefabs/Modules/Amplifier") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 6:
                Instantiate(Resources.Load("Prefabs/Modules/LFO") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 7:
                Instantiate(Resources.Load("Prefabs/Modules/Sequencer") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 8:
                Instantiate(Resources.Load("Prefabs/Modules/DataSplitter") as GameObject, inFrontOfPlayer, playerRotation);
                break;
            case 9:
                Instantiate(Resources.Load("Prefabs/Modules/Metronome") as GameObject, inFrontOfPlayer, playerRotation);
                break;
        }
    }
}
