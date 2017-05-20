using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpaceRace
{

    public class ApplicationSettingsManager : MonoBehaviour
    {
        /// <summary>
        /// Quits the application on all platforms.
        /// </summary>
        public void ExitApplication()
        {
            if (Application.isPlaying)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        /// <summary>
        /// Sets the graphics teir of the application, do not adjust if you are unsure.
        /// </summary>
        /// <param name="tier">The teir to be set</param>
        public void SetGraphicsTier(UnityEngine.Rendering.GraphicsTier tier)
        {
            UnityEngine.Graphics.activeTier = tier;
        }
        /// <summary>
        /// Gets the active tier of the application graphic settings.
        /// </summary>
        /// <returns></returns>
        public UnityEngine.Rendering.GraphicsTier GetGraphicsTier()
        {
            return UnityEngine.Graphics.activeTier;
        }
        /// <summary>
        /// Sets the graphics quality level of the application.
        /// </summary>
        /// <param name="level">level of graphics</param>
        public void SetQualityLevel(string level)
        {
            string[] levels = QualitySettings.names;
            if (levels.Contains(level))
            {
                int i = 0;
                int l;
                foreach (string _level in levels)
                {
                    if (_level == level)
                    {
                        i = l;
                    }
                    l++;
                }
                QualitySettings.SetQualityLevel(i, false);
            }
        }
        /// <summary>
        /// Sets the graphics quality level of the application.
        /// </summary>
        /// <param name="level">level of graphics</param>
        /// <param name="doExpensiveChanges">Should the expensive graphics changes be applied?</param>
        public void SetQualityLevel(string level, bool doExpensiveChanges)
        {
            string[] levels = QualitySettings.names;
            if (levels.Contains(level))
            {
                int i = 0;
                int l;
                foreach (string _level in levels)
                {
                    if (_level == level)
                    {
                        i = l;
                    }
                    l++;
                }
                QualitySettings.SetQualityLevel(i, doExpensiveChanges);
            }
        }
        /// <summary>
        /// Sets the graphics quality level of the application.
        /// </summary>
        /// <param name="level">level of graphics</param>
        public void SetQualityLevel(int level)
        {
            if (level < QualitySettings.names.Length)
            {
                QualitySettings.SetQualityLevel(level, false);
            }
        }
        /// <summary>
        /// Sets the graphics quality level of the application.
        /// </summary>
        /// <param name="level">level of graphics</param>
        /// <param name="doExpensiveChanges">Should the expensive graphics changes be applied?</param>
        public void SetQualityLevel(int level, bool doExpensiveChanges)
        {
            if (level < QualitySettings.names.Length)
            {
                QualitySettings.SetQualityLevel(level, doExpensiveChanges);
            }
        }
        /// <summary>
        /// Gets the current level of graphic quality.
        /// </summary>
        /// <returns></returns>
        public int GetQualityLevel()
        {
            return QualitySettings.GetQualityLevel();
        }
        /// <summary>
        /// Sets the target frame rate of the application, if possible.
        /// </summary>
        /// <param name="framerate">Targwet framerate</param>
        public void SetFramerate(int framerate)
        {
            Application.targetFrameRate = framerate;
        }
        /// <summary>
        /// Sets the audio quality of the application, but default it is 2-Speaker stereo.
        /// </summary>
        /// <param name="mode">Speaker drivers mode to be set</param>
        public void SetAudioCapability(AudioSpeakerMode mode)
        {
            AudioSettings.speakerMode = mode;
        }
        /// <summary>
        /// Gets thw current speaker mode supported by the drivers.
        /// </summary>
        /// <returns></returns>
        public AudioSpeakerMode GetAudioCapability()
        {
            return AudioSettings.driverCapabilities;
        }
        /// <summary>
        /// Set the screen orientation to only allow landscape rotations.
        /// </summary>
        public void SetOrientationLandscapeOnly()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.WSAPlayerARM)
            {
                Screen.orientation = ScreenOrientation.Landscape;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
            }
        }
        /// <summary>
        /// Set the screen orientation to only allow portrait rotations.
        /// </summary>
        public void SetOrientationPortraitOnly()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.WSAPlayerARM)
            {
                Screen.orientation = ScreenOrientation.Portrait;
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
            }
        }

    }
}
