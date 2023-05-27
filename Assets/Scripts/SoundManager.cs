using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audio_clips;
    public AudioClip[] bgm_clips;

    // SoundManager에 있는 2개의 player 오브젝트 객체
    AudioSource bgm_player;
    AudioSource sfx_player;

    // 옵션에서 설정할 사운드 변수들
    public Slider bgm_slider; // 배경음 변수
    public Slider sfx_slider; // 효과음 변수

    public static SoundManager instance; // instance 변수를 활용해서 어디서든 PlaySound() 함수에 쉽게 접근 할 수 있게 함

    void Awake()
    {
        instance = this; // instance 변수를 this로 지정함

        bgm_player = GameObject.Find("BGM Player").GetComponent<AudioSource>(); // BMG Player의 Audio Souce 컴퍼넌트를 가져옴
        sfx_player = GameObject.Find("Sfx Player").GetComponent<AudioSource>(); // Sfx Player의 Audio Souce 컴퍼넌트를 가져옴

        bgm_slider = bgm_slider.GetComponent<Slider>();                         // bgm_slider의 컴퍼넌트를 가져옴
        sfx_slider = sfx_slider.GetComponent<Slider>();                         // sfx_slider의 컴퍼넌트를 가져옴

        // onValueChanged를 통해 슬라이더의 값이 변경되었을 때 발생될 이벤트를 지정 할 수 있게 함
        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);
    }

    public void PlaySound(string type) // 소리를 내는 함수  /  매개변수로 상황을 지정해서 상황에 맞는 사운드를 출력할 수 있게함
    {
        int index = 0;

        // 각 상황에 맞는 사운드 출력
        switch (type)
        {
            case "Get Jelatin": index = 0; break;
            case "Grow": index = 1; break;
            case "Get Gold": index = 2; break;
            case "Buy": index = 3; break;
            case "Unlock": index = 4; break;
            case "Fail": index = 5; break;
            case "Button": index = 6; break;
            case "Pause In": index = 7; break;
            case "Pause Out": index = 8; break;
            case "Clear": index = 9; break;
        }

        sfx_player.clip = audio_clips[index];
        sfx_player.Play();
        // 각 상황에 맞는 사운드 출력
    }

    // 지정된 함수들은 float형 매개변수(변경된 값)를 하나 받아와야 함
    void ChangeBgmSound(float value)
    {
        bgm_player.volume = value; // bgm_player.volume을 받아온 float형 매개변수로 저장함
    }

    void ChangeSfxSound(float value)
    {
        sfx_player.volume = value; // sfx_player.volume을 받아온 float형 매개변수로 저장함
    }
    // 지정된 함수들은 float형 매개변수(변경된 값)를 하나 받아와야 함
}
