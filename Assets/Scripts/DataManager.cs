using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] // [Serializable]는 클래스 또는 구조체를 직렬화 할 수 있음을 나타냄
public class SaveData // 세이브 할 데이터 -> GameManager 스크립트에 저장되어 있는 (Jelly 리스트)와 (jelatin), (gold) 그리고 (젤리 해금 리스트) , Jelly 스크립트에 저장되어 있는 각 젤리의 (id), (level), (exp), (위치)
{
    // 그 중 여기서 저장할 정보
    public int jelatin;                              // 젤라틴 수
    public int gold;                                 // 골드 수
    
    public bool[] jelly_unlock_list = new bool[12];  // 해금한 젤리들
    public List<Data> jelly_list = new List<Data>(); // Jelly 리스트
    
    public int num_level;                            // 젤리 아파트의 레벨
    public int click_level;                          // 젤리 꾹꾹이의 레벨

    public float bgm_vol;                            // 배경음 볼륨
    public float sfx_vol;                            // 효과음 볼륨

    public int index;                                // 배경음 종류
}

public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        JsonLoad();
    }

    // 유니티에서 기본으로 제공하는 JsonUility를 이용해 Json 형식으로 데이터를 저장하고 불러오는 기본적인 방법임

    // 이전에 저장했던 데이터를 GameManager로 저장
    public void JsonLoad()
    {
        SaveData save_data = new SaveData();

        if (!File.Exists(path))
        {
            GameManager.instance.jelatin = 0;     // 젤라틴 수 초기화
            GameManager.instance.gold = 0;        // 골드 수 초기화
            GameManager.instance.num_level = 1;   // 젤리 아파트의 레벨 초기화
            GameManager.instance.click_level = 1; // 젤리 꾹꾹이의 레벨 초기화
            GameManager.instance.index = 0;       // 배경음 종류 초기화
            JsonSave();
        }
        else
        {
            string load_json = File.ReadAllText(path);
            save_data = JsonUtility.FromJson<SaveData>(load_json);

            if (save_data != null)
            {
                // 젤리에 관한 변수들
                for (int i = 0; i < save_data.jelly_list.Count; ++i)                            // 지금 수용하고 있는 젤리의 개수만큼 반복
                    GameManager.instance.jelly_data_list.Add(save_data.jelly_list[i]);          // 수용하고 있는 젤리들을 저장할 값 (현재 값)으로 설정함

                for (int i = 0; i < save_data.jelly_unlock_list.Length; ++i)                    // 해금한 젤리 배열 길이 (해금한 젤리 수)만큼 반복
                    GameManager.instance.jelly_unlock_list[i] = save_data.jelly_unlock_list[i]; // 해금한 젤리의 종류를 저장할 값 (현재 값)으로 설정함

                // 게임에 관한 변수들
                GameManager.instance.jelatin = save_data.jelatin;                               // 젤라틴 수를 저장할 값 (현재 값)으로 설정함
                GameManager.instance.gold = save_data.gold;                                     // 골드 수를 저장할 값 (현재 값)으로 설정함

                GameManager.instance.num_level = save_data.num_level;                           // 젤리 아파트의 레벨을 저장할 값 (현재 값)으로 설정함
                GameManager.instance.click_level = save_data.click_level;                       // 젤리 꾹꾹이의 레벨을 저장할 값 (현재 값)으로 설정함

                SoundManager.instance.bgm_slider.value = save_data.bgm_vol;                     // 배경음 볼륨을 저장할 값 (현재 값)으로 설정함
                SoundManager.instance.sfx_slider.value = save_data.sfx_vol;                     // 효과음 볼륨을 저장할 값 (현재 값)으로 설정함

                GameManager.instance.index = save_data.index;                                   // 배경음 종류를 저장할 값 (현재 값)으로 설정함
            }
        }
    }

    // GameManager에 저장되어 있는 데이터를 새로운 객체에 저장
    public void JsonSave()
    {
        SaveData save_data = new SaveData();

        for (int i = 0; i < GameManager.instance.jelly_list.Count; ++i)                                                // 수용하고 있는 젤리의 개수만큼 반복
        {
            Jelly jelly = GameManager.instance.jelly_list[i];                                                          // 수용하고 있는 젤리들을 jelly 변수에 저장후
            save_data.jelly_list.Add(new Data(jelly.gameObject.transform.position, jelly.id, jelly.level, jelly.exp)); // 젤리를 저장할 배열에 jelly 변수를 이용해서 젤리들의 정보들을 저장함
        }
        
        for (int i = 0; i < GameManager.instance.jelly_unlock_list.Length; ++i)                                        // 해금한 젤리 배열 길이 (해금한 젤리 수)만큼 반복
            save_data.jelly_unlock_list[i] = GameManager.instance.jelly_unlock_list[i];                                // 해금한 젤리의 종류를 GameManager에 저장

        save_data.jelatin = GameManager.instance.jelatin;                                                              // 젤라틴 수를 GameManager에 저장
        save_data.gold = GameManager.instance.gold;                                                                    // 골드 수를 GameManager에 저장
        
        save_data.num_level = GameManager.instance.num_level;                                                          // 젤리 아파트의 레벨을 GameManager에 저장
        save_data.click_level = GameManager.instance.click_level;                                                      // 젤리 꾹꾹이의 레벨을 GameManager에 저장

        save_data.bgm_vol = SoundManager.instance.bgm_slider.value;                                                    // 배경음 볼륨을 GameManager에 저장
        save_data.sfx_vol = SoundManager.instance.sfx_slider.value;                                                    // 효과음 볼륨을 GameManager에 저장

        save_data.index = GameManager.instance.index;                                                                  // 배경음 종류를 GameManager에 저장

        string json = JsonUtility.ToJson(save_data, true);

        File.WriteAllText(path, json);
    }
}