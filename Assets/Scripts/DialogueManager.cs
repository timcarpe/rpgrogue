using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Import this to quickly access Unity's UI classes
using VIDE_Data; //Import this to use VIDE Dialogue's VD class
using RichTextSubstringHelper;
using Combat;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance;

	private EventsManager EventsManager;
	private PlayerManager PlayerManager;
	private EnemyManager EnemyManager;
	//public CombatManager CombatManager;

	public delegate void CombatStartDialogue();
	public CombatStartDialogue CombatStartDialogueCallback;

	public TextMeshProUGUI EVENT_text; //References
	public TextMeshProUGUI[] PLAYER_text; //References
	public KeyCode continueButton; //Button to continue

	private bool keyDown = false;
	private string lastEventText;

	#region Singleton
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("More than one singleton!");
			return;
		}

		Instance = this;
	}
	#endregion

	void Start()
	{
		EnemyManager = EnemyManager.Instance;
		PlayerManager = PlayerManager.Instance;
		EventsManager = EventsManager.Instance;
		/*//Disable UI when starting just in case
		EVENT_text.gameObject.SetActive(false);
		foreach (Text t in PLAYER_text)
			t.transform.parent.gameObject.SetActive(false);

		//Subscribe to some events and Begin the Dialogue
		VD.OnNodeChange += UpdateUI;
		VD.OnEnd += End;
		VD.BeginDialogue(GetComponent<VIDE_Assign>()); //This is the first most important method when using VIDE
		VD.SetNode(EventsManager.ChooseDialogue());*/
		//Disable UI when starting just in case
		EVENT_text.gameObject.SetActive(false);
		foreach (TextMeshProUGUI t in PLAYER_text)
			t.transform.parent.gameObject.SetActive(false);

		//Subscribe to some events and Begin the Dialogue
		VD.OnNodeChange += UpdateUI;
		VD.OnEnd += End;
		VD.BeginDialogue(GetComponent<VIDE_Assign>()); //This is the first most important method when using VIDE
	}

	void Update()
	{
		if (VD.isActive)
		{
			if (!VD.nodeData.isPlayer && Input.GetKeyUp(continueButton))
			{
				if (keyDown)
				{
					keyDown = false;
				}
				else
				{
					VD.Next(); //Second most important method when using VIDE
				}
			}
		}
		else
		{
			if (Input.GetKeyUp(continueButton))
			{
				//Start();
				SetUpNewDialogue();
			}
		}
	}

	public void SelectChoiceAndGoToNext(int playerChoice)
	{
		if (PLAYER_text[0].text == "Continue.")
			VD.Next();
		else
		{
			keyDown = true;
			VD.nodeData.commentIndex = playerChoice; //Setting this when on a player node will decide which node we go next
			VD.Next();
		}
	}
	//Every time VD.nodeData is updated, this method will be called. (Because we subscribed it to OnNodeChange event)
	void UpdateUI(VD.NodeData data)
	{
		StopAllCoroutines();
		WipeAll(); //Turn stuff off first

		if (!data.isPlayer) //For NPC. Activate text gameobject and set its text
		{
			EVENT_text.gameObject.SetActive(true);
			//EVENT_text.text = data.comments[data.commentIndex];
			lastEventText = data.comments[data.commentIndex];
			//Debug.Log(data.comments[data.commentIndex]);
			StartCoroutine(TypeSentence(data.comments[data.commentIndex]));
			//Modified
			PLAYER_text[0].transform.parent.gameObject.SetActive(true);
			PLAYER_text[0].text = "Continue.";
		}
		else //For Player. It will activate the required Buttons and set their text
		{
			EVENT_text.gameObject.SetActive(true);
			//EVENT_text.text = lastEventText;
			StartCoroutine(TypeSentence("What do you want to do?"));

			for (int i = 0; i < PLAYER_text.Length; i++)
			{
				if (i < data.comments.Length)
				{
					PLAYER_text[i].transform.parent.gameObject.SetActive(true);
					PLAYER_text[i].text = data.comments[i];
				}
				else
				{
					PLAYER_text[i].transform.parent.gameObject.SetActive(false);
				}

				PLAYER_text[0].transform.parent.GetComponent<Button>().Select();
			}
		}
	}

	public void SetUpNewDialogue(int node = -1)
	{
		if (node == -1)
			VD.SetNode(EventsManager.ChooseDialogue());
		else
		{
			VD.SetNode(node);
		}
		//Start();
	}
	//Set all UI references off
	void WipeAll()
	{
		EVENT_text.gameObject.SetActive(false);
		foreach (TextMeshProUGUI t in PLAYER_text)
			t.transform.parent.gameObject.SetActive(false);
	}

	//This will be called when we reach the end of the dialogue.
	//Very important that this gets called before we call BeginDialogue again!
	void End(VD.NodeData data)
	{
		//VD.OnNodeChange -= UpdateUI;
		//VD.OnEnd -= End;
		//VD.EndDialogue(); //Third most important method when using VIDE     
		//WipeAll();
		//SetMoveOnButton();
	}

	public void Close()
	{
		VD.EndDialogue(); //Third most important method when using VIDE     
	}

	public void SetMoveOnButton(string text = "empty")
	{
		Debug.Log("Setting up move on text!");

		//RemoveCombatListener();

		StopAllCoroutines();

		WipeAll();
		
		EVENT_text.gameObject.SetActive(true);
		if (text == "empty")
			StartCoroutine(TypeSentence("There is nothing left to do here."));
		else
			StartCoroutine(TypeSentence(text));

		PLAYER_text[3].transform.parent.gameObject.SetActive(true);
		PLAYER_text[3].text = "Move On.";
	}
	//Just in case something happens to this script
	void OnDisable()
	{
		VD.OnNodeChange -= UpdateUI;
		VD.OnEnd -= End;
	}

	public void SetLootText()
	{
		StopAllCoroutines();

		WipeAll();

		string addText = "";

		if (EventsManager.WasLucky())
			addText = " You feel a little lucky";

		EVENT_text.gameObject.SetActive(true);
		StartCoroutine(TypeSentence("You looted a <b>" + PlayerManager.LootItem.GetComponent<EquipmentStats>().itemName + "</b>!" + addText));

		PLAYER_text[0].transform.parent.gameObject.SetActive(true);
		PLAYER_text[0].text = "Continue.";
	}

	public void PlayerDiedText()
	{
		StopAllCoroutines();

		WipeAll();

		SetUpNewDialogue(16);

	}

	public void SetCombatText()
	{
		//CombatManager.StartCombat();

		CombatStartDialogueCallback();

		StopAllCoroutines();

		WipeAll();

		EVENT_text.gameObject.SetActive(true);
		StartCoroutine(TypeSentence("What do you want to do?"));

		PLAYER_text[0].transform.parent.gameObject.SetActive(true);
		PLAYER_text[0].text = "Basic Attack.";

		//SetCombatListener();
	}

	/*private void SetCombatListener()
	{
		//PLAYER_text[0].transform.parent.gameObject.GetComponent<Button>().onClick.AddListener(CombatManager.AttackEnemyWrapper);
	}

	private void RemoveCombatListener()
	{
		//PLAYER_text[0].transform.parent.gameObject.GetComponent<Button>().onClick.RemoveListener(CombatManager.AttackEnemyWrapper);
	}*/

	public void UpdateCombatText(string name, int damage)
	{
		//StopAllCoroutines();
		WipeAll();

		EVENT_text.gameObject.SetActive(true);
		StartCoroutine(TypeSentence(name + " did " + damage + " damage!"));
	}

	public void AdvanceEventText()
	{
		VD.Next();
	}

	IEnumerator TypeSentence(string sentence)
	{
		EVENT_text.text = string.Empty;

		for (int i = 0; i < sentence.RichTextLength(); i++)
		{
			yield return null;
			EVENT_text.text = sentence.RichTextSubString(i + 1);
		}
	}
}
