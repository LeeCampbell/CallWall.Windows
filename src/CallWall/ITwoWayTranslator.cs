namespace CallWall
{
    public interface ITwoWayTranslator<TSource, TTarget>
    {
        TTarget Translate(TSource source);
        TSource TranslateBack(TTarget target);
    }
}