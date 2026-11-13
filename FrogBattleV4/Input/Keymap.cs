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
                // Scan only non-idle keys (to avoid messier priority)
                GetStatus(previous[key], current[key]) != InputStatus.Idle)
            .Aggregate(default(InputState), (state, key) =>
                // Pressed and Released states take priority over Idle and Held
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

        /// <summary>
        /// Map several keys to one given input action.
        /// </summary>
        /// <param name="action">Action to map the following keys to.</param>
        /// <param name="keys">Keys to associate with the given action.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="ArgumentException">
        /// One of the specified keys has already been mapped to another action.
        /// </exception>
        public KeymapBuilder Map(InputAction action, params Keys[] keys)
        {
            try
            {
                foreach (var key in keys)
                {
                    _map.Add(key, action);
                }

                return this;
            }
            catch (ArgumentException e)
            {
                // Undo this operation to avoid leaving the builder in a
                // sorta-invalid state (or have it seem that way to the caller)
                foreach (var key in keys)
                {
                    _map.Remove(key);
                }

                throw new ArgumentException("Duplicate key mapping is not allowed.", nameof(keys), e);
            }
        }

        public Keymap Build()
        {
            return new Keymap(_map.ToFrozenDictionary());
        }
    }
}