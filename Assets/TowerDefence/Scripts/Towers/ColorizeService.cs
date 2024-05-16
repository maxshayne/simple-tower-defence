using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class ColorizeService
    {
        private MeshRenderer[] _renderers;

        public void Configure(MeshRenderer[] renderers)
        {
            _renderers = renderers;
            foreach (var meshRenderer in _renderers)
            {
                _defaultColors.Add(meshRenderer.material.color);
            }
        }

        public void Colorize(Color color)
        {
            foreach (var meshRenderer in _renderers)
            {
                meshRenderer.material.color = color;
            }
        }

        public void ResetColor()
        {
            for (var index = 0; index < _renderers.Length; index++)
            {
                var meshRenderer = _renderers[index];
                meshRenderer.material.color = _defaultColors[index];
            }
        }

        private readonly List<Color> _defaultColors = new();
    }
}