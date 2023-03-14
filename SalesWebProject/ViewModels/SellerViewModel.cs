﻿using SalesWebProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SalesWebProject.ViewModels
{
    public class SellerMainViewModel
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public List<SellerViewModel> Sellers { get; set; }
    }

    public class SellerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo {0} necessário")]
        [Display(Name = "Nome")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O {0} deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo {0} necessário")]
        [EmailAddress(ErrorMessage = "Entre com um e-mail válido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} necessário")]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "0:dd/MM/yyyy")]
        public DateTime BirthDate { get; set; }
        public string BirthDateString { get { return this.BirthDate.ToShortDateString(); } }


        [Required(ErrorMessage = "Campo {0} necessário")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        [Display(Name = "Salário Base")]
        public double BaseSalary { get; set; }

        public string BaseSalaryMonetary { get { return this.BaseSalary.ToString("C", new CultureInfo("pt-Br")); } }

        public int DepartmentId { get; set; }

        [Display(Name = "Departamento")]
        public Department Department { get; set; }

        public List<SalesRecordViewModel> Sales { get; set; } = new List<SalesRecordViewModel>();

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(m => m.Date >= initial && m.Date <= final).Sum(m => m.Amount);
        }

    }
}
