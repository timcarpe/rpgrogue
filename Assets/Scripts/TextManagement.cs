using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManagement : MonoBehaviour
{
	//public GameObject events_;
	private EventsManager EventsManager;
	//private ButtonHandler ButtonHandler;

	public Animator EventTextAnimator;
	public Text eventName;
	public Text eventType;
	public Text cameraName;
	public Text cameraPosition;
	public Text eventText;
	public Text fps;
	//public Text option1_;
	//public Text option2_;
	//public Text option3_;
	//public Text option4_;
	//public Button button1_;
	//public Button button2_;
	//public Button button3_;
	//public Button button4_;
	//public int optionChoice;
	//private bool setUp = false;
	public void Start()
	{
		EventsManager = EventsManager.Instance;
		//ButtonHandler = ButtonHandler.Instance;
	}
	public void Update()
	{
		//Update the UI Debugger with new information
		Debugger();
		//SetupUI();
	}
	//Adds information to the debugger menu
	private void Debugger()
	{
		eventName.text = "Event Name:" + EventsManager.GetEventName();
		eventType.text = "Event Type: " + EventsManager.GetEventType();
		cameraName.text = "Camera Name: " + EventsManager.GetCameraName();
		cameraPosition.text = "Camera Position: " + EventsManager.GetCameraPosition();
		fps.text = "FPS: " + (1.0f / Time.smoothDeltaTime).ToString("F0");
	}
	//Sets the event text in the UI to the next sentence
	public void SetNextText()
	{
		/*//eventText.text = EventsManager.GetText();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(EventsManager.GetText()));*/
	}
	public void SetNextResult(int options = 0)
	{
		/*//eventText.text = EventsManager.GetResult(options);
		StopAllCoroutines();
		StartCoroutine(TypeSentence(EventsManager.GetResult(options)));*/
	}
	public void SetTextandOptions()
	{
		/*SetNextText();
		SetOptions();*/
	}
	public void SetResultandOptions()
	{
		/*SetNextResult();
		SetOptions();*/
	}
	IEnumerator TypeSentence (string sentence)
	{
		/*//yield return new WaitForSeconds(1);
		eventText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			eventText.text += letter;
			yield return null;
		}*/
		yield return null;
	}
	public void OpenEventBox()
	{
		EventTextAnimator.SetBool("IsOpen", true);
	}
	public void CloseEventBox()
	{
		EventTextAnimator.SetBool("IsOpen", false);
	}
	//Sets options for the event interface
	public void SetOptions()
	{
		/*button1_.onClick.RemoveAllListeners();
		//If text has not ended add continue button that continue text
		if (!EventsManager.IsTextEnd())
		{
			option1_.text = "Continue.";
			option2_.enabled = false;
			option3_.enabled = false;
			option4_.enabled = false;
			button1_.onClick.AddListener(ButtonScript.ContinueText);
		}
		//If text has ended and there is an option choice from player
		else if (EventsManager.IsTextEnd() && optionChoice > 0)
		{
			SetNextResult(optionChoice - 1);
			//Debug.Log(EventsManager.GetResult());
			option1_.text = "Move on.";
			option2_.enabled = false;
			option3_.enabled = false;
			option4_.enabled = false;
			button1_.onClick.AddListener(ButtonScript.LoadNextScene);
		}
		//If text has ended add the decision options for each event type
		else
		{
			string optionsSwitch = EventsManager.GetEventType();
			switch (optionsSwitch)
			{
				case "TREASURE":
					{
						option1_.enabled = true;
						option2_.enabled = true;
						option1_.text = "Pick it up.";
						option2_.text = "Leave.";
						button1_.onClick.AddListener(ButtonScript.ChooseOption1);
						button2_.onClick.AddListener(ButtonScript.ChooseOption2);
						break;
					}
				case "CHEST":
					{
						option1_.enabled = true;
						option2_.enabled = true;
						option1_.text = "Open it.";
						option2_.text = "Leave.";
						button1_.onClick.AddListener(ButtonScript.ChooseOption1);
						button2_.onClick.AddListener(ButtonScript.ChooseOption2);
						break;
					}
				default: break;
			}
		}*/
	}
	//Sets up UI for first time
	/*private void SetupUI()
	{
		if(!setUp)
		{
			SetTextandOptions();
			setUp = true;
		}
	}*/
}

