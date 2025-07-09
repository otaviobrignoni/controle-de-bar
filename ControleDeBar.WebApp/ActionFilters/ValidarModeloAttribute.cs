using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControleDeBar.WebApp.ActionFilters;

public class ValidarModeloAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var modelstate = context.ModelState;

        if (!modelstate.IsValid)
        {
            var controller = (Controller)context.Controller;

            context.ActionArguments.Values.FirstOrDefault(x => x.GetType().Name.EndsWith("ViewModel"));

            context.Result = controller.View();
        }
    }
}
