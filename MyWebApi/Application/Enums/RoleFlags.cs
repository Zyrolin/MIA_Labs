using System;

namespace MyWebApi.Entities;

[Flags]
public enum Roles
{
    None     = 0,
    Customer = 1 << 0,
    Admin    = 1 << 1, 

}
