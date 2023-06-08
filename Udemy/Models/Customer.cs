using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class Customer
    {
        [Key]
        [StringLength(5)]
        public string CustomerID { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(30)]
        public string ContactName { get; set; }

        [StringLength(30)]
        public string ContactTitle { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
