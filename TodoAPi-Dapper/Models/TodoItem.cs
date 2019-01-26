using System;
namespace TodoAPi_Dapper.Models
{
    public class TodoItem: IBaseEntity
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
