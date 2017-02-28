using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace NetCoreAspGenericControllers.Controllers
{
    public class CustomControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var isController = base.IsController(typeInfo);

            if (!isController)
            {
                isController =
                    !typeInfo.IsInterface && (
                        typeInfo.Name.EndsWith("Controller`1", StringComparison.OrdinalIgnoreCase) 
                    )
                ;
            }

            if (isController)
            {
                // Output Detected Controllers
                Console.WriteLine($"{typeInfo.Name} IsController: {isController}.");
            }

            return isController;
        }
    }
}
