using UnityEngine;

namespace TowerDefence.Towers
{
    public class TowerDummy : MonoBehaviour
    {
        public void Configure()
        {
            _colorizeService.Configure(GetComponentsInChildren<MeshRenderer>());
        }
        
        public void Colorize(Color color)
        {
            _colorizeService.Colorize(color);
        }

        public void RestoreColors()
        {
            _colorizeService.ResetColor();
        }
        
        private readonly ColorizeService _colorizeService = new();
    }
}