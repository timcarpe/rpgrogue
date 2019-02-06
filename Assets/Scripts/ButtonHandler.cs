using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonHandler : MonoBehaviour
{
	public TextManagement textManagement_;
	public EventsManager eventsScript_;
	/*
	//Updates the event text with the next line of text and updates the options UI
	public void ContinueText()
	{
		textManagement_.SetNextText();
		textManagement_.SetOptions();
	}
	//Updates the event text with the result and updates the options UI
	public void ContinueResult()
	{
		textManagement_.SetNextResult();
		textManagement_.SetOptions();
	}
	//Loads the next scene but currently just restarts the scene
	public void LoadNextScene()
	{
		eventsScript_.EndEvent();
		StartCoroutine(eventsScript_.SceneStart());
	}*/
	//Sets the variable in Text Management of the option button choice
	/*private void SetOptionsChoice(int option)
	{
		textManagement_.optionChoice = option;
	}*/
	/*
	public void ChooseOption(int option)
	{
		SetOptionsChoice(option);
		textManagement_.SetOptions();
	}
	//Individual functions for each option. These are individual so they can be added as listeners to buttons
	public void ChooseOption1()
	{
		SetOptionsChoice(1);
		textManagement_.SetOptions();
	}
	public void ChooseOption2()
	{
		SetOptionsChoice(2);
		textManagement_.SetOptions();
	}
	public void ChooseOption3()
	{
		SetOptionsChoice(3);
		textManagement_.SetOptions();
	}
	public void ChooseOption4()
	{
		SetOptionsChoice(4);
		textManagement_.SetOptions();
	}
	*/
}
