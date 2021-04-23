using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.DTO
{
    public class Obj
    {
        public int Id { get; set; }
        public Obj2 Props { get; set; }
    }
    public class Obj2
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }
}
