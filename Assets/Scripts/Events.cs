using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EVENT", menuName = "Event")]
public class Events : ScriptableObject
{
	public string eventName;
	public string type;
	public string eventType;
	public string cleanName;
	[Range(0,5)]public int tier;
	public bool repeatable = true;
	public bool canDo = true;
	//public Vector3 cameraLoc;
	public int[] dialogueIndex;

}
