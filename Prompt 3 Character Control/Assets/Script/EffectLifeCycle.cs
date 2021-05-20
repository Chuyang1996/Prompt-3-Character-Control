using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EffectLifeCycle : MonoBehaviour
{
	public bool IfStillAvaliable;

	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			if (!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if (IfStillAvaliable)
				{
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
					this.gameObject.SetActive(false);
					#endif
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
}
