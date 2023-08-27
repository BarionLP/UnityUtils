namespace Ametrin.Utils{
    public readonly struct ValueChangedEventArgs<T>{
        public readonly T Old;
        public readonly T New;

        public ValueChangedEventArgs(T old, T @new){
            Old = old;
            New = @new;
        }
    }
}
