using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteMirror : MonoBehaviour
{
    public List<Sprite> originalSprites; // ������ ����������� �������
    public string mirroredSpriteName = "R_Walk"; // ����� ������������� �������

    void Start()
    {
        if (originalSprites.Count > 0 && originalSprites[0].texture.isReadable)
        {
            Sprite mirroredSprite = MirrorSprite(originalSprites[0]);
            SaveMirroredSprite(mirroredSprite, mirroredSpriteName);
            // ��������� ������ ����������� ������� � ������������ ��������
            originalSprites[0] = mirroredSprite;
        }
        else
        {
            Debug.LogError("No readable sprite found in the list. Please make sure the list is not empty and the sprites are readable.");
        }
    }

    private Sprite MirrorSprite(Sprite original)
    {
        // ��������� ���� �������� � ������������ �������
        Texture2D newTexture = new Texture2D(original.texture.width, original.texture.height);
        for (int y = 0; y < original.texture.height; y++)
        {
            for (int x = 0; x < original.texture.width; x++)
            {
                newTexture.SetPixel(original.texture.width - 1 - x, y, original.texture.GetPixel(x, y));
            }
        }
        newTexture.Apply();

        // ��������� ������ ������� � ������������ ��������
        return Sprite.Create(newTexture, original.rect, new Vector2(0.5f, 0.5f));
    }

#if UNITY_EDITOR
    private void SaveMirroredSprite(Sprite sprite, string name)
    {
        // ���������� �������� �� ���� PNG � Assets
        byte[] bytes = sprite.texture.EncodeToPNG();
        string path = "Assets/" + name + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();

        // ������������ ��������� ��������
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

        // ��������� ������� � ����������� ��������
        Sprite newSprite = Sprite.Create(texture, sprite.rect, new Vector2(0.5f, 0.5f));
    }
#endif
}
