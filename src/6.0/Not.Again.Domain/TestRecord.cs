using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Not.Again.Domain
{
    public class TestRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TestRecordId { get; set; }

        [Required, ForeignKey(nameof(TestAssembly))]
        public Guid TestAssemblyId { get; set; }

        public virtual TestAssembly TestAssembly { get; set; }

        [MaxLength(256)]
        public string ClassName { get; set; }

        [MaxLength(256)]
        public string FullName { get; set; }

        [MaxLength(256)]
        public string MethodName { get; set; }

        [MaxLength(256)]
        public string TestName { get; set; }

        public string DelimitedTestArguments { get; set; }

        public long LastHash { get; set; }
    }
}