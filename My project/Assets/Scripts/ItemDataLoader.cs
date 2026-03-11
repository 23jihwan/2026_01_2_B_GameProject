using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;


public class ItemDataLoader : MonoBehaviour
{
    [SerializeField]
    private string jsonFileName = "items";      //Resource 폴더에서 가져올 JSON 파일 이름

    private List<ItemData> itemList;

    private void Start()
    {
        LoadItemData();
    }
    //한글 인코딩을 위한 핼퍼 함수

    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";      //텍스트가 NULL 값이면 함수를 끝낸다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);    //string 을 Byte배열로 변환한 후
        return Encoding.UTF8.GetString(bytes);          //언코딩을 UTF8로 바꾼다.
    }

    void LoadItemData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile != null)
        {
            //원본 텍스트에서 UTF8로 변환 처리
            byte[] bytes = Encoding.Default.GetBytes(jsonFile.text);
            string currnetText = Encoding.UTF8.GetString(bytes);

            //변환 된 텍스트 사용
            itemList = JsonConvert.DeserializeObject<List<ItemData>>(currnetText);

            foreach (var item in itemList)
            {
                Debug.Log($"아이템 : {EncodeKorean(item.itemName)}, 설명 : {EncodeKorean(item.description)}"); 
            }
        }
        else
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다. :{jsonFileName}");
        }
    }
}
