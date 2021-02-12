using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip encounter;
    public AudioClip battleStart;
    public AudioClip levelUp;
    public AudioClip swordSlash1;
    public AudioClip atkBuff;
    public AudioClip defBuff;
    public AudioClip atkDebuff;
    public AudioClip defDebuff;
    public AudioClip fire1;
    public AudioClip fireBlast1;
    public AudioClip heal1;
    public AudioClip heal2;
    public AudioClip ice1;
    public AudioClip ice2;
    public AudioClip lightning1;
    public AudioClip lightning2;
    public AudioClip arrowShot;
    public AudioClip arrowBurst;
    public AudioClip bite;
    public AudioClip claw;
    public AudioClip punch;
    public AudioClip roar;
    public AudioClip gameOver;
    public AudioClip hover;
    public AudioClip select;

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "Encounter":
                audio.PlayOneShot(encounter);
                break;
            case "BattleStart":
                audio.PlayOneShot(battleStart);
                break;
            case "LevelUp":
                audio.PlayOneShot(levelUp);
                break;
            case "SwordSlash":
                audio.PlayOneShot(swordSlash1);
                break;
            case "AtkBuff":
                audio.PlayOneShot(atkBuff);
                break;
            case "DefBuff":
                audio.PlayOneShot(defBuff);
                break;
            case "AtkDebuff":
                audio.PlayOneShot(atkDebuff);
                break;
            case "DefDebuff":
                audio.PlayOneShot(defDebuff);
                break;
            case "Fire":
                audio.PlayOneShot(fire1);
                break;
            case "FireBlast":
                audio.PlayOneShot(fireBlast1);
                break;
            case "Heal1":
                audio.PlayOneShot(heal1);
                break;
            case "Heal2":
                audio.PlayOneShot(heal2);
                break;
            case "Ice1":
                audio.PlayOneShot(ice1);
                break;
            case "Ice2":
                audio.PlayOneShot(ice2);
                break;
            case "Lightning1":
                audio.PlayOneShot(lightning1);
                break;
            case "Lightning2":
                audio.PlayOneShot(lightning2);
                break;
            case "ArrowShot":
                audio.PlayOneShot(arrowShot);
                break;
            case "ArrowBurst":
                audio.PlayOneShot(arrowBurst);
                break;
            case "Bite":
                audio.PlayOneShot(bite);
                break;
            case "Claw":
                audio.PlayOneShot(claw);
                break;
            case "Punch":
                audio.PlayOneShot(punch);
                break;
            case "Roar":
                audio.PlayOneShot(roar);
                break;
            case "GameOver":
                audio.PlayOneShot(gameOver);
                break;
            case "Hover":
                audio.PlayOneShot(hover);
                break;
            case "Select":
                audio.PlayOneShot(select);
                break;
            default:
                break;
        }
    }
}
