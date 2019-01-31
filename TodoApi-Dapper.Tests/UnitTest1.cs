using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TodoAPi_Dapper.Controllers;
using TodoAPi_Dapper.Models;
using TodoAPi_Dapper.Models.Persistance;
using TodoAPi_Dapper.Models.Repository;
using TodoAPi_Dapper.Implements;
using Todos;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Core.Testing;
using Google.Protobuf;

namespace TodoApi_Dapper.Tests
{
    public class TodoControllerTests
    {
        private readonly ITestOutputHelper _output;
        private List<Todo> todoItems;
        private Mock<IUnitOfWork> uowMoq;
        private Mock<ITodoItemRepository> todoItemRepoMoq;
        // private TodoController controller;
        private TodoImplements implements;

        public TodoControllerTests(ITestOutputHelper output)
        {
            _output = output;
            todoItems = new List<Todo>()
            {
                new Todo{ Id = Guid.NewGuid().ToString("N"), Name = "Test1", IsComplete = false},
                new Todo{ Id = Guid.NewGuid().ToString("N"), Name = "Test2", IsComplete = false},
                new Todo{ Id = Guid.NewGuid().ToString("N"), Name = "Test3", IsComplete = false}
            };
            uowMoq = new Mock<IUnitOfWork>();
            todoItemRepoMoq = new Mock<ITodoItemRepository>();
            todoItemRepoMoq.Setup(x => x.FindAllAsync()).ReturnsAsync(todoItems);
            todoItemRepoMoq.Setup(x => x.FindAsync(It.IsAny<String>()))
                .ReturnsAsync((String id) => todoItems.Find(x => x.Id == id));
            todoItemRepoMoq.SetupAsync(x => x.Add(It.IsAny<Todo>()))
                .Callback<Todo>(item => todoItems.Add(item));
            todoItemRepoMoq.SetupAsync(x => x.Update(It.IsAny<Todo>()))
                .Callback<Todo>(item =>
                {
                    var index = todoItems.FindIndex(x => x.Id == item.Id);
                    todoItems[index] = item;
                });
            todoItemRepoMoq.SetupAsync(x => x.Remove(It.IsAny<Todo>()))
                .Callback<Todo>(item => todoItems.Remove(item));
            todoItemRepoMoq.Setup(x => x.Count()).Returns(todoItems.Count);
            uowMoq.Setup(x => x.TodoItems).Returns(todoItemRepoMoq.Object);

            // controller = new TodoController(uowMoq.Object);
            implements = new TodoImplements(uowMoq.Object);

        }

        [Fact(DisplayName = "GetTodoItems({})が正しく動作する")]
        public async Task OkGetTodoItemsTest()
        {
            var fakeServerCallContext = TestServerCallContext.Create("fooMethod", null, DateTime.UtcNow.AddHours(1), new Metadata(), CancellationToken.None, "127.0.0.1", null, null, (metadata) => TaskUtils.CompletedTask, () => new WriteOptions(), (writeOptions) => { });
            var response = await implements.GetTodoItems(new Empty(), fakeServerCallContext);

            Assert.Equal(todoItems, response.Todos);
        }

        // [Fact(DisplayName = "GET: api/todo/{id} が正しく動作する")]
    }
}
