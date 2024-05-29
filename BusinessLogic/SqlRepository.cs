using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class SqlRepository<T> : IRepository<T> where T : class
{
    private DepoQuickContext _database;
    private DbSet<T> _entities;

    public SqlRepository(DepoQuickContext database)
    {
        this._database = database;
        this._entities = database.Set<T>();
    }

    public void Add(T element)
    {
        _entities.Add(element);
        
        _database.SaveChanges();
    }

    public T? GetById(int id)
    {
        return _entities.Find(id);
    }
    
    public List<T> GetBy(Func<T, bool> predicate)
    {
        return _entities.Where(predicate).ToList();
    }
    
    public List<T> GetAll()
    {
        List<T> list = _entities.ToList();
        
        LoadEntities(list);

        return list;
    }
    
    private void LoadEntities(List<T> entities)
    {
        foreach (var element in entities)
        {
            List<PropertyInfo> properties = element.GetType().GetProperties().ToList();
            foreach (var property in properties)
            {
                bool isList = property.PropertyType.IsGenericType &&
                              property.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
                if (isList)
                {
                    LoadCollections(element, property);
                }
                else
                {
                    LoadReferences(element, property);
                }
            }
        }
    }
    
    private void LoadCollections(T element, PropertyInfo property)
    {
        _database.Entry(element).Collection(property.Name).Load();
    }
    
    private void LoadReferences(T element, PropertyInfo property)
    {
        _database.Entry(element).Reference(property.Name).Load();
    }

    public void Update(T element)
    {
        _entities.Update(element);
 
        _database.SaveChanges();
    }

    public void Delete(int id)
    {
        T existingElement = GetById(id);
        _entities.Remove(existingElement);
        
        _database.SaveChanges();
    }
}