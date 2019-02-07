using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Player;
using Enemies;

public class EventsManager : MonoBehaviour
{
	//All of the other classes
	public TextManagement TextManagement;
	public ButtonHandler ButtonHandler;
	public Animator SceneFadeAnimator;
	public DialogueManager DialogueManager;
	public PlayerManager PlayerManager;
	public EnemyManager EnemyManager;

	public Transform player;
	public Rigidbody2D rb2D;

	public Transform mainCamera;
	public Text loadingText;

	public Vector3 playerOffset;
	public Vector3 placeableOffset;
	public Vector3 monsterOffset;
	public Vector3 newMove; //Position to walk the player to
	public float speed; //Speed to move player at
	private bool wasLucky;

	public Events[] eventsArray;

	public CameraLocations[] cameraLocationsArray;
	private int chosenEventIndex;
	private int chosenCameraIndex;

	[SerializeField] private GameObject loadingObject;

	//Chooses an appropriate event from array of events
	public void ChooseEvent()
	{
		bool canDo = false;
		int random = 0;

		while (canDo == false)
		{
			//Choose a random index for the events array
			random = Random.Range(0, eventsArray.Length);
			//Checks to see if the event can be repeated
			canDo = eventsArray[random].repeatable;
		}
		//If the event can be repeated set the event index to random number
		chosenEventIndex = random;
	}

	//Returns the event dialogue index for VIDE
	public int ChooseDialogue()
	{
		return eventsArray[chosenEventIndex].dialogueIndex[Random.Range(0, eventsArray[chosenEventIndex].dialogueIndex.Length - 1)];
	}

	//Returns event and camera information for debugging purposes
	public string GetEventName()
	{
		if (eventsArray[chosenEventIndex] != null)
			return eventsArray[chosenEventIndex].eventName;
		else
			return "Empty";
	}
	public string GetEventType()
	{
		if (eventsArray[chosenEventIndex] != null)
			return eventsArray[chosenEventIndex].type;
		else
			return "Empty";
	}
	public string GetCameraName()
	{
		if (cameraLocationsArray[chosenCameraIndex] != null)
			return cameraLocationsArray[chosenCameraIndex].name;
		else
			return "Empty";
	}
	public string GetCameraPosition()
	{
		if (cameraLocationsArray[chosenCameraIndex] != null)
			return cameraLocationsArray[chosenCameraIndex].cameraPosition.ToString("F3");
		else
			return "Empty";
	}

	//Reads the camera location for the event and puts it into place
	public void LoadCamera()
	{
		chosenCameraIndex = Random.Range(0, cameraLocationsArray.Length);
		mainCamera.position = cameraLocationsArray[chosenCameraIndex].cameraPosition;
		mainCamera.rotation = cameraLocationsArray[chosenCameraIndex].cameraRotation;
	}

	//Determine loadable objects for an event and load them into scene
	public void LoadObjects()
	{
		//Get the type of event
		string caseSwitch = eventsArray[chosenEventIndex].type;

		Debug.Log("Loading event of type: " + caseSwitch);

		switch (caseSwitch)
		{
			case "CHEST":

				//Set loading object container
				loadingObject = GameObject.Find("CHEST001");

				//Move loading object container into position
				loadingObject.transform.position = (mainCamera.position + placeableOffset);
				break;

			case "GHOSTBATTLE":

				//Set loading object container
				loadingObject = GameObject.Find("GHOSTCONTAINER");

				//Choose a random amount of enemies
				int numberEnemies = Random.Range(1, loadingObject.transform.childCount + 1);
				Debug.Log(loadingObject.transform.childCount);
				//Set the scene enemy array size
				EnemyManager.SetSceneEnemeniesSize(numberEnemies);
				Debug.Log("Created an enemy arrray of length: " + EnemyManager.sceneEnemies.Length);

				//Go through numberEnemies amount of childlren of loading object container and set them active
				for (int i = 0; i < numberEnemies; i++)
				{
					loadingObject.transform.GetChild(i).gameObject.SetActive(true);
					//Add the enemy to the scene enemy array
					EnemyManager.AddSceneEnemy(i, loadingObject.transform.GetChild(i).gameObject);
				}

				//Set enemy HP
				EnemyManager.SetEnemyHP();

				//Move loading object container into position of camera
				loadingObject.transform.position = (mainCamera.position + monsterOffset);

				break;

			case "MONSTER1":
				break;
			case "MONSTER2":
				break;
			case "MONSTER3":
				break;
			default:
				break;
		}

	}

	private void SetLoadingText()
	{
		loadingText.text = "You encountered... :" + GetEventType().ToLower();
	}

	//Move loading objects back to unseeable area
	private void ResetLoadedObjects()
	{
		if (loadingObject != null)
		{
			loadingObject.transform.position = new Vector3(-8.27f, 0f, 1.4f);

			if (loadingObject.transform.childCount > 0 && (loadingObject.name != "CHEST001"))
				foreach (Transform child in loadingObject.transform)
					child.gameObject.SetActive(false);
		}
	}

	public void StartEvent()
	{
		//Reset the loaded object from the camera view
		ResetLoadedObjects();

		//Seting loading text
		SetLoadingText();

		//Choose the next event
		ChooseEvent();

		//Load the camera for the chosen event
		LoadCamera();

		//Load the player based on the camera position
		LoadPlayer();

		//Load all objects into the scene based on camera position
		LoadObjects();

		//Set the text and the options in the event box
		//TextManagement.SetTextandOptions();
		DialogueManager.SetUpNewDialogue();

		//Moves the player into the scene
		MovePlayer();

		//Animates the event box to open
		TextManagement.OpenEventBox();
	}

	public void EndEvent()
	{
		TextManagement.CloseEventBox();
	}
	//Checks camera position and moves player into scene outside of camera
	public void LoadPlayer()
	{
		player.position = (new Vector3(mainCamera.position.x, 3, mainCamera.position.z) + playerOffset);
		player.rotation = mainCamera.rotation;
	}

	//Moves player into the camera
	public void MovePlayer()
	{
		//THIS ISNT WORKING!
		player.position += newMove * speed * Time.deltaTime;
	}

	public bool WasLucky()
	{
		Debug.Log("Was player lucky? " + wasLucky);
		return wasLucky;
	}
	//Chooses loot from lootable item list while checking its rarity
	public void ChooseLoot()
	{
		//Get rarity from event and add player lucky bonus
		wasLucky = PlayerManager.IsLucky();

		int eventRarity = eventsArray[chosenEventIndex].tier + System.Convert.ToInt32(wasLucky);

		List<GameObject> canLoot = new List<GameObject>();

		//Add only items that have appropriate rarity to the list
		foreach(GameObject item in PlayerManager.LootableItems)
		{
			if (item.GetComponent<EquipmentStats>().rarity <= eventRarity)
				canLoot.Add(item);
		}

		//Change list to array so we can choose a random index
		GameObject[] canLootArray = canLoot.ToArray();

		//Choose a random index from array
		PlayerManager.LootItem = canLootArray[Random.Range(0, canLootArray.Length - 1)];
	}

	public IEnumerator SceneStart()
	{
		//Fade out Scene and wait three seconds
		SceneFadeAnimator.gameObject.SetActive(true);
		SceneFadeAnimator.SetBool("IsStarted", false);

		//End the event
		EndEvent();
		yield return new WaitForSecondsRealtime(3);

		//Start the scene
		StartEvent();

		//Fade in Scene
		SceneFadeAnimator.SetBool("IsStarted", true);
	}

	public void WrapperSceneIEnumerator()
	{
		StartCoroutine(SceneStart());
	}
}
