using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // [Serializable]는 클래스 또는 구조체를 직렬화 할 수 있음을 나타냄

public class Data // 세이브 할 데이터 -> GameManager 스크립트에 저장되어 있는 (Jelly 리스트)와 (jelatin), (gold) 그리고 (젤리 해금 리스트) , Jelly 스크립트에 저장되어 있는 각 젤리의 (id), (level), (exp), (위치)
{
    // 그 중 여기서 저장할 정보
    public int id;      // 젤리들의 id
    public int level;   // 젤리들의 레벨
    public float exp;   // 젤리들의 경험치
    public Vector3 pos; // 젤리들의 위치
   
    // Data 클래스를 정의한 뒤
    public Data(Vector3 pos, int id, int level, float exp) // GameManager 스크립트에서 저장하는 방식
    {
        this.pos = pos;
        this.id = id;
        this.level = level;
        this.exp = exp;
    }
}