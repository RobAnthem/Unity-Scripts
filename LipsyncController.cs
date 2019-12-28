// This code is licensed under CC0 license, feel free to alter a redistribute this code.
// Written by Robin Trantham with help from UnityAnswers
// Lip Sync Controller, intended to automatically sync the jaw movement of a rigged humanoid.
using UnityEngine;

public class LipsyncController : MonoBehaviour
{
	/// <summary>
	/// Type of X, Y, or Z axis
	/// </summary>
	public enum Axis
	{
		X, Y, Z
	}
	/// <summary>
	/// Type of analysis to retrieve spectrum data.
	/// </summary>
	public FFTWindow analysisType = FFTWindow.Rectangular;
	/// <summary>
	/// Movement axis for up/down movement of jaw
	/// </summary>
	public Axis mouthAxis = Axis.X;
	/// <summary>
	/// Rotation axis for jaw movement;
	/// </summary>
	public Axis mouthRotAxis = Axis.Z;
	/// <summary>
	/// Jaw Object to lipsync
	/// </summary>
	public GameObject mouth;
	/// <summary>
	/// Weight of jaw movement by audio spectrum
	/// </summary>
	public float volume = 0.25f;
	/// <summary>
	/// Low audio frequency
	/// </summary>
	public float frqLow = 200;
	/// <summary>
	/// High audio frequency
	/// </summary>
	public float frqHigh = 800;
	/// <summary>
	/// Weight of rotation on jaw bone
	/// </summary>
	public float rotVolume = 0.25f;
	/// <summary>
	/// Ignore automatic listener distance and calculate volume to distance
	/// </summary>
	public bool ignoreDistance;
	/// <summary>
	/// only used if Ignore Distance is checked
	/// </summary>
	public float minDistance = 5.0f, maxDistance = 30.0f;
	/// <summary>
	/// Only used if Ignore Distance is checked
	/// </summary>
	public float minVol = .25f, maxVol = 10.0f;
	#region Private Fields
	private float[] freqData;
	private float nSamples = 256;
	private float fMax = 24000;
	private AudioSource audio;
	private const float fFloor = 20;
	private float prevSum;
	private float y0;
	private float y1;
	private int sizeFilter = 5;
	private float[] filter;
	private float filterSum;
	private int posFilter = 0;
	private int qSamples = 0;
	private bool resetState;
	private float[] aSamples;
	private const int aSamplesLength = 4096;
	#endregion
	void Start()
	{
		audio = GetComponent<AudioSource>(); // get AudioSource component
		if (mouthAxis == Axis.Y)
			y0 = mouth.transform.localPosition.y;
		else if (mouthAxis == Axis.X)
			y0 = mouth.transform.localPosition.x;
		else
			y0 = mouth.transform.localPosition.z;
		if (mouthRotAxis == Axis.Y)
			y1 = mouth.transform.localRotation.y;
		else if (mouthRotAxis == Axis.X)
			y1 = mouth.transform.localRotation.x;
		else
			y1 = mouth.transform.localRotation.z;
		freqData = new float[(int)nSamples];
		audio.Play();
		//aSamples = new float[aSamplesLength];
	}
	void Update()
	{
		if (!audio.isPlaying)
		{
			if (!resetState)
			{
				ResetMouth();
				resetState = true;
			}
			return;
		}
		else
		{
			resetState = false;
		}
		if (ignoreDistance)
		{
			volume = Mathf.Lerp(minVol, maxVol, Mathf.InverseLerp(minDistance, maxDistance,
				Vector3.Distance(audio.transform.position, Camera.main.transform.position)));
		}
		Vector3 pos = mouth.transform.localPosition;
		Quaternion rot = mouth.transform.localRotation;
		if (mouthAxis == Axis.Y)
		{
			pos.y = y0 + MovingAverage(BandVol(frqLow, frqHigh)) * volume;
		}
		else if (mouthAxis == Axis.X)
		{
			pos.x = y0 + MovingAverage(BandVol(frqLow, frqHigh)) * volume;
		}
		else
		{
			pos.z = y0 + MovingAverage(BandVol(frqLow, frqHigh)) * volume;
		}
		if (mouthRotAxis == Axis.Y)
		{
			rot.y = y1 + BandVol(frqLow, frqHigh) * rotVolume;
		}
		else if (mouthRotAxis == Axis.X)
		{
			rot.x = y1 + BandVol(frqLow, frqHigh) * rotVolume;
		}
		else
		{
			rot.z = y1 + BandVol(frqLow, frqHigh) * rotVolume;
		}
		mouth.transform.localPosition = pos;
		mouth.transform.localRotation = rot;
	}
	void ResetMouth()
	{
		Vector3 pos = mouth.transform.localPosition;
		Quaternion rot = mouth.transform.localRotation;
		if (mouthAxis == Axis.Y)
		{
			pos.y = y0;
		}
		else if (mouthAxis == Axis.X)
		{
			pos.x = y0;
		}
		else
		{
			pos.z = y0;
		}
		if (mouthRotAxis == Axis.Y)
		{
			rot.y = y1;
		}
		else if (mouthRotAxis == Axis.X)
		{
			rot.x = y1;
		}
		else
		{
			rot.z = y1;
		}
		mouth.transform.localPosition = pos;
		mouth.transform.localRotation = rot;
	}
	public void PlaySound(AudioClip clip)
	{
		audio.clip = clip;
		audio.Play();
	}
	/// <summary>
	/// Calculate the average spectrum analysis along the audio clip for smoother movement
	/// </summary>
	/// <param name="sample"></param>
	/// <returns></returns>
	float MovingAverage(float sample)
	{

		if (qSamples == 0) filter = new float[sizeFilter];
		filterSum += sample - filter[posFilter];
		filter[posFilter++] = sample;
		if (posFilter > qSamples) qSamples = posFilter;
		posFilter = posFilter % sizeFilter;
		return filterSum / qSamples;
	}
	/// <summary>
	/// Set mouth to base closed state
	/// </summary>
	float BandVol(float fLow, float fHigh)
	{
		fLow = Mathf.Clamp(fLow, fFloor, fMax);
		fHigh = Mathf.Clamp(fHigh, fLow, fMax);
		audio.GetSpectrumData(freqData, 1, analysisType);
		int n1 = (int)Mathf.Floor(fLow * nSamples / fMax);
		int n2 = (int)Mathf.Floor(fHigh * nSamples / fMax);
		float sum = 0;
		for (int i = n1; i < freqData.Length; i++)
		{
			sum += freqData[i];
		}
		return sum / (n2 - n1 + 1);
	}
	/// <summary>
	/// **TEST NOT IMPLEMENTED**
	/// Calculate audio volume from source for distance calculations
	/// </summary>
	/// <param name="channel"></param>
	/// <returns></returns>
	float GetRMS(int channel)
	{
		audio.GetOutputData(aSamples, channel);
		float sum = 0;
		foreach (float f in aSamples)
		{
			sum += f * f;
		}
		return Mathf.Sqrt(sum / aSamplesLength);
	}
}
