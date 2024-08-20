using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Not.Again.Domain
{
    public class TestRun
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TestRunId { get; set; }

        [Required, ForeignKey(nameof(TestRecord))]
        public Guid TestRecordId { get; set; }

        public virtual TestRecord TestRecord { get; set; }

        public DateTime RunDate { get; set; }

        public int Result { get; set; }

        public long TotalDuration { get; set; }
    }
}