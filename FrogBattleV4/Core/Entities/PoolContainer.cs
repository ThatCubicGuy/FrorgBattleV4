using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Entities;

public class PoolContainer
{
    private double _currentValue;
    private readonly List<IMutatorComponent> _mutators = [];

    public PoolContainer()
    {
        Mutators = new ReadOnlyCollection<IMutatorComponent>(_mutators);
    }

    public event EventHandler<PoolValueChangedArgs>? ValueChanged;
    public double Capacity { get; set; }

    public double Value
    {
        get => _currentValue;
        set
        {
            var oldValue = _currentValue;
            _currentValue = Math.Clamp(value, 0, Capacity);
            ValueChanged?.Invoke(this, new PoolValueChangedArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue
            });
        }
    }

    public ReadOnlyCollection<IMutatorComponent> Mutators { get; }
}

public class PoolValueChangedArgs : EventArgs
{
    public double OldValue { get; set; }
    public double NewValue { get; set; }
}