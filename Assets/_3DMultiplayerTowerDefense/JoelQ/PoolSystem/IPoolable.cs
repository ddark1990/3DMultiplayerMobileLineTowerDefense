using System;

namespace JoelQ.GameSystem {
    public interface IPoolable<T> {
        event Action<T> OnReturnPoolEvent;
    }
}