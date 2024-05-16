using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Data
{
    [CreateAssetMenu(menuName = "Create LevelInfo", fileName = "LevelInfo", order = 0)]
    public class LevelInfo : ScriptableObject
    {
        [SerializeField] private List<WaveInfo> waves;

        public List<WaveInfo> Waves => waves;
    }
}