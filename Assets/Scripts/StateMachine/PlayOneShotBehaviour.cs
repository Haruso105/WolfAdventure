using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;   //再生する効果音
    public float volume = 1f;   //音量
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;
    //playOnEnterにチェックが付いていたらアニメーションが始まったときに、Exitの方についていたら終わったときに流れる
    //playAfterDelayにチェックが付いてたら指定した時間が経過してから再生される。

    // 遅延用サウンドタイマー
    public float playDelay = 0.25f; //流れるまでの時間
    private float timeSinceEntered = 0; //このスクリプトが呼び出されてからの経過時間
    private bool hasDelayedSoundPlayed = false; //サウンドが再生されたかチェック

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
            //PlayClipAtPoint(再生データ, 音の発生する位置, 音量) これは自動的にAudioSourceを作成して指定した位置で再生する。
        }

        timeSinceEntered = 0f;
        hasDelayedSoundPlayed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playAfterDelay && !hasDelayedSoundPlayed)    
        {
            timeSinceEntered += Time.deltaTime;
            if(timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                hasDelayedSoundPlayed = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }
    }
}
