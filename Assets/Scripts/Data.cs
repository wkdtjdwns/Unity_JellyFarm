using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // [Serializable]�� Ŭ���� �Ǵ� ����ü�� ����ȭ �� �� ������ ��Ÿ��

public class Data // ���̺� �� ������ -> GameManager ��ũ��Ʈ�� ����Ǿ� �ִ� (Jelly ����Ʈ)�� (jelatin), (gold) �׸��� (���� �ر� ����Ʈ) , Jelly ��ũ��Ʈ�� ����Ǿ� �ִ� �� ������ (id), (level), (exp), (��ġ)
{
    // �� �� ���⼭ ������ ����
    public int id;      // �������� id
    public int level;   // �������� ����
    public float exp;   // �������� ����ġ
    public Vector3 pos; // �������� ��ġ
   
    // Data Ŭ������ ������ ��
    public Data(Vector3 pos, int id, int level, float exp) // GameManager ��ũ��Ʈ���� �����ϴ� ���
    {
        this.pos = pos;
        this.id = id;
        this.level = level;
        this.exp = exp;
    }
}