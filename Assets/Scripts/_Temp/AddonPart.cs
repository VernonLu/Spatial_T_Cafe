using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddonPart : MonoBehaviour
{
	public bool isFinished = false;

	public float waitTime = 0.5f;
	private void OnEnable()
	{
		StartCoroutine(Finished());
	}
	void Start() { }

	IEnumerator Finished()
	{
		yield return new WaitForSeconds(waitTime);
		isFinished = true;
		TabListManager.Instance.UpdateTabList();
	}

}