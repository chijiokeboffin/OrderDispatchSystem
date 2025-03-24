using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Domain.Entities
{
    public class ReadyForDispatch
    {
        [Key]
        public int Id { get; set; }
        public int ControlNumber { get; set; }

        [Required]
        public string SalesOrder { get; set; }

        [Required]
        public List<Container> Containers { get; set; } = new List<Container>();

        [Required]
        public DeliveryAddress DeliveryAddress { get; set; }
    }

    public class Container
    {
        [Key]
        public string LoadId { get; set; }

        [Required]
        public string ContainerType { get; set; }

        [Required]
        public List<Item> Items { get; set; } = new List<Item>();

        [ForeignKey("ReadyForDispatch")]
        public int ReadyForDispatchId { get; set; }
        public ReadyForDispatch ReadyForDispatch { get; set; }
    }
   

    public class Item
    {
        [Key]
        public int Id { get; set; } // Auto-generated primary key

        [Required]
        public string ItemCode { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double CartonWeight { get; set; }

        [ForeignKey("Container")]
        public string LoadId { get; set; }
        public Container Container { get; set; }
    }

    public class DeliveryAddress
    {
        [Key]
        public int Id { get; set; } // Auto-generated primary key

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        [ForeignKey("ReadyForDispatch")]
        public int ReadyForDispatchId { get; set; }
        public ReadyForDispatch ReadyForDispatch { get; set; }
    }
}
