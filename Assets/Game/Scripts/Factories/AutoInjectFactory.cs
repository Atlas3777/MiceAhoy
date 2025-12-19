using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Factories
{
    public class AutoInjectFactory
    {
        public AutoInjectFactory()
        {
            Debug.Log("create auto inject fact");
        }

        public AutoInjectModule CreateAutoInjectModule()
        {
            Debug.Log("create auto inject");
            return new();
        }
    }
}