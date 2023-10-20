using UnityEngine;
using EPOOutline;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public new BoxCollider collider;
    public SpriteRenderer sprite;
    public Outlinable outlinable;
    
    
    public void SetSprite(Sprite _sprite)
    {
        sprite.sprite = _sprite;
    }
}
