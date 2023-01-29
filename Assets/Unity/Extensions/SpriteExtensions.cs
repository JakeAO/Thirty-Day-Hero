using System.Threading;
using UnityEngine;

namespace Unity.Extensions
{
    public static class SpriteExtensions
    {
        private static Sprite _whiteSprite;
        private static Sprite _blackSprite;
        private static Sprite _greySprite;
        private static Sprite _redSprite;

        public static Sprite WhiteSprite => LazyInitializer.EnsureInitialized(
            ref _whiteSprite,
            () => Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height),
                new Vector2((int)(Texture2D.whiteTexture.width / 2f), (int)(Texture2D.whiteTexture.height / 2f))));

        public static Sprite BlackSprite => LazyInitializer.EnsureInitialized(
            ref _blackSprite,
            () => Sprite.Create(
                Texture2D.blackTexture,
                new Rect(0, 0, Texture2D.blackTexture.width, Texture2D.blackTexture.height),
                new Vector2((int)(Texture2D.blackTexture.width / 2f), (int)(Texture2D.blackTexture.height / 2f))));

        public static Sprite GreySprite => LazyInitializer.EnsureInitialized(
            ref _greySprite,
            () => Sprite.Create(
                Texture2D.grayTexture,
                new Rect(0, 0, Texture2D.grayTexture.width, Texture2D.grayTexture.height),
                new Vector2((int)(Texture2D.grayTexture.width / 2f), (int)(Texture2D.grayTexture.height / 2f))));

        public static Sprite RedSprite => LazyInitializer.EnsureInitialized(
            ref _redSprite,
            () => Sprite.Create(
                Texture2D.redTexture,
                new Rect(0, 0, Texture2D.redTexture.width, Texture2D.redTexture.height),
                new Vector2((int)(Texture2D.redTexture.width / 2f), (int)(Texture2D.redTexture.height / 2f))));
    }
}