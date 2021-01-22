using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
	public float waitTime = 0.5f;
	private void OnEnable()
	{
		StartCoroutine(DisableSelf());
	}

	private IEnumerator DisableSelf()
	{
		yield return new WaitForSeconds(waitTime);
		this.gameObject.SetActive(false);
	}
}