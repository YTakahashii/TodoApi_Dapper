using System;
using TodoAPi_Dapper.Models.Repository;

namespace TodoAPi_Dapper.Models.Persistance
{
    public interface IUnitOfWork
    {
        ITodoItemRepository TodoItems { get; }
    }
}
