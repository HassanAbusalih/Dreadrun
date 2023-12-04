using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    public Image baseimage;
    public Sprite[] sprites;
    public float speed = .02f;
    
    private int spriteindex;
    private Coroutine animationCouroutine;
  
    bool IsDone;

    private void Update()
    {
        PlayUIAnim();
    }


    public void PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(PlayUIAnimation());
    }

    // Public method to stop the animation
    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(PlayUIAnimation());
    }

 
    IEnumerator PlayUIAnimation()
    {
        // Wait for the specified amount of time before continuing
        yield return new WaitForSeconds(speed);

        // If the current sprite index is greater than or equal to the number of sprites in the array,
        // reset the index to 0 to loop back to the beginning of the array
        if (spriteindex >= sprites.Length)
        {
            spriteindex = 0;
        }

        // Update the image sprite to the current sprite in the array
        baseimage.sprite = sprites[spriteindex];

        // Increment the sprite index for the next iteration
        spriteindex += 1;

        // If the animation is not done, start the coroutine again to play the next iteration
        if (IsDone == false)
            animationCouroutine = StartCoroutine(PlayUIAnimation());
    }
}

