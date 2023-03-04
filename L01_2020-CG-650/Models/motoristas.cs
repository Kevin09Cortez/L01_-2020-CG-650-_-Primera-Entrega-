using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace L01_2020_CG_650.Models
{
    public class motoristas
    {
        [Key]
        public int motoristaId { get; set; }
        public string nombreMotorista { get; set; }
    }
}
