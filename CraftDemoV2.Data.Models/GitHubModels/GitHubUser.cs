using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static CraftDemoV2.Common.GitHubValidations.UserValidations.UserValidations;

namespace CraftDemoV2.Data.Models.GitHubModels
{
    //This is the model used for the database table.
    public class GitHubUser
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxUserNameLength)]
        [Unicode(false)]
        public string UserName { get; set; } = null!;


        [Required] 
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
