using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovementController : MonoBehaviour
{
    public FirstPersonCharacter character;
    public Animator animator;

    float eyeMovementVal = 0f;

    private void Update()
    {
        eyeMovementVal = Mathf.Clamp01(character.GetSpeed() / (character.maxWalkSpeed * character.sprintSpeedModifier));
        animator.SetFloat("MoveSpeed", eyeMovementVal);
    }
}
