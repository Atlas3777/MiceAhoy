using System;
using UnityEngine;

namespace Game.Script
{
    [Serializable]
    public class PickableItemWrapper
    {
        public SpriteRenderer PickableItemSpriteRenderer;
        public SpriteRenderer PlateItemSpriteRenderer;
        public Sprite PickableItemSprite;
        public Sprite PlateItemSprite;
    }
}