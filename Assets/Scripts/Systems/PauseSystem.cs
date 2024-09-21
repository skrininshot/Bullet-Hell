using System.Collections.Generic;

public class PauseSystem
{
    public bool IsPaused { get; private set; }

    private readonly TimeShifter _timeShifter;
    private readonly List<IPausable> _pausables = new();

    public PauseSystem(TimeShifter timeShifter)
    {
        _timeShifter = timeShifter;
    }

    public void SetPause(bool value)
    {
        if (IsPaused == value) return;

        IsPaused = value;

        if (IsPaused)
        {
            _timeShifter.RegisterUser(this, 0f);

            foreach (var pausable in _pausables)
                pausable.Pause();
        }   
        else
        {
            _timeShifter.UnregisterUser(this);

            foreach (var pausable in _pausables)
                pausable.Resume();
        }   
    }

    public void RegisterPausable(IPausable obj)
    {
        if (!_pausables.Contains(obj))
        {
            _pausables.Add(obj);

            if (IsPaused)
                obj.Pause();
        }  
    }

    public void UnregisterPausable(IPausable obj)
    {
        if (_pausables.Contains(obj))
            _pausables.Remove(obj);
    }
}
