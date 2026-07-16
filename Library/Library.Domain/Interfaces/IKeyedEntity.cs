using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IKeyedEntity
    {
        string Key { get; }
    }
}
