using System;
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


namespace TodoApi_Dapper.Tests
{
    public class TodoControllerTests
    {
        private readonly ITestOutputHelper _output;
        private List<TodoItem> itemlist;
        private Mock<IUnitOfWork> uowMoq;
        private Mock<ITodoItemRepository> todoItemRepoMoq;
        private TodoController controller;


        public TodoControllerTests(ITestOutputHelper output)
        {
            _output = output;
            itemlist = new List<TodoItem>()
            {
                new TodoItem{ Id = Guid.NewGuid().ToString("N"), Name = "Test1", IsComplete = false},
                new TodoItem{ Id = Guid.NewGuid().ToString("N"), Name = "Test2", IsComplete = false},
                new TodoItem{ Id = Guid.NewGuid().ToString("N"), Name = "Test3", IsComplete = false}
            };
            uowMoq = new Mock<IUnitOfWork>();
            todoItemRepoMoq = new Mock<ITodoItemRepository>();
            todoItemRepoMoq.Setup(x => x.FindAllAsync()).ReturnsAsync(itemlist);
            todoItemRepoMoq.Setup(x => x.FindAsync(It.IsAny<String>()))
                .ReturnsAsync((String id) => itemlist.Find(x => x.Id == id));
            todoItemRepoMoq.SetupAsync(x => x.Add(It.IsAny<TodoItem>()))
                .Callback<TodoItem>(item => itemlist.Add(item));
            todoItemRepoMoq.SetupAsync(x => x.Update(It.IsAny<TodoItem>()))
                .Callback<TodoItem>(item =>
                {
                    var index = itemlist.FindIndex(x => x.Id == item.Id);
                    itemlist[index] = item;
                });
            todoItemRepoMoq.SetupAsync(x => x.Remove(It.IsAny<TodoItem>()))
                .Callback<TodoItem>(item => itemlist.Remove(item));
            todoItemRepoMoq.Setup(x => x.Count()).Returns(itemlist.Count);
            uowMoq.Setup(x => x.TodoItems).Returns(todoItemRepoMoq.Object);

            controller = new TodoController(uowMoq.Object);
        }

        [Fact(DisplayName = "GET: api/todo が正しく動作する")]
        public async Task OkGetTodoItemsTest()
        {
            var result = await controller.GetTodoItems();
            
            Assert.Equal(itemlist, result);
        }

        [Fact(DisplayName = "GET: api/todo/{id} が正しく動作する")]
        public async Task OkGetTodoItemTest()
        {
            // itemlist[0]のIdを指定して値を取得
            var targetId = itemlist[0].Id;
            var result = await controller.GetTodoItem(targetId);

            // resultがitemlist[0]と等しいこと
            Assert.Equal(itemlist[0], result.Value);
        }
    }
}
