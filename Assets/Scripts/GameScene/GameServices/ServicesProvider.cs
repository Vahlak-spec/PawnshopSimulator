using System;
using System.Collections.Generic;

namespace PawnshopSimulator.Services
{
    public class ServicesProvider
    {
        public Ticker Ticker => _ticker;

        private readonly Dictionary<Type, IGameService> _services;

        private Ticker _ticker;

        public ServicesProvider(Ticker ticker)
        {
            _ticker = ticker;
            _services = new Dictionary<Type, IGameService>();
        }

        public void AddService(IGameService gameComponent)
        {
            var type = gameComponent.GetType();

            if (_services.ContainsKey(type))
                throw new InvalidOperationException($"Êîìïîíåíò {type.Name} âæå º!");

            _services.Add(type, gameComponent);
        }

        public T GetService<T>() where T : class, IGameService
        {
            if (_services.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }

            throw new KeyNotFoundException($"Êîìïîíåíò òèïà {typeof(T).Name} íå çíàéäåíî!");
        }

        public void Bind()
        {
            foreach (var component in _services.Values)
                component.Bind(this);
        }
        public void LaunchGame()
        {
            foreach (var component in _services.Values)
                component.OnLaunchGame();
        }
    }
}
