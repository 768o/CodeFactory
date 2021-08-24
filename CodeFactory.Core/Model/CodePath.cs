using System;
using System.Collections.Generic;

namespace CodeFactory.Core.Model
{
    public class CodePath
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public List<CodePath> Childrens { get; set; } = new List<CodePath>();
    }
}
