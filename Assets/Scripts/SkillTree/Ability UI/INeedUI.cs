using System;
public interface INeedUI 
{
    public event Action OnCoolDown;
    public string Keybind { get; }
}