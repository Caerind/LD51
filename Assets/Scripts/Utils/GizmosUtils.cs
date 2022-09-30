using UnityEngine;

public static class GizmosUtils
{
    public static void DrawSprite(Sprite sprite, Vector3 position)
    {
        Rect dstRect = new Rect(position.x - sprite.bounds.max.x,
                                  position.y + sprite.bounds.max.y,
                                  sprite.bounds.size.x,
                                  -sprite.bounds.size.y);

        Rect srcRect = new Rect(sprite.rect.x / sprite.texture.width,
                                 sprite.rect.y / sprite.texture.height,
                                 sprite.rect.width / sprite.texture.width,
                                 sprite.rect.height / sprite.texture.height);

        Graphics.DrawTexture(dstRect, sprite.texture, srcRect, 0, 0, 0, 0);
    }
}
