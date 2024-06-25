
namespace functors_applicatives_monads
{
    public static class MaybeTypeDefinitions
    {
        //Functor
        public static Maybe<C> Map<T, C>(this Maybe<T> m, Func<T, C> f)
            => m switch
            {
                Just<T> { Value: var x } => new Just<C>(f(x)),
                _ => new Nothing<C>()
            };

        //Applicative
        public static Maybe<T> Pure<T>(this T value) => new Just<T>(value);

        public static Maybe<C> Apply<T, C>(Maybe<Func<T, C>> m, Maybe<T> value)
            => m switch
            {
                Just<Func<T, C>> { Value: var f } => value.Map(f),
                _ => new Nothing<C>()
            };

        //Monad
        public static Maybe<C> Bind<T, C>(this Maybe<T> m, Func<T, Maybe<C>> f)
            => m switch
            {
                Just<T> { Value: var x } => f(x),
                _ => new Nothing<C>()
            };

        public record Maybe<T>();
        public record Just<T>(T Value) : Maybe<T>();
        public record Nothing<T>() : Maybe<T>;
    }
}
