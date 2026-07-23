using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Library.Repository.Repositories.Helper
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IKeyedEntity
    {
        protected abstract string FileName { get; }

        protected string filePath => @"C:\Users\deme\Desktop\‏\codes\doit_midterm\LibraryMidterm\Library\Library.Repository\Data\" + FileName;

        public virtual void Add(T entity)
        {
            string line = JsonSerializer.Serialize(entity);
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        public virtual void Delete(T entity)
        {
            List<T> values = GetAll();
            List<T> remeiningValues = values.Where(x => x.Key != entity.Key).ToList();
            SaveChanges(remeiningValues);
        }
        
        public virtual T Get(Func<T, bool> predicate)
        {
            List<T> values = GetAll();
            T value = values.FirstOrDefault(predicate);
            return value;
        }

        public virtual List<T> GetAll()
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }
            string[] lines = File.ReadAllLines(filePath);
            List<T> values = new List<T>();

            foreach (var item in lines)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                try
                {
                    T value = JsonSerializer.Deserialize<T>(item);
                    if (value != null)
                    {
                        values.Add(value);
                    }

                }
                catch (JsonException)
                {
                    continue;
                }
            }
            return values;
        }

        public virtual T GetByKey(string key)
        {
            List<T> values = GetAll();
            T value = values.FirstOrDefault(v => v.Key == key);
            return value;
        }

        public virtual void SaveChanges(List<T> entities)
        {
            File.Delete(filePath);
            File.AppendAllLines(filePath, entities.Select(e => JsonSerializer.Serialize(e, e.GetType())));
        }

        public virtual void Update(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            List<T> values = GetAll();
            int index = values.FindIndex(v => v.Key == entity.Key);
            if(index != -1)
            {
                values[index] = entity;
                SaveChanges(values);
            }
        }
    }
}
