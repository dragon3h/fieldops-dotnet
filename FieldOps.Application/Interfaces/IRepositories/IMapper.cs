namespace FieldOps.Application.Interfaces.IRepositories;

public interface IMapper<TInput, TOutput> where TInput : class where TOutput : class
{
    TOutput Map(TInput source);
    TInput MapForCreation(TOutput dtoObject);
    TInput MapForUpdate(TInput source, TOutput dtoObject );
    List<TOutput> MapList(List<TInput> source);
}