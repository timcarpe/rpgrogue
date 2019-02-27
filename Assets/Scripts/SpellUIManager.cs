using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellUIManager : MonoBehaviour
{
	public GameObject[] m_spellOverhead;
	public Button[] m_ButtonArray;
	private int m_CurrentSpellIndex = 3;

	private void Start()
	{

	}
	public void SetSpellImage(Sprite sprite)
	{
		if (m_CurrentSpellIndex < 0)
			return;

		m_spellOverhead[m_CurrentSpellIndex].SetActive(true);
		m_spellOverhead[m_CurrentSpellIndex].GetComponent<Image>().sprite = sprite;
		m_CurrentSpellIndex--;
	}

	public void ResetLastSpellImage()
	{
		if (m_CurrentSpellIndex > 3)
			return;

		m_spellOverhead[m_CurrentSpellIndex].GetComponent<Image>().sprite = null;
		m_spellOverhead[m_CurrentSpellIndex].gameObject.SetActive(false);
		m_CurrentSpellIndex++;
	}

	public void ResetAllSpellImages()
	{
		foreach (GameObject image in m_spellOverhead)
		{
			image.GetComponent<Image>().sprite = null;
			image.gameObject.SetActive(false);
			m_CurrentSpellIndex = 3;
		}
	}

	public void SetButtonsActive()
	{
		foreach(Button button in m_ButtonArray)
		{
			button.interactable = true;
		}
	}

	public void SetButtonsInActive()
	{
		foreach (Button button in m_ButtonArray)
		{
			button.interactable = false;
		}
	}
}
