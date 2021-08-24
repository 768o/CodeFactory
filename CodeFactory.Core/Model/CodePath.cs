using System;

namespace CodeFactory.Core.Model
{
    public class CodePath
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public Guid? ParentId { get; set; }
    }
}
