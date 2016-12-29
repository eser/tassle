namespace Tasslehoff.Logging
{
    public delegate void EventHandler<TArgs>(object sender, TArgs args) where TArgs : class;
}
