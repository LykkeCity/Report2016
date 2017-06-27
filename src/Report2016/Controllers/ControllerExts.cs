using Microsoft.AspNetCore.Mvc;
using Report2016.Helpers;

namespace Report2016.Controllers
{
    public static class ControllerExts
    {


        public static string GetUserId(this Controller ctx){
            return  ctx.User.Identity.GetUserId();
        }
    }
}
