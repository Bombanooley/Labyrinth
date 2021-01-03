using UnityEngine;
using UnityEngine.UI;


namespace Labyrinth
{
    public sealed class DisplayScore
    {
        private Text _text;

        public DisplayScore(Text text)
        {
            _text = text;
        }

        public void Display (int value, int desire)
        {
            _text.text = $"{value} из {desire}";
        }

        public void ClearDisplay()
        {
            _text.text = "";
        }

    }
}
