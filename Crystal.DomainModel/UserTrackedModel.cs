using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crystal.DomainModel
{
    public class ObservedDevice
    {
        public string Kpr { get; set; }
        public string Napr { get; set; }
        public string ProdNumber { get; set; }
        public int Nzp { get; set; }
        public int Nnprt { get; set; }

    }

    public class ObservedBatch
    {
        public string Nprt { get; set; }
        public string Napr { get; set; }
        public string Naop { get; set; }
        public decimal Nzp { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Date { get; set; }
        public string ProdNumber { get; set; }

        public int DaysOnOperation { get; set; }
    }
}
