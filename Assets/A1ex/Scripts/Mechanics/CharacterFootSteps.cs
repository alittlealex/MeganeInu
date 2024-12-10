using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFootSteps : MonoBehaviour
{
    public AudioSource footStepAudio;
    public FirstPersonCharacter character;


    private void Update()
    {
        if (character.GetSpeed() > 0)
        {
            if(!footStepAudio.isPlaying)
                footStepAudio.Play();
        }
        else
        {
            if(footStepAudio.isPlaying)
                footStepAudio.Pause();
        }

        if(character.IsSprinting())
            footStepAudio.pitch = 2f * (character.GetSpeed() / character.GetMaxSpeed());
        else
            footStepAudio.pitch = 1.35f * (character.GetSpeed() / character.GetMaxSpeed());

        if(!character.IsOnGround())
        {
            footStepAudio.pitch = 0;
        }
    }
}
