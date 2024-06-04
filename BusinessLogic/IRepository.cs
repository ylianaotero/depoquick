namespace BusinessLogic;

public interface IRepository<T>
{
    void Add(T element);
    
    T? GetById(int id);

    List<T> GetBy(Func<T, bool> predicate);
    
    List<T> GetAll();
    
    void Update(T element);
    
    void Delete(int id);

    void Reload(T element);
}