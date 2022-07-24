using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventController : MonoBehaviour, IStateObject
{
    [System.Serializable]
    public class TimedEvent
    {
        public UnityEngine.Events.UnityEvent m_event;
        public float time;
    }
    public List<TimedEvent> timedEvents;
    private bool hasTriggered = false;

	private void Update()
	{
        if (hasTriggered)
            return;
        foreach (TimedEvent tEvent in timedEvents)
        {
            tEvent.time -= Time.deltaTime;
        }
        foreach (TimedEvent tEvent in timedEvents)
        {
            if (tEvent.time <= 0)
            {
                tEvent.m_event.Invoke();
                timedEvents.Remove(tEvent);
                return;
            }
        }
        if (timedEvents.Count == 0)
            hasTriggered = true;
    }




}
