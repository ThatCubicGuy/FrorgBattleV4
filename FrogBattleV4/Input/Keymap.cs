using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4.Input;

public class Keymap : IKeymap
{
    private readonly FrozenDictionary<Keys, InputAction> _map;

    private Keymap(FrozenDictionary<Keys, InputAction> map)
    {
        _map = map;
    }

    public InputState Translate(KeyboardState previous, KeyboardState current)
    {
        return _map.Keys.Where(key =>
                GetStatus(previous[key], current[key]) != InputStatus.Idle)
            .Aggregate(default(InputState), (state, key) =>
                state.WithRestricted(_map[key], GetStatus(previous[key], current[key])));
    }

    private static InputStatus GetStatus(KeyState previous, KeyState current)
    {
        return InputStatusExtensions.GetStatus(previous, current);
    }

    public static KeymapBuilder GetBuilder() => new();

    public class KeymapBuilder
    {
        private readonly Dictionary<Keys, InputAction> _map = new(Enum.GetValues<Keys>().Length);

        public KeymapBuilder Map(InputAction action, params Keys[] keys)
        {
            foreach (var key in keys)
            {
                _map.Add(key, action);
            }

            return this;
        }

        public Keymap Build()
        {
            return new Keymap(_map.ToFrozenDictionary());
        }
    }
}