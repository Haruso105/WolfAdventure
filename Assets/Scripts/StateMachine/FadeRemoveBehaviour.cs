using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;

    //OnStateEnterはこのスクリプトが付いたAnimationStateが動き始めたら呼び出される。
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;
    }

    //OnStateUpdate このスクリプトが付いたStateのアニメーションが流れている間、読み込まれる。
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;

        float newAlpha = startColor.a * (1 - timeElapsed / fadeTime);   //時間がたつにつれ薄くなっていく

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        if(timeElapsed > fadeTime)
        {
            Destroy(objToRemove);
        }
    }
}
