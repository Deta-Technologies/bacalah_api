using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace bacalah.Entities.Entities;

[Table("users")]
public class User : IdentityUser
{

}