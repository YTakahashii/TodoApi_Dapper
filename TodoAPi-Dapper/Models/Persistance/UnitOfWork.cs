using System;
using Microsoft.Extensions.Configuration;
using TodoAPi_Dapper.Models.Repository;

namespace TodoAPi_Dapper.Models.Persistance
{
    public class UnitOfWork: IUnitOfWork
    {
        private ITodoItemRepository todoItems;

        public UnitOfWork()
        {

        }

        public ITodoItemRepository TodoItems
        {
            get
            {
                if (todoItems == null)
                {
                    todoItems = new TodoItemRepository();
                }

                return todoItems;
            }
        }
    }
}
