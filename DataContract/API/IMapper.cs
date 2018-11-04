namespace DataContract.API
{
    public interface IMapper<TSource, TTarget>
    {
        TTarget Map(TSource objectToMAp);
    }
}
