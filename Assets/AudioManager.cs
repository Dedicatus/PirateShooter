using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [FMODUnity.EventRef] [SerializeField] string gunshotEvent = "event:/Gunshot";
    [FMODUnity.EventRef] [SerializeField] string exploseEvent = "event:/Explose";
    [FMODUnity.EventRef] [SerializeField] string jumpEvent = "event:/Jump";
    [FMODUnity.EventRef] [SerializeField] string jumpOnEnemyEvent = "event:/JumpOnEnemy";

    FMOD.Studio.EventInstance gunshot;
    FMOD.Studio.EventInstance explose;
    FMOD.Studio.EventInstance jump;
    FMOD.Studio.EventInstance jumpOnEnemy;
    void Awake()
    {
        gunshot = FMODUnity.RuntimeManager.CreateInstance(gunshotEvent);
        explose = FMODUnity.RuntimeManager.CreateInstance(exploseEvent);
        jump = FMODUnity.RuntimeManager.CreateInstance(jumpEvent);
        jumpOnEnemy = FMODUnity.RuntimeManager.CreateInstance(jumpOnEnemyEvent);
    }

    public void PlayGunshot()
    {
        gunshot.start();
    }

    public void PlayExplose()
    {
        explose.start();
    }

    public void PlayJump()
    {
        jump.start();
    }
    public void PlayJumpOnEnemy()
    {
        jumpOnEnemy.start();
    }
}
