using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
[DisallowMultipleComponent] //Prevent people from adding 2 of this script to a single object.
public class colorFader : MonoBehaviour {

	public enum colorType { Unselected, Image, Sprite, RawImage, Text, Particle} //Enum for determining source object.
    public colorType ColorType = colorType.Unselected; //Enum type to help code determine the proper object to derive color from.
    public Color FadeToColor; //The color we will be fading towards.
    public object objectToColor; //The object we will be coloring.
    public Color objectsColor; //The Changing color of the object.
    public Color originalColor; //The Original color of the object.
    public float fadeTime; //Amount of time to take fading.
    public float startTime; //By default is 0, but if looped, will be reset to zero once fadeTime is reached.
    public bool destroyOnFade; //Will the object be destroyed after fadeTime is reached?
    public bool willLoop; //Will the object Loop? it can't do both.
	void Start () {
        // We must determine what type of script that we are effecting the color of.
        #region Image
        if (ColorType == colorType.Image) //Was the Selected Enum an Image?
        {
            objectToColor = gameObject.GetComponent<Image>(); //Getting the source, assuming it is an image.
            if (objectToColor != null) //GetComponent does not throw an error, even if it fails to get your variable.
            {
                Image oTC = (Image)objectToColor; //Declaring we actually have an image.
                objectsColor = oTC.color; //Finally we can get a color from it
                originalColor = oTC.color; //We need to store the original color for looping or resetting purposes.
            }
        }
        #endregion
        #region Sprite
        else if (ColorType == colorType.Sprite) //Was the Selected Enum an Sprite?
        {
            objectToColor = gameObject.GetComponent<SpriteRenderer>(); //Getting the source, assuming it is an sprite.
            if (objectToColor != null) //GetComponent does not throw an error, even if it fails to get your variable.
            {
                SpriteRenderer oTC = (SpriteRenderer)objectToColor; //Declaring we actually have a Sprite.
                objectsColor = oTC.color; //Finally we can get a color from it
                originalColor = oTC.color; //We need to store the original color for looping or resetting purposes.
            }
        }
        #endregion
        #region RawImage
        else if (ColorType == colorType.RawImage) //Was the Selected Enum a RawImage?
        {
            objectToColor = gameObject.GetComponent<RawImage>(); //Getting the source, assuming it is a RawImage.
            if (objectToColor != null) //GetComponent does not throw an error, even if it fails to get your variable.
            {
                RawImage oTC = (RawImage)objectToColor; //Declaring we actually have a RawImage.
                objectsColor = oTC.color; //Finally we can get a color from it
                originalColor = oTC.color; //We need to store the original color for looping or resetting purposes.
            }
        }
        #endregion
        #region Text
        else if (ColorType == colorType.Text) //Did user select the Text Enum?
        {
            objectToColor = gameObject.GetComponent<Text>(); //Getting the source, assuming it is an Text.
            if (objectToColor != null) //GetComponent does not throw an error, even if it fails to get your variable.
            {
                Text oTC = (Text)objectToColor; //Declaring we actually have a RawImage.
                objectsColor = oTC.color; //Finally we can get a color from it
                originalColor = oTC.color; //We need to store the original color for looping or resetting purposes.
            }
        }
        #endregion
        startTime = 0; //Setting the start time to 0, in C# this is a redundancy.
    }
	void Update () {
        // We must determine what type of object the source object is, yet again
        #region Image
        if (ColorType == colorType.Image)
        {
            if (objectToColor != null)
            {
                startTime += Time.deltaTime; //Add the time it took from last Update to our overall Time.
                Image oTC = (Image)objectToColor; //Declare the object is what it is again.
                objectsColor = Color.Lerp(objectsColor, FadeToColor, (Mathf.InverseLerp(0, fadeTime, startTime / 10))); //Blend the two colors based on a scaled Inverse lerp of the time/max time.
                oTC.color = objectsColor; //Set the color of our object to our new color.
            }
        }
        #endregion
        #region RawImage
        else if (ColorType == colorType.RawImage)
        {
            if (objectToColor != null)
            {
                startTime += Time.deltaTime; //Add the time it took from last Update to our overall Time.
                RawImage oTC = (RawImage)objectToColor; //Declare the object is what it is again.
                objectsColor = Color.Lerp(objectsColor, FadeToColor, (Mathf.InverseLerp(0, fadeTime, startTime / 10))); //Blend the two colors based on a scaled Inverse lerp of the time/max time.
                oTC.color = objectsColor; //Set the color of our object to our new color.
            }
        }
        #endregion
        #region Sprite
        else if (ColorType == colorType.Sprite)
        {
            if (objectToColor != null)
            {
                startTime += Time.deltaTime; //Add the time it took from last Update to our overall Time.
                SpriteRenderer oTC = (SpriteRenderer)objectToColor; //Declare the object is what it is again.
                objectsColor = Color.Lerp(objectsColor, FadeToColor, (Mathf.InverseLerp(0, fadeTime, startTime / 10))); //Blend the two colors based on a scaled Inverse lerp of the time/max time.
                oTC.color = objectsColor; //Set the color of our object to our new color.
            }
        }
        #endregion
        #region Text
        else if (ColorType == colorType.Text)
        {
            if (objectToColor != null)
            {
                startTime += Time.deltaTime; //Add the time it took from last Update to our overall Time.
                Text oTC = (Text)objectToColor; //Declare the object is what it is again.
                objectsColor = Color.Lerp(objectsColor, FadeToColor, (Mathf.InverseLerp(0, fadeTime, startTime / 10))); //Blend the two colors based on a scaled Inverse lerp of the time/max time.
                oTC.color = objectsColor; //Set the color of our object to our new color.
            }
        }
        #endregion
        #region Bools
        if (startTime > fadeTime && destroyOnFade) //Check if the overall time is greater than our fade time, and if the object should be destroyed.
        {
            Destroy(gameObject); //Destroy the GameObject this script is attached to.
        }
        else if (startTime > fadeTime && willLoop) //Check if the overall time is greater than our fade time, and if the object should continue to loop.
        {
            Color tempColor = FadeToColor; //Create a temporary color so we don't overwrite our original color.
            FadeToColor = originalColor; //Set our destination color to the original color, because we reached our destination.
            originalColor = tempColor; //Set our store the old destination color for looping back.
            startTime = 0; //Reset out time to 0 so the loop can begin again.
        }
        else if (startTime > fadeTime) //Check if the overall time is greater than our fade time, and if so, disable it and save the color.
        {
            FadeToColor = originalColor; //Store the destination color so that when the object is re-enabled, it will fade back.
            gameObject.SetActive(false); //Disable the GameObject this script is attached to.
        }
        #endregion
    }
}