using System;
using SadPumpkin.Util.Context;

public interface IDataDrawer<T>
{
    /// <summary>
    /// Draw the editor window GUI for the generic type T.
    /// </summary>
    /// <param name="value">Current value of the generic type.</param>
    /// <param name="context">Context containing dependencies.</param>
    /// <param name="lateRef">Late-evaluating alternate for ref, pass out new reference if value ref changes.</param>
    void DrawGUI(T value, IContext context, Action<T> lateRef);
}