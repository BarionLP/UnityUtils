namespace Ametrin.Utils
{
    public readonly struct ValueChangeArg<T>
    {
        public readonly T Old;
        public readonly T New;

        public ValueChangeArg(T old, T @new)
        {
            Old = old;
            New = @new;
        }
    }
}