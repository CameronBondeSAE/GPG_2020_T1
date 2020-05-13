using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


[Serializable]
public class TutorialEvent
{
	public string textToShow;
	public string messageToSatisfyAndRemoveText;
}

public class TuteTest : SerializedMonoBehaviour
{
	// eventMessage to tutorial text
	public List<TutorialEvent> tutorialEvents;

	public GameObject tutorialUI;

	[Header("HACKS for hooking up non-abstracted events")]
	// HACK: Systems to subscribe to and turn into a generic message
	public UnitSelectionManager unitSelectionManager;

	public int currentTutorialMessageIndex = -1; // HACK: to start at zero with showing 'nextTutorialMessage'

	public TutorialEvent currentTutorial;

	/// <summary>
	/// HACK: Take non-abstracted events and turn them into general messages
	/// </summary>
	void Start()
	{
		UnitBase.SpawnStaticEvent                        += obj => EventMessageTriggered("OnSpawnUnit");
		unitSelectionManager.OnSelectionEvent            += list => EventMessageTriggered("OnSelection");
		AbilityController.ClickedLocalAbilityStaticEvent += controller => EventMessageTriggered("OnClickedAbility");
		UIManager.TargetActionStaticEvent                += context => EventMessageTriggered("OnTargetAction");

		NextTutorialMessage();
	}

	private void EventMessageTriggered(string eventMessage)
	{
		if (tutorialEvents[currentTutorialMessageIndex].messageToSatisfyAndRemoveText == eventMessage)
		{
			TurnOffTutorialMessage();
			NextTutorialMessage();
		}
	}

	private void NextTutorialMessage()
	{
		StartCoroutine(NextTutorialMessageCoRoutine());
	}

	private IEnumerator NextTutorialMessageCoRoutine()
	{
		yield return new WaitForSeconds(1);
		currentTutorialMessageIndex++;
		if (currentTutorialMessageIndex <= tutorialEvents.Count - 1)
		{
			currentTutorial = tutorialEvents[currentTutorialMessageIndex];
			TurnOnTutorialMessage(tutorialEvents[currentTutorialMessageIndex].textToShow);
		}
	}


	private void TurnOnTutorialMessage(string text)
	{
		Debug.Log("Tutorial text = " + text);
		tutorialUI.SetActive(true);
		tutorialUI.GetComponentInChildren<TextMeshProUGUI>().text = text;
	}

	public void TurnOffTutorialMessage()
	{
		Debug.Log("MESSAGE OFF");
		tutorialUI.SetActive(false);
	}
}