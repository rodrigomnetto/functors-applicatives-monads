
namespace functors_applicatives_monads
{
    using State = int;

    //State transformer State -> (a, State)
    public record ST<T>(Func<State, (T, State)> F)
    {
        public (T, State) Run(State s) => F(s);
        public static ST<T> Pack(Func<State, (T, State)> f) => new(f);
    }

    public static class StateTypeDefinitions
    {
        //Functor (a -> b) -> ST a -> ST b
        public static ST<B> Map<A, B>(this Func<A, B> f, ST<A> st)
        => ST<B>.Pack((State s) =>
        {
            (var r1, var s1) = st.Run(s);
            return (f(r1), s1);
        });

        //Pure f -> ST f
        public static ST<T> Pure<T>(this T f) => ST<T>.Pack((State s) => (f, s));

        //Return f -> ST f
        public static ST<T> Return<T>(this T f) => Pure(f);

        //Applicative ST (a -> b) -> ST a -> ST b 
        public static ST<B> Apply<A, B>(this ST<Func<A, B>> f, ST<A> st)
        => ST<B>.Pack((State s) =>
        {
            (var f1, var s1) = f.Run(s);
            (var r, var s2) = st.Run(s1);
            return (f1(r) ,s2);
        });

        //Monad ST a -> (a -> ST b) -> ST b
        public static ST<B> Bind<A, B>(this ST<A> st, Func<A, ST<B>> f)
        => ST<B>.Pack((State s) =>
        {
            (var r1, var s1) = st.Run(s);
            var st2 = f(r1);
            return st2.Run(s1);
        });  
    }
}
