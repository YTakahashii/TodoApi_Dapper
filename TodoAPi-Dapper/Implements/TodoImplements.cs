using System;
using System.Threading.Tasks;
using Grpc;
using Grpc.Core;
using Grpc.Core.Utils;
using TodoAPi_Dapper.Models;
using TodoAPi_Dapper.Models.Persistance;
using Todos;
namespace TodoAPi_Dapper.Implements
{
    public class TodoImplements: Todos.Todos.TodosBase
    {
        private IUnitOfWork _unitOfWork;

        public TodoImplements(IUnitOfWork unitOfWork): base()
        {
            _unitOfWork = unitOfWork;
            if (_unitOfWork.TodoItems.Count() == 0)
            {
                _unitOfWork.TodoItems.Add(new Todo { Name = "Item1", IsComplete = false });
            }
        }

        public override async Task<GetTodoItemsResponse> GetTodoItems(Empty request, ServerCallContext context)
        {
            var result =  await _unitOfWork.TodoItems.FindAllAsync();
            var response = new GetTodoItemsResponse();
            foreach(var todo in result)
            {
                response.Todos.Add(todo);
            }

            return response;
        }
    }
}
