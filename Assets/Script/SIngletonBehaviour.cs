using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIngletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (SIngletonBehaviour<T>._instance == null)
			{
				SIngletonBehaviour<T>._instance = (Object.FindObjectOfType(typeof(T)) as T);
			}
			return SIngletonBehaviour<T>._instance;
		}
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x0018F8B4 File Offset: 0x0018DAB4
	protected virtual void OnApplicationQuit()
	{
        _isQuit = true;
		base.StopAllCoroutines();
	}

	// Token: 0x04003403 RID: 13315
	private static T _instance;

	// Token: 0x04003404 RID: 13316
	private static bool _isQuit;
}
