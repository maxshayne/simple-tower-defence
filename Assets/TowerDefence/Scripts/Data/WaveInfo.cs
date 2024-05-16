using System;
using System.Collections.Generic;

namespace TowerDefence.Data
{
    [Serializable]
    public class WaveInfo
    {
        public List<MobsCountInfo> MobsInfos;
        public float SpawnDelayBetweenMobs;
    }
}