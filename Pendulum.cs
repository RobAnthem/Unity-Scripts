using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Cubening
{
    /// <summary>
    /// A float tool for ping-ponging between two floats.
    /// </summary>
    public class Pendulum
    {
        //Lowest float number the pendulum can reach.
        private float min;
        //Highest float number the pendulum can reach.
        private float max;
        //How much time before the pendulum reachs its goal, and changes direction.
        private float Scale;
        //The current scale of the pendulum, comparable to the total scale.
        private float timeScale;
        //The current output number of the pendulum.
        private float current;
        //Current durection of the pendulum.
        private bool direction;
        public delegate void AnimationComplete();
        public event AnimationComplete OnComplete;
        /// <summary>
        /// Constructs a pendulum object to pingpong between 
        /// minimum and maximum when the scale reaches the switch time.
        /// </summary>
        /// <param name="minimum">Minimum output float.</param>
        /// <param name="maximum">Maximum output float.</param>
        /// <param name="switchTime">Amount of time before switching direction.</param>
        public Pendulum(float minimum, float maximum, float switchTime)
        {
            min = minimum;
            max = maximum;
            current = 0;
            Scale = switchTime;
            direction = true;
        }
        /// <summary>
        /// Constructs a pendulum object to pingpong between 
        /// minimum and maximum when the scale reaches the switch time.
        /// </summary>
        /// <param name="value">The default starting value of the return float</param>
        /// <param name="minimum">Minimum output float.</param>
        /// <param name="maximum">Maximum output float.</param>
        /// <param name="switchTime">Amount of time before switching direction.</param>
        public Pendulum(float value, float minimum, float maximum, float switchTime)
        {
            min = minimum;
            max = maximum;
            current = value;
            Scale = switchTime;
            direction = true;
        }
        /// <summary>
        /// Constructs a pendulum object to pingpong between 
        /// minimum and maximum when the scale reaches the switch time.
        /// </summary>
        /// <param name="value">The default starting value of the return float</param>
        /// <param name="minimum">Minimum output float.</param>
        /// <param name="maximum">Maximum output float.</param>
        /// <param name="switchTime">Amount of time before switching direction.</param>
        /// <param name="startDirection">Default starting direction, false is down, true is up</param>
        public Pendulum(float value, float minimum, float maximum, float switchTime, bool startDirection)
        {
            min = minimum;
            max = maximum;
            current = value;
            Scale = switchTime;
            direction = startDirection;
        }
        /// <summary>
        /// Sets the direction of the pendulum, false is down, true is up.
        /// </summary>
        /// <param name="dir">Direction of the pendulum</param>
        public void SetDirection(bool dir)
        {
            direction = dir;
        }
        /// <summary>
        /// Reset the pendulum to default values.
        /// </summary>
        public void Reset()
        {
            current = 0;
            timeScale = 0;
        }
        /// <summary>
        /// Reset the pendulum to default values, with specific direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Reset(bool dir)
        {
            current = 0;
            timeScale = 0;
            direction = dir;
        }
        /// <summary>
        /// Get the next return float of the pendulum.
        /// </summary>
        /// <param name="time">The time since the last pendulum call</param>
        /// <returns></returns>
        public float MoveNext(float time)
        {
            if (direction)
            {
                timeScale += time/Scale;
                if (timeScale > 1.0f)
                {
                    direction = false;
                }
            }
            else
            {
                timeScale -= time/Scale;
                if (timeScale < 0)
                {
                    direction = true;
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }
            }
            current = Mathf.Lerp(min, max, timeScale);
            return current;
        }
        /// <summary>
        /// Returns the current float state of the pendulum.
        /// </summary>
        /// <returns></returns>
        public float GetCurrent()
        {
            return current;
        }
        public void SetMin(float f)
        {
            min = f;
        }
        public void SetMax(float f)
        {
            max = f;
        }
    }
}
