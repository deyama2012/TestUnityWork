using UnityEngine;

namespace Slots
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private Fruits type;
        [SerializeField] private Texture2D texture;

        public Fruits Type => type;
        public Texture2D Texture => texture;
    }

    public enum Fruits
    {
        None = 0,
        Banana,
        Orange,
        Strawberry
    }
}
