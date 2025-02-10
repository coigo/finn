namespace Infra.Shared;

public interface IUseCase<T1, T2> {
    
    public  Task<T2> Execute(T1 data);

} 