﻿using Microsoft.AspNetCore.Identity;

namespace PustokMVC.Models;

public class AppUser:IdentityUser
{
    public string FullName { get; set; }    
}
