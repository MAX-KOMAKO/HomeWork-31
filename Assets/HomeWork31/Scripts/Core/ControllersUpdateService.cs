using System.Collections.Generic;

public class ControllersUpdateService
{
    private List<Controller> _controllers = new List<Controller>();

    public void Add(Controller controller)
    {
        _controllers.Add(controller);
        controller.Enable();
    }

    public void Remove(Controller controller)
    {
        controller.Disable();
        _controllers.Remove(controller);
    }

    public void Update(float deltaTime)
    {
        for (int i = _controllers.Count - 1; i >= 0; i--)
        {
            _controllers[i].Update(deltaTime);
        }
    }

    public void Clear()
    {
        foreach (Controller controller in _controllers)
        {
            controller.Disable();
        }
        _controllers.Clear();
    }
}