using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CriptoSofttek.Entities
{
    public class TypeMovement
    {
        public TypeMovement() { }


        [Key]
        public int Id { get; set; }
        public string description { get; set; }

    }
}
