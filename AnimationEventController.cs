using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
	public UnityEngine.Events.UnityEvent[] events;
	public void OnAnimationEvent(AnimationEvent e)
	{
		events[e.intParameter].Invoke();
	}
	public void ShakeScreen(float val)
	{
		CamFollow.Instance.Shake(val);
	}
}
