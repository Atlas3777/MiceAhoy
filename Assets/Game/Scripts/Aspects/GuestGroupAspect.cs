using System;
using System.Collections.Generic;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Script.Aspects
{
    public class GuestGroupAspect : ProtoAspectInject
    {
        public ProtoPool<GuestGroupTag> GuestGroupPool;
        public ProtoPool<TargetGroupSize>  TargetGroupSizePool;
        public ProtoPool<GroupNeedsTableTag> GroupNeedsTablePool;
        public ProtoPool<GroupGotTableEvent> GroupGotTableEventPool;
        public ProtoPool<GroupArrivedEvent> GroupArrivedEventPool;
        public ProtoPool<GuestGroupServedEvent> GuestGroupServedEventPool;
        public ProtoPool<GroupIsWalkingTag> GroupIsWalkingPool;
        public ProtoPool<WaitingTakeOrderTag>  WaitingTakeOrderTagPool;
        public ProtoPool<WaitingOrderTag> WaitingOrderTagPool;
        public ProtoPool<GuestGroupServedTag> GuestGroupServedTagPool;
    }

    [Serializable]
    public struct GuestGroupServedEvent : IComponent
    {
    }

    [Serializable]
    public struct GroupGotTableEvent : IComponent
    {
    }

    [Serializable]
    public struct GroupArrivedEvent : IComponent
    {
    }

    [Serializable]
    public struct GuestGroupServedTag : IComponent
    {
    }
    
    public struct WaitingOrderTag :IComponent
    {
    }
    
    [Serializable]
    public struct GuestGroupTag : IComponent
    {
        public List<ProtoPackedEntityWithWorld> includedGuests;
        public ProtoPackedEntityWithWorld table;
    }

    [Serializable]
    public struct GroupNeedsTableTag : IComponent
    {
    }

    [Serializable]
    public struct WaitingTakeOrderTag : IComponent
    {
    }

    [Serializable]
    public struct GroupIsWalkingTag : IComponent
    {
    }

    [Serializable]
    public struct TargetGroupSize : IComponent
    {
        public int size;
    }
}