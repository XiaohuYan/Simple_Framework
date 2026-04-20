using System;
using System.Collections.Concurrent;

namespace SimpleFramework.Event
{
    /// <summary>
    /// 事件中心
    /// </summary>
    /// <typeparam name="TEnum">事件枚举类型</typeparam>
    public static partial class EventSystem
    {
        /// <summary>
        /// 存储所有事件id和对应的事件
        /// </summary>
        private static readonly ConcurrentDictionary<EventId, Delegate> eventDelegates = new ConcurrentDictionary<EventId, Delegate>();

        #region AddListener

        /// <summary>
        /// 添加无参事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void AddListener(EventId eventId, Action listener) => AddListenerInternal(eventId, listener);

        /// <summary>
        /// 添加一个参数事件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void AddListener<T>(EventId eventId, Action<T> listener) => AddListenerInternal(eventId, listener);

        /// <summary>
        /// 添加两个参数事件
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void AddListener<T1, T2>(EventId eventId, Action<T1, T2> listener) => AddListenerInternal(eventId, listener);

        /// <summary>
        /// 添加三个参数事件
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <typeparam name="T3">第三个参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void AddListener<T1, T2, T3>(EventId eventId, Action<T1, T2, T3> listener) => AddListenerInternal(eventId, listener);

        /// <summary>
        /// 添加事件统一接口
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        /// <exception cref="InvalidOperationException">判断事件id添加新对应事件的方法类型是否相同</exception>
        private static void AddListenerInternal(EventId eventId, Delegate listener)
        {
            eventDelegates.AddOrUpdate(eventId, listener, (_, existing) =>
            {
                // 类型检查：如果已存在委托，验证类型是否匹配
                if (existing != null && existing.GetType() != listener.GetType())
                {
                    throw new InvalidOperationException($"事件 {eventId} 已经添加了类型为 {existing.GetType()} 的监听器，无法添加类型为 {listener.GetType()} 的监听器。同一事件ID只能注册相同签名的事件。");
                }
                return Delegate.Combine(existing, listener);
            });
        }

        #endregion

        #region RemoveListener

        /// <summary>
        /// 移除无参数事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void RemoveListener(EventId eventId, Action listener) => RemoveListenerInternal(eventId, listener);

        /// <summary>
        /// 移除一个参数事件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void RemoveListener<T>(EventId eventId, Action<T> listener) => RemoveListenerInternal(eventId, listener);

        /// <summary>
        /// 移除两个参数事件
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void RemoveListener<T1, T2>(EventId eventId, Action<T1, T2> listener) => RemoveListenerInternal(eventId, listener);

        /// <summary>
        /// 移除三个参数事件
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <typeparam name="T3">第三个参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        public static void RemoveListener<T1, T2, T3>(EventId eventId, Action<T1, T2, T3> listener) => RemoveListenerInternal(eventId, listener);

        /// <summary>
        /// 移除事件统一接口
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">事件方法</param>
        private static void RemoveListenerInternal(EventId eventId, Delegate listener)
        {
            if (eventDelegates.TryGetValue(eventId, out var existingDelegate))
            {
                var newDelegate = Delegate.Remove(existingDelegate, listener);
                if (newDelegate != null)
                {
                    eventDelegates[eventId] = newDelegate;
                }
                else
                {
                    eventDelegates.TryRemove(eventId, out _);
                }
            }
        }

        #endregion

        #region Broadcast

        /// <summary>
        /// 调用无参事件
        /// </summary>
        /// <param name="eventId"></param>
        public static void Broadcast(EventId eventId) => BroadcastInternal<Action>(eventId, d => d?.Invoke());

        /// <summary>
        /// 调用一个参数事件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="arg">事件参数</param>
        public static void Broadcast<T>(EventId eventId, T arg) => BroadcastInternal<Action<T>>(eventId, d => d?.Invoke(arg));

        /// <summary>
        /// 调用两个参数事件
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="arg1">事件第一个参数</param>
        /// <param name="arg2">事件第二个参数</param>
        public static void Broadcast<T1, T2>(EventId eventId, T1 arg1, T2 arg2) => BroadcastInternal<Action<T1, T2>>(eventId, d => d?.Invoke(arg1, arg2));

        /// <summary>
        /// 调用两个参数事件
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <typeparam name="T3">第三个参数类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="arg1">事件第一个参数</param>
        /// <param name="arg2">事件第二个参数</param>
        /// <param name="arg3">事件第三个参数</param>
        public static void Broadcast<T1, T2, T3>(EventId eventId, T1 arg1, T2 arg2, T3 arg3) => BroadcastInternal<Action<T1, T2, T3>>(eventId, d => d?.Invoke(arg1, arg2, arg3));

        /// <summary>
        /// 调用事件统一接口
        /// </summary>
        /// <typeparam name="TDelegate">事件类型</typeparam>
        /// <param name="eventId">事件id</param>
        /// <param name="invokeAction">事件</param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void BroadcastInternal<TDelegate>(EventId eventId, Action<TDelegate> invokeAction) where TDelegate : Delegate
        {
            if (eventDelegates.TryGetValue(eventId, out var delegateObj))
            {
                if (delegateObj is TDelegate typedDelegate)
                {
                    // 传递类型为TDelegate的事件
                    invokeAction(typedDelegate);
                }
                else
                {
                    // 类型不匹配时抛出异常
                    throw new InvalidOperationException($"事件 {eventId} 的委托类型为 {delegateObj.GetType()}，但尝试以 {typeof(TDelegate)} 类型广播。请确保广播时使用的参数类型与注册时一致。");
                }
            }
        }

        #endregion

        #region 其他方法

        /// <summary>
        /// 清除所有事件
        /// </summary>
        public static void Clear()
        {
            eventDelegates.Clear();
        }

        /// <summary>
        /// 检查是否有某个事件的监听器
        /// </summary>
        public static bool HasListener(EventId eventId)
        {
            return eventDelegates.ContainsKey(eventId);
        }

        /// <summary>
        /// 获取某个事件的监听器数量
        /// </summary>
        public static int GetListenerCount(EventId eventId)
        {
            if (eventDelegates.TryGetValue(eventId, out var delegateObj) && delegateObj != null)
            {
                return delegateObj.GetInvocationList().Length;
            }
            return 0;
        }

        /// <summary>
        /// 获取事件当前的委托类型
        /// </summary>
        public static Type GetDelegateType(EventId eventId)
        {
            if (eventDelegates.TryGetValue(eventId, out var delegateObj))
            {
                return delegateObj?.GetType();
            }
            return null;
        }

        #endregion
    }
}