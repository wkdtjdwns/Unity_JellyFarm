using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audio_clips;
    public AudioClip[] bgm_clips;

    // SoundManager�� �ִ� 2���� player ������Ʈ ��ü
    AudioSource bgm_player;
    AudioSource sfx_player;

    // �ɼǿ��� ������ ���� ������
    public Slider bgm_slider; // ����� ����
    public Slider sfx_slider; // ȿ���� ����

    public static SoundManager instance; // instance ������ Ȱ���ؼ� ��𼭵� PlaySound() �Լ��� ���� ���� �� �� �ְ� ��

    void Awake()
    {
        instance = this; // instance ������ this�� ������

        bgm_player = GameObject.Find("BGM Player").GetComponent<AudioSource>(); // BMG Player�� Audio Souce ���۳�Ʈ�� ������
        sfx_player = GameObject.Find("Sfx Player").GetComponent<AudioSource>(); // Sfx Player�� Audio Souce ���۳�Ʈ�� ������

        bgm_slider = bgm_slider.GetComponent<Slider>();                         // bgm_slider�� ���۳�Ʈ�� ������
        sfx_slider = sfx_slider.GetComponent<Slider>();                         // sfx_slider�� ���۳�Ʈ�� ������

        // onValueChanged�� ���� �����̴��� ���� ����Ǿ��� �� �߻��� �̺�Ʈ�� ���� �� �� �ְ� ��
        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);
    }

    public void PlaySound(string type) // �Ҹ��� ���� �Լ�  /  �Ű������� ��Ȳ�� �����ؼ� ��Ȳ�� �´� ���带 ����� �� �ְ���
    {
        int index = 0;

        // �� ��Ȳ�� �´� ���� ���
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
        // �� ��Ȳ�� �´� ���� ���
    }

    // ������ �Լ����� float�� �Ű�����(����� ��)�� �ϳ� �޾ƿ;� ��
    void ChangeBgmSound(float value)
    {
        bgm_player.volume = value; // bgm_player.volume�� �޾ƿ� float�� �Ű������� ������
    }

    void ChangeSfxSound(float value)
    {
        sfx_player.volume = value; // sfx_player.volume�� �޾ƿ� float�� �Ű������� ������
    }
    // ������ �Լ����� float�� �Ű�����(����� ��)�� �ϳ� �޾ƿ;� ��
}
