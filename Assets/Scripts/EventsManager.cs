using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Enemies;

public class EventsManager : MonoBehaviour
{
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
	public Vector3 newMove;
	public float speed;
	public Events[] eventsArray;
	public CameraLocations[] cameraLocationsArray;
	private int chosenEventIndex;
	private int chosenCameraIndex;
	private int textIndex = 0;
	[SerializeField] private GameObject loadingObject;

	//Chooses an appropriate event
	public void ChooseEvent()
	{
		bool canDo = false;
		int random = 0;
		while (canDo == false)
		{
			random = Random.Range(0, eventsArray.Length);
			canDo = eventsArray[random].repeatable;
			Debug.Log(random);
		}
		chosenEventIndex = random;
	}

	public int ChooseDialogue()
	{
		/*int random = 0;
		random = Random.Range(0, eventsArray[chosenEventIndex].dialogueIndex.Length);
		return random;*/
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
		string caseSwitch = eventsArray[chosenEventIndex].type;

		//PLACEABLE OFFSET NOT WORKING
		switch (caseSwitch)
		{
			case "CHEST":

				loadingObject = GameObject.Find("CHEST001");
				Debug.Log(loadingObject.transform.position);

				loadingObject.transform.position = (mainCamera.position + placeableOffset);
				Debug.Log(loadingObject.transform.position);
				break;

			case "GHOSTBATTLE":

				loadingObject = GameObject.Find("GHOSTCONTAINER");
				int numberEnemies = Random.Range(1, loadingObject.transform.childCount);

				EnemyManager.SetSceneEnemeniesSize(numberEnemies);
				//EnemyManager.sceneEnemies = new GameObject[numberEnemies];
				Debug.Log("Created an enemy arrray of length: " + EnemyManager.sceneEnemies.Length);
				for (int i = 0; i < numberEnemies; i++)
				{
					loadingObject.transform.GetChild(i).gameObject.SetActive(true);
					EnemyManager.AddSceneEnemy(i, loadingObject.transform.GetChild(i).gameObject);
					//EnemyManager.sceneEnemies[i] = loadingObject.transform.GetChild(i).gameObject;
				}

				EnemyManager.SetEnemyHP();

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
		//Resest the array index for the text
		textIndex = 0;
		//Reset the loaded object from the camera view
		ResetLoadedObjects();
		//Reset the option choice
		//TextManagement.optionChoice = 0;
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
		//Debug.Log(player.position);
		player.position = (new Vector3(mainCamera.position.x, 3, mainCamera.position.z) + playerOffset);
		player.rotation = mainCamera.rotation;
		//Debug.Log(player.position);
	}
	//Moves player into the camera
	public void MovePlayer()
	{
		//THIS ISNT WORKING!
		//rb2D.velocity = new Vector2(200, rb2D.velocity.y);
		//rb2D.MovePosition((rb2D.position + newMove) * Time.fixedDeltaTime);
		player.position += newMove * speed * Time.deltaTime;
	}

	public void ChooseLoot()
	{
		int eventRarity = eventsArray[chosenEventIndex].tier;
		List<GameObject> canLoot = new List<GameObject>();

		foreach(GameObject item in PlayerManager.LootableItems)
		{
			if (item.GetComponent<EquipmentStats>().rarity <= eventRarity)
				canLoot.Add(item);
		}

		GameObject[] canLootArray = canLoot.ToArray();
		PlayerManager.LootItem = canLootArray[Random.Range(0, canLootArray.Length - 1)];

		//Debug.Log("You looted " + PlayerManager.lootItem.name);
	}

	public IEnumerator SceneStart()
	{
		//Fade out Scene and wait three seconds
		SceneFadeAnimator.gameObject.SetActive(true);
		SceneFadeAnimator.SetBool("IsStarted", false);
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
