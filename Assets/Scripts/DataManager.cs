using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] // [Serializable]�� Ŭ���� �Ǵ� ����ü�� ����ȭ �� �� ������ ��Ÿ��
public class SaveData // ���̺� �� ������ -> GameManager ��ũ��Ʈ�� ����Ǿ� �ִ� (Jelly ����Ʈ)�� (jelatin), (gold) �׸��� (���� �ر� ����Ʈ) , Jelly ��ũ��Ʈ�� ����Ǿ� �ִ� �� ������ (id), (level), (exp), (��ġ)
{
    // �� �� ���⼭ ������ ����
    public int jelatin;                              // ����ƾ ��
    public int gold;                                 // ��� ��
    
    public bool[] jelly_unlock_list = new bool[12];  // �ر��� ������
    public List<Data> jelly_list = new List<Data>(); // Jelly ����Ʈ
    
    public int num_level;                            // ���� ����Ʈ�� ����
    public int click_level;                          // ���� �ڲ����� ����

    public float bgm_vol;                            // ����� ����
    public float sfx_vol;                            // ȿ���� ����

    public int index;                                // ����� ����
}

public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        JsonLoad();
    }

    // ����Ƽ���� �⺻���� �����ϴ� JsonUility�� �̿��� Json �������� �����͸� �����ϰ� �ҷ����� �⺻���� �����

    // ������ �����ߴ� �����͸� GameManager�� ����
    public void JsonLoad()
    {
        SaveData save_data = new SaveData();

        if (!File.Exists(path))
        {
            GameManager.instance.jelatin = 0;     // ����ƾ �� �ʱ�ȭ
            GameManager.instance.gold = 0;        // ��� �� �ʱ�ȭ
            GameManager.instance.num_level = 1;   // ���� ����Ʈ�� ���� �ʱ�ȭ
            GameManager.instance.click_level = 1; // ���� �ڲ����� ���� �ʱ�ȭ
            GameManager.instance.index = 0;       // ����� ���� �ʱ�ȭ
            JsonSave();
        }
        else
        {
            string load_json = File.ReadAllText(path);
            save_data = JsonUtility.FromJson<SaveData>(load_json);

            if (save_data != null)
            {
                // ������ ���� ������
                for (int i = 0; i < save_data.jelly_list.Count; ++i)                            // ���� �����ϰ� �ִ� ������ ������ŭ �ݺ�
                    GameManager.instance.jelly_data_list.Add(save_data.jelly_list[i]);          // �����ϰ� �ִ� �������� ������ �� (���� ��)���� ������

                for (int i = 0; i < save_data.jelly_unlock_list.Length; ++i)                    // �ر��� ���� �迭 ���� (�ر��� ���� ��)��ŭ �ݺ�
                    GameManager.instance.jelly_unlock_list[i] = save_data.jelly_unlock_list[i]; // �ر��� ������ ������ ������ �� (���� ��)���� ������

                // ���ӿ� ���� ������
                GameManager.instance.jelatin = save_data.jelatin;                               // ����ƾ ���� ������ �� (���� ��)���� ������
                GameManager.instance.gold = save_data.gold;                                     // ��� ���� ������ �� (���� ��)���� ������

                GameManager.instance.num_level = save_data.num_level;                           // ���� ����Ʈ�� ������ ������ �� (���� ��)���� ������
                GameManager.instance.click_level = save_data.click_level;                       // ���� �ڲ����� ������ ������ �� (���� ��)���� ������

                SoundManager.instance.bgm_slider.value = save_data.bgm_vol;                     // ����� ������ ������ �� (���� ��)���� ������
                SoundManager.instance.sfx_slider.value = save_data.sfx_vol;                     // ȿ���� ������ ������ �� (���� ��)���� ������

                GameManager.instance.index = save_data.index;                                   // ����� ������ ������ �� (���� ��)���� ������
            }
        }
    }

    // GameManager�� ����Ǿ� �ִ� �����͸� ���ο� ��ü�� ����
    public void JsonSave()
    {
        SaveData save_data = new SaveData();

        for (int i = 0; i < GameManager.instance.jelly_list.Count; ++i)                                                // �����ϰ� �ִ� ������ ������ŭ �ݺ�
        {
            Jelly jelly = GameManager.instance.jelly_list[i];                                                          // �����ϰ� �ִ� �������� jelly ������ ������
            save_data.jelly_list.Add(new Data(jelly.gameObject.transform.position, jelly.id, jelly.level, jelly.exp)); // ������ ������ �迭�� jelly ������ �̿��ؼ� �������� �������� ������
        }
        
        for (int i = 0; i < GameManager.instance.jelly_unlock_list.Length; ++i)                                        // �ر��� ���� �迭 ���� (�ر��� ���� ��)��ŭ �ݺ�
            save_data.jelly_unlock_list[i] = GameManager.instance.jelly_unlock_list[i];                                // �ر��� ������ ������ GameManager�� ����

        save_data.jelatin = GameManager.instance.jelatin;                                                              // ����ƾ ���� GameManager�� ����
        save_data.gold = GameManager.instance.gold;                                                                    // ��� ���� GameManager�� ����
        
        save_data.num_level = GameManager.instance.num_level;                                                          // ���� ����Ʈ�� ������ GameManager�� ����
        save_data.click_level = GameManager.instance.click_level;                                                      // ���� �ڲ����� ������ GameManager�� ����

        save_data.bgm_vol = SoundManager.instance.bgm_slider.value;                                                    // ����� ������ GameManager�� ����
        save_data.sfx_vol = SoundManager.instance.sfx_slider.value;                                                    // ȿ���� ������ GameManager�� ����

        save_data.index = GameManager.instance.index;                                                                  // ����� ������ GameManager�� ����

        string json = JsonUtility.ToJson(save_data, true);

        File.WriteAllText(path, json);
    }
}