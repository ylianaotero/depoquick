using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class SqlRepository<T> : IRepository<T> where T : class
{
    private const string ElementNotFoundExceptionMessage = "Elemento no encontrado";
    
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
        T element = _entities.Find(id);
        
        LoadEntities(new List<T> {element});
        
        return element;
    }
    
    public List<T> GetBy(Func<T, bool> predicate)
    {
        List<T> elements = _entities.Where(predicate).ToList();
        
        LoadEntities(elements);
        
        return elements;
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
        var isNavigationCollection = _database.Entry(element).Metadata.FindNavigation(property.Name) != null;
        if (isNavigationCollection)
        {
            _database.Entry(element).Collection(property.Name).Load();
        }
    }
    
    private void LoadReferences(T element, PropertyInfo property)
    {
        var isNavigationProperty = _database.Entry(element).Metadata.FindNavigation(property.Name) != null;
        if (isNavigationProperty)
        {
            _database.Entry(element).Reference(property.Name).Load();
        }
    }

    public void Update(T element)
    {
        _entities.Update(element);
 
        _database.SaveChanges();
    }

    public void Delete(int id)
    {
        T existingElement = GetTById(id);
        
        if (existingElement == null)
        {
            throw new ArgumentNullException( ElementNotFoundExceptionMessage);
        }
        
        _entities.Remove(existingElement);
        
        _database.SaveChanges();
    }
    
    public void Reload(T element)
    {
        _database.Entry(element).Reload();
    }

    private T GetTById(int id)
    {
        T element = _entities.Find(id);

        return element;
    }
}