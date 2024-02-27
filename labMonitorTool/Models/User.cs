using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations; // Validation via Data Annotations

// Added for use of SQL Database components
using System.Data;
using System.Data.SqlClient;

// Added in order to use IConfiguration, so we can get our DB Connection string from the appsettings.json file
using Microsoft.Extensions.Configuration;

// Adedd for Session vars
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace labMonitor.Models
{
    public class User
    {
        //--- Notes --->>>
        //  userID, int, PK
        //  userFName, string (32)
        //  userLName, string (32)
        //  userPassword, string (255)
        //  userSalt, string (32)
        //  userDept, int, FK
        //  userPrivilege, int
        //-------------->>>


        [Required]
        public int userID { get; set; }                 //  userID, int, PK

        [Required, StringLength(32)]
        public string userFName { get; set; }           //  userFName, string (32)

        [Required, StringLength(32)]
        public string userLName { get; set; }           //  userLName, string (32)

        [Required, StringLength(255)]
        public string userPassword { get; set; }        //  userPassword, string (255)

        [Required, StringLength(32)]
        public string userSalt { get; set; }            //  userSalt, string (32)

        [Required]
        public int userDept { get; set; }               //  userDept, int, FK

        [Required]
        public int userPrivilege { get; set; }          //  userPrivilege, int

        public string userFeedback { get; set; }
    }
}